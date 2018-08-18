using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.Service;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.Service;

namespace Wox.UnitConverter
{
    public class UnitConverterPlugin : WoxPlugin
    {
        public override IWoxResultFinder PrepareContext()
        {
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            ISystemService systemService = new SystemService("Wox.UnitConverter");
            IDataAccessConfigurationService dataAccessConfigurationService = new DataAccessConfigurationService(systemService);
            IDataAccessService dataAccessService = new DataAccessService(dataAccessConfigurationService);
            IPrefixDefinitionRepository prefixDefinitionRepository = new PrefixDefinitionRepository(dataAccessService);
            IUnitDefinitionRepository unitDefinitionRepository = new UnitDefinitionRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorService();
            IFileReaderService fileReaderService = new FileReaderService();
            IUnitConversionService unitConversionService = new UnitConversionService(unitService, dataAccessService, prefixDefinitionRepository, unitDefinitionRepository, fileGeneratorService, fileReaderService);

            return new WoxUnitResultFinder(WoxContextService, unitConversionService, systemService);
        }
    }
}