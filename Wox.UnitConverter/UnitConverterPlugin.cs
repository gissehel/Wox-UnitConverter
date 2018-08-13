using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using Wox.EasyHelper;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.Service;

namespace Wox.UnitConverter
{
    public class UnitConverterPlugin : PluginBase<WoxUnitResultFinder>
    {
        public override WoxUnitResultFinder PrepareContext()
        {
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            IDataAccessConfigurationService dataAccessConfigurationService = new DataAccessConfigurationService("Wox.UnitConverter");
            IDataAccessService dataAccessService = new DataAccessService(dataAccessConfigurationService);
            IPrefixDefinitionRepository prefixDefinitionRepository = new PrefixDefinitionRepository(dataAccessService);
            IUnitDefinitionRepository unitDefinitionRepository = new UnitDefinitionRepository(dataAccessService);

            var resultFinder = new WoxUnitResultFinder(WoxContextService, unitService, prefixDefinitionRepository, unitDefinitionRepository);

            dataAccessService.Init();
            resultFinder.Init();

            return resultFinder;
        }
    }
}