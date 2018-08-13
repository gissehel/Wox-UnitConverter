using FluentDataAccess.Core.Service;
using System;
using System.IO;

namespace Wox.UnitConverter.Service
{
    public class DataAccessConfigurationService : IDataAccessConfigurationService
    {
        public DataAccessConfigurationService(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public string ApplicationDataPath => GetApplicationDataPath();

        private string GetApplicationDataPath()
        {
            var appDataPathParent = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDataPath = Path.Combine(appDataPathParent, ApplicationName);
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            return appDataPath;
        }

        public string DatabaseName => ApplicationName;

        public string ApplicationName { get; }
    }
}