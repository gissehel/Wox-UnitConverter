using System;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.Tool;

namespace Wox.UnitConverter.Service
{
    public class WoxUnitResultFinder : WoxResultFinderBase
    {
        public IUnitConversionService UnitConversionService { get; }

        public WoxUnitResultFinder(IWoxContextService woxContextService, IUnitConversionService unitConversionService)
            : base(woxContextService)
        {
            UnitConversionService = unitConversionService;
        }

        public void Init()
        {
            UnitConversionService.Init();
        }

        public IEnumerable<WoxResult> CreateNewUnit(string search)
        {
            var fields = SeparateWithSymbolAndName(search, "=");
            if (fields != null)
            {
                var name = fields.Item1;
                var symbol = fields.Item2;
                var unitString = fields.Item3;

                var result = UnitConversionService.PrepareNewUnitCreation(name, symbol, unitString);

                if (result != null)
                {
                    yield return GetActionResult(result.Item1, result.Item2, () => UnitConversionService.CreateNewUnit(name, symbol, unitString));
                }
            }
        }

        public IEnumerable<WoxResult> CreateNewPrefix(string search)
        {
            var fields = SeparateWithSymbolAndName(search, "~");
            if (fields != null)
            {
                var name = fields.Item1;
                var symbol = fields.Item2;
                var valueString = fields.Item3;

                valueString = valueString.Replace(" ", "");

                bool inverted = false;
                if (valueString.StartsWith("1/"))
                {
                    inverted = true;
                    valueString = valueString.Substring(2);
                }

                var scalar = (new ScalarFloat()).Parse(valueString) as ScalarFloat;

                var result = UnitConversionService.PrepareNewPrefixCreation(name, symbol, scalar, inverted);

                if (result != null)
                {
                    yield return GetActionResult(result.Item1, result.Item2, () => UnitConversionService.CreateNewPrefix(name, symbol, scalar, inverted));
                }
            }
        }

        private Tuple<string, string, string> SeparateWithSymbolAndName(string search, string affectationOperator)
        {
            var operatorFields = search.SeparateAndTrim(affectationOperator);
            if (operatorFields == null)
            {
                return null;
            }
            var symbol = operatorFields.Item1;
            var valueString = operatorFields.Item2;
            var name = symbol;

            var bracketFileds = valueString.SeparateAndTrim("[");
            if (bracketFileds != null)
            {
                valueString = bracketFileds.Item1;
                name = bracketFileds.Item2.TrimStart('[').TrimEnd(']');
            }

            return new Tuple<string, string, string>(name, symbol, valueString);
        }

        public IEnumerable<WoxResult> ConvertUnit(string search)
        {
            var result = UnitConversionService.Convert(search);
            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, () => { });
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