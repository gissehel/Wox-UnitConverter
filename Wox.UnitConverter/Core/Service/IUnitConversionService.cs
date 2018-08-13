using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unit.Lib.Core.DomainModel;

namespace Wox.UnitConverter.Core.Service
{
    public interface IUnitConversionService
    {
        void Init();

        Tuple<string, string> Convert(string search);

        Tuple<string, string> PrepareNewUnitCreation(string name, string symbol, string definiton);

        void CreateNewUnit(string name, string symbol, string definiton);

        Tuple<string, string> PrepareNewPrefixCreation(string name, string symbol, ScalarFloat factor, bool inverted);

        void CreateNewPrefix(string name, string symbol, ScalarFloat factor, bool inverted);
    }
}