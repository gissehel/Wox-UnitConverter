using System;
using System.Collections.Generic;
using Unit.Lib.Core.DomainModel;

namespace Wox.UnitConverter.Core.Service
{
    public interface IUnitConversionService
    {
        void Init();

        Tuple<string, string> Convert(string search);

        Tuple<string, string, Action> PrepareNewUnitCreation(string line);

        void CreateNewUnit(string name, string symbol, string definiton);

        Tuple<string, string, Action> PrepareNewPrefixCreation(string line);

        void CreateNewPrefix(string name, string symbol, ScalarFloat factor, bool inverted, string definition);

        void ExportTo(string filename);

        void ImportFrom(string filename);

        bool CanImportFrom(string filename);

        IEnumerable<UnitBaseName<ScalarFloat, float>> GetUnitBaseNames();

        IEnumerable<UnitPrefix<ScalarFloat, float>> GetUnitPrefixes();
    }
}