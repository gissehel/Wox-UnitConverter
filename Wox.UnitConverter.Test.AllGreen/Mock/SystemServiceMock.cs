using System.Collections.Generic;
using Wox.UnitConverter.Core.Service;

namespace Wox.UnitConverter.Test.AllGreen.Mock
{
    public class SystemServiceMock : ISystemService
    {
        public void OpenUrl(string url)
        {
            UrlOpened.Add(url);
        }

        public List<string> UrlOpened { get; private set; } = new List<string>();

        public string ApplicationDataPath { get; set; }
    }
}