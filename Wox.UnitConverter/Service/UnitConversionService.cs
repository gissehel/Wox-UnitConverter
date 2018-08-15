using System;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Wox.EasyHelper;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.DomainModel;
using Wox.UnitConverter.Tool;

namespace Wox.UnitConverter.Service
{
    public class UnitConversionService : IUnitConversionService
    {
        private IUnitService<ScalarFloat, float> UnitService { get; }
        private IPrefixDefinitionRepository PrefixDefinitionRepository { get; }
        private IUnitDefinitionRepository UnitDefinitionRepository { get; }
        public IFileGeneratorService FileGeneratorService { get; }
        public IFileReaderService FileReaderService { get; }

        public UnitConversionService(IUnitService<ScalarFloat, float> unitService, IPrefixDefinitionRepository prefixDefinitionRepository, IUnitDefinitionRepository unitDefinitionRepository, IFileGeneratorService fileGeneratorService, IFileReaderService fileReaderService)
        {
            UnitService = unitService;
            PrefixDefinitionRepository = prefixDefinitionRepository;
            UnitDefinitionRepository = unitDefinitionRepository;
            FileGeneratorService = fileGeneratorService;
            FileReaderService = fileReaderService;
        }

        public Tuple<string, string> Convert(string search)
        {
            string title = search;
            string output = null;
            try
            {
                var unitString = search;
                string targetUnitString = null;
                string targetExtraUnitString = null;
                var arrowFields = search.Separate("->");
                if (arrowFields != null)
                {
                    unitString = arrowFields.Item1;
                    targetUnitString = arrowFields.Item2;
                }

                if (targetUnitString != null)
                {
                    var doubleComaFields = targetUnitString.Separate(":");
                    if (doubleComaFields != null)
                    {
                        targetUnitString = doubleComaFields.Item1;
                        targetExtraUnitString = doubleComaFields.Item2;
                    }
                }
                var unit = UnitService.Parse(unitString);
                string inputUnit = unit.UnitElement.Name;
                string outputUnit = null;

                if (targetUnitString != null)
                {
                    var targetUnit = UnitService.Parse(targetUnitString);
                    UnitValue<ScalarFloat, float> targetExtraUnit = null;
                    if (targetExtraUnitString != null)
                    {
                        targetExtraUnit = UnitService.Parse(targetExtraUnitString);
                    }
                    var result = UnitService.Convert(UnitService.Divide(unit, targetUnit));
                    string resultAsString = null;
                    string targetUnitAsString = null;

                    if (result.UnitElement.GetDimension().QuantityCount == 0)
                    {
                        resultAsString = result.Value.Value.ToString();
                    }
                    else
                    {
                        if (targetExtraUnit != null)
                        {
                            result = UnitService.Convert(result, targetExtraUnit);
                        }
                        outputUnit = result.UnitElement.Name;
                        resultAsString = "({0})".FormatWith(result.AsString);
                    }

                    if (targetUnit.Value.Value == targetUnit.Value.GetNeutral().Value)
                    {
                        targetUnitAsString = targetUnit.UnitElement.AsString;
                    }
                    else
                    {
                        targetUnitAsString = "({0})".FormatWith(targetUnit.AsString);
                    }
                    if (targetUnit.UnitElement.GetDimension().QuantityCount != 0)
                    {
                        if (outputUnit == null)
                        {
                            outputUnit = targetUnit.UnitElement.Name;
                        }
                        else
                        {
                            outputUnit = "{0} x {1}".FormatWith(outputUnit, targetUnit.UnitElement.Name);
                        }
                    }
                    output = "{0} {1}".FormatWith(resultAsString, targetUnitAsString);
                }
                else
                {
                    var result = UnitService.Convert(unit);
                    if (result.UnitElement.GetDimension().QuantityCount == 0 && result.Value.Value == 1)
                    {
                        output = null;
                    }
                    else
                    {
                        output = result.AsString;
                        if (result.UnitElement.GetDimension().QuantityCount != 0)
                        {
                            outputUnit = result.UnitElement.Name;
                        }
                    }
                }
                if (outputUnit != null)
                {
                    title = "{0} ( {1} -> {2} )".FormatWith(title, inputUnit, outputUnit);
                }
                else
                {
                    title = "{0} ( {1} -> )".FormatWith(title, inputUnit);
                }
            }
            catch (Exception ex)
            {
                title = "Exception !";
                output = ex.Message;
            }

            return new Tuple<string, string>(title, output);
        }

        public void Init()
        {
            PrefixDefinitionRepository.Init();
            UnitDefinitionRepository.Init();

            var nullScalar = new ScalarFloat();
            foreach (var prefixDefinition in PrefixDefinitionRepository.GetPrefixDefinitions())
            {
                try
                {
                    UnitService.AddPrefix(prefixDefinition.Name, prefixDefinition.Symbol, "user", nullScalar.GetNewFromFloat(prefixDefinition.Factor) as ScalarFloat, prefixDefinition.Inverted);
                }
                catch
                {
                }
            }
            foreach (var unitDefinition in UnitDefinitionRepository.GetUnitDefinitions())
            {
                try
                {
                    var unit = UnitService.Parse(unitDefinition.Definition);
                    UnitService.AddUnit(unit, unitDefinition.Name, unitDefinition.Symbol, "user");
                }
                catch
                {
                }
            }
        }

        public void CreateNewUnit(string name, string symbol, string definiton)
        {
            var unit = UnitService.Parse(definiton);

            UnitService.AddUnit(unit, name, symbol, "user");
            UnitDefinitionRepository.AddUnitDefinition(new UnitDefinition { Name = name, Symbol = symbol, Definition = unit.AsString });
        }

        public void CreateNewPrefix(string name, string symbol, ScalarFloat factor, bool inverted, string definition)
        {
            UnitService.AddPrefix(name, symbol, "user", factor, inverted);
            PrefixDefinitionRepository.AddPrefixDefinition(new PrefixDefinition { Name = name, Symbol = symbol, Definition = definition, Factor = factor.Value, Inverted = inverted });
        }

        public void ExportTo(string filename)
        {
            using (var fileGenerator = FileGeneratorService.CreateGenerator(filename))
            {
                foreach (var prefixDefinition in PrefixDefinitionRepository.GetPrefixDefinitions())
                {
                    fileGenerator.AddLine("{0}~{1}{2} [{3}]".FormatWith(prefixDefinition.Symbol, prefixDefinition.Inverted ? "1/" : "", prefixDefinition.Factor, prefixDefinition.Name));
                }
                foreach (var unitDefinition in UnitDefinitionRepository.GetUnitDefinitions())
                {
                    fileGenerator.AddLine("{0}={1} [{2}]".FormatWith(unitDefinition.Symbol, unitDefinition.Definition, unitDefinition.Name));
                }
            }
        }

        public bool CanImportFrom(string filename) => FileReaderService.FileExists(filename);

        public void ImportFrom(string filename)
        {
            using (var fileReader = FileReaderService.Read(filename))
            {
                string line = null;
                line = fileReader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("#"))
                    {
                        line = line.Substring(0, line.IndexOf("#"));
                    }
                    line = line.Trim(' ', '\r', '\t', '\n');
                    if (line.Contains("~"))
                    {
                        AddPrefixDefintion(line);
                    }
                    else if (line.Contains("="))
                    {
                        AddUnitDefintion(line);
                    }
                    line = fileReader.ReadLine();
                }
            }
        }

        public Tuple<string, string, Action> PrepareNewUnitCreation(string line)
        {
            var fields = line.SeparateWithSymbolAndName("=");
            if (fields != null)
            {
                var name = fields.Item1;
                var symbol = fields.Item2;
                var definition = fields.Item3;

                var unit = UnitService.Parse(definition);

                return new Tuple<string, string, Action>
                    (
                        "Create unit [{0}] (with name [{2}]) with value [{1}]".FormatWith(symbol, unit.AsString, name),
                        "Define {0} with name {2} as {1} ({3})".FormatWith(symbol, unit.AsString, name, unit.UnitElement.AsString),
                        () => CreateNewUnit(name, symbol, definition)
                    );
            }
            return null;
        }

        public Tuple<string, string, Action> PrepareNewPrefixCreation(string line)
        {
            var fields = line.SeparateWithSymbolAndName("~");
            if (fields != null)
            {
                var name = fields.Item1;
                var symbol = fields.Item2;
                var definition = fields.Item3;

                definition = definition.Replace(" ", "");

                bool inverted = false;
                if (definition.StartsWith("1/"))
                {
                    inverted = true;
                    definition = definition.Substring(2);
                }

                var factor = (new ScalarFloat()).Parse(definition) as ScalarFloat;

                return new Tuple<string, string, Action>
                (
                    "Create prefix [{0}] (with name [{2}]) with prefix value [{1}{3}]".FormatWith(symbol, factor.AsString, name, inverted ? " inverted" : ""),
                    "Define {0} with name {2} as {1}{3}".FormatWith(symbol, factor.AsString, name, inverted ? " inverted" : ""),
                    () => CreateNewPrefix(name, symbol, factor, inverted, definition)
                );
            }
            return null;
        }

        private void AddUnitDefintion(string line)
        {
            PrepareNewUnitCreation(line)?.Item3?.Invoke();
        }

        private void AddPrefixDefintion(string line)
        {
            PrepareNewPrefixCreation(line)?.Item3?.Invoke();
        }

        public IEnumerable<UnitBaseName<ScalarFloat, float>> GetUnitBaseNames() => UnitService.GetUnitBaseNames();

        public IEnumerable<UnitPrefix<ScalarFloat, float>> GetUnitPrefixes() => UnitService.GetUnitPrefixes();
    }
}