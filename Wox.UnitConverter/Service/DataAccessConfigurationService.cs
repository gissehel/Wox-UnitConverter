using FluentDataAccess.Core.Service;
using Wox.EasyHelper.Core.Service;

namespace Wox.UnitConverter.Service
{
    public class DataAccessConfigurationService : IDataAccessConfigurationService
    {
        public DataAccessConfigurationService(ISystemService systemService)
        {
            SystemService = systemService;
        }

        private ISystemService SystemService { get; }

        public string ApplicationDataPath => SystemService.ApplicationDataPath;

        public string DatabaseName => SystemService.ApplicationName;
    }
}