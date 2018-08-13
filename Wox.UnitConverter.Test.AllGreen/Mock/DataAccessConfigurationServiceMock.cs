using FluentDataAccess.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wox.UnitConverter.Test.AllGreen.Mock
{
    public class DataAccessConfigurationServiceMock : IDataAccessConfigurationService
    {
        public string ApplicationDataPath { get; set; }

        public string DatabaseName => "Wox.UnitConverter.AllGreen";
    }
}