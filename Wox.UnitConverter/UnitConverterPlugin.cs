using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using Wox.EasyHelper;
using Wox.Plugin;
using Wox.UnitConverter.Service;

namespace Wox.UnitConverter
{
    public class UnitConverterPlugin : PluginBase<WoxUnitResultFinder>
    {
        public override WoxUnitResultFinder PrepareContext()
        {
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            return new WoxUnitResultFinder(WoxContextService, unitService);
        }
    }
}