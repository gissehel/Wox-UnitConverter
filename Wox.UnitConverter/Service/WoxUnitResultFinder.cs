using System;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Service
{
    public class WoxUnitResultFinder : WoxResultFinderBase
    {
        private IUnitService<ScalarFloat, float> UnitService { get; }
        public IPrefixDefinitionRepository PrefixDefinitionRepository { get; }
        public IUnitDefinitionRepository UnitDefinitionRepository { get; }

        public WoxUnitResultFinder(IWoxContextService woxContextService, IUnitService<ScalarFloat, float> unitService, IPrefixDefinitionRepository prefixDefinitionRepository, IUnitDefinitionRepository unitDefinitionRepository)
            : base(woxContextService)
        {
            UnitService = unitService;
            PrefixDefinitionRepository = prefixDefinitionRepository;
            UnitDefinitionRepository = unitDefinitionRepository;
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

        private Tuple<string, string> Separate(string value, string pattern)
        {
            var patternIndex = value.IndexOf(pattern);
            if (patternIndex < 0)
            {
                return null;
            }
            var left = value.Substring(0, patternIndex);
            var right = value.Substring(patternIndex + pattern.Length, value.Length - (patternIndex + pattern.Length));
            return new Tuple<string, string>(left, right);
        }

        public IEnumerable<WoxResult> CreateNewUnit(string search)
        {
            var equalsFields = Separate(search, "=");
            var symbol = equalsFields.Item1.Trim(' ', '\t', '\r', '\n');
            var unitString = equalsFields.Item2.Trim(' ', '\t', '\r', '\n');
            var name = symbol;

            var bracketFileds = Separate(unitString, "[");
            if (bracketFileds != null)
            {
                unitString = bracketFileds.Item1;
                name = bracketFileds.Item2.TrimStart(' ', '\t', '[').TrimEnd(' ', '\t', ']');
            }

            var unit = UnitService.Parse(unitString);

            yield return new WoxResult
            {
                Title = "Create unit [{0}] (with name [{2}]) with value [{1}]".FormatWith(symbol, unit.AsString, name),
                SubTitle = "Define {0} with name {2} as {1} ({3})".FormatWith(symbol, unit.AsString, name, unit.UnitElement.AsString),
                Action = () =>
                {
                    UnitService.AddUnit(unit, name, symbol, "user");
                    UnitDefinitionRepository.AddUnitDefinition(new UnitDefinition { Name = name, Symbol = symbol, Definition = unit.AsString });
                },
                ShouldClose = true,
            };
        }

        public IEnumerable<WoxResult> CreateNewPrefix(string search)
        {
            var tildeFields = Separate(search, "~");
            var symbol = tildeFields.Item1.Trim(' ', '\t', '\r', '\n');
            var valueString = tildeFields.Item2.Trim(' ', '\t', '\r', '\n');
            var name = symbol;

            var bracketFileds = Separate(valueString, "[");
            if (bracketFileds != null)
            {
                valueString = bracketFileds.Item1;
                name = bracketFileds.Item2.TrimStart(' ', '\t', '[').TrimEnd(' ', '\t', ']');
            }

            valueString = valueString.Replace(" ", "");

            bool inverted = false;
            if (valueString.StartsWith("1/"))
            {
                inverted = true;
                valueString = valueString.Substring(2);
            }

            var scalar = (new ScalarFloat()).Parse(valueString) as ScalarFloat;

            yield return new WoxResult
            {
                Title = "Create prefix [{0}] (with name [{2}]) with prefix value [{1}{3}]".FormatWith(symbol, scalar.AsString, name, inverted ? " inverted" : ""),
                SubTitle = "Define {0} with name {2} as {1}{3}".FormatWith(symbol, scalar.AsString, name, inverted ? " inverted" : ""),
                Action = () =>
                {
                    UnitService.AddPrefix(name, symbol, "user", scalar, inverted);
                    PrefixDefinitionRepository.AddPrefixDefinition(new PrefixDefinition { Name = name, Symbol = symbol, Factor = scalar.Value, Inverted = inverted });
                },
                ShouldClose = true,
            };
        }

        public IEnumerable<WoxResult> ConvertUnit(string search)
        {
            string title = search;
            string output = null;
            try
            {
                var unitString = search;
                string targetUnitString = null;
                string targetExtraUnitString = null;
                var arrowFields = Separate(search, "->");
                if (arrowFields != null)
                {
                    unitString = arrowFields.Item1;
                    targetUnitString = arrowFields.Item2;
                }

                if (targetUnitString != null)
                {
                    var doubleComaFields = Separate(targetUnitString, ":");
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

            if (output != null)
            {
                yield return new WoxResult
                {
                    Title = title,
                    SubTitle = output,
                    ShouldClose = false,
                };
            }
        }

        public override IEnumerable<WoxResult> GetResults(WoxQuery query)
        {
            var search = query.Search;
            if (search.Contains("="))
            {
                return CreateNewUnit(search);
            }
            else if (search.Contains("~"))
            {
                return CreateNewPrefix(search);
            }
            else
            {
                return ConvertUnit(search);
            }
        }
    }
}