using FluentDataAccess.Core.Service;

namespace Wox.UnitConverter.Test.AllGreen.Mock
{
    public class DataAccessConfigurationServiceMock : IDataAccessConfigurationService
    {
        public string ApplicationDataPath { get; set; }

        public string DatabaseName => "Wox.UnitConverter.AllGreen";
    }
}