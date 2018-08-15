using FluentDataAccess.Core.Service;
using System;
using System.Diagnostics;
using System.IO;
using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Service
{
    public class SystemService : ISystemService, IDataAccessConfigurationService
    {
        public SystemService(string applicationName)
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

        public void OpenUrl(string url)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true,
                    RedirectStandardOutput = false,
                    CreateNoWindow = false,
                }
            };

            try
            {
                proc.Start();
            }
            catch (Exception)
            {
                // TODO : Find something usefull here...
            }
        }
    }
}