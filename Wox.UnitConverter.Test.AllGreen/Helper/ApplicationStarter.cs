﻿using FluentDataAccess.Core.Service;
using FluentDataAccess.Service;
using System;
using System.IO;
using System.Reflection;
using Unit.Lib.Core.DomainModel;
using Unit.Lib.Core.Service;
using Unit.Lib.Service;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.Test.Mock.Service;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.Service;

namespace Wox.UnitConverter.Test.AllGreen.Helper
{
    public class ApplicationStarter
    {
        public WoxContextServiceMock WoxContextService { get; set; }

        public QueryServiceMock QueryService { get; set; }

        public IWoxResultFinder WoxUnitResultFinder { get; set; }

        public SystemServiceMock SystemService { get; set; }

        private string TestName { get; set; }
        public string TestPath => GetApplicationDataPath();

        public void Init(string testName)
        {
            TestName = testName;
            QueryServiceMock queryService = new QueryServiceMock();
            WoxContextServiceMock woxContextService = new WoxContextServiceMock(queryService);
            var constantProvider = new ConstantProvider<ScalarFloat, float>();
            IUnitService<ScalarFloat, float> unitService = new UnitService<ScalarFloat, float>(constantProvider);
            SystemServiceMock systemService = new SystemServiceMock();
            IDataAccessConfigurationService dataAccessConfigurationService = new DataAccessConfigurationService(systemService);
            IDataAccessService dataAccessService = new DataAccessService(dataAccessConfigurationService);
            IPrefixDefinitionRepository prefixDefinitionRepository = new PrefixDefinitionRepository(dataAccessService);
            IUnitDefinitionRepository unitDefinitionRepository = new UnitDefinitionRepository(dataAccessService);
            IFileGeneratorService fileGeneratorService = new FileGeneratorServiceMock();
            IFileReaderService fileReaderService = new FileReaderServiceMock();
            IUnitConversionService unitConversionService = new UnitConversionService(unitService, dataAccessService, prefixDefinitionRepository, unitDefinitionRepository, fileGeneratorService, fileReaderService);

            WoxUnitResultFinder woxUnitResultFinder = new WoxUnitResultFinder(woxContextService, unitConversionService, systemService);

            systemService.ApplicationDataPath = TestPath;

            dataAccessService.Init();
            woxUnitResultFinder.Init();

            WoxContextService = woxContextService;
            QueryService = queryService;
            WoxUnitResultFinder = woxUnitResultFinder;
            SystemService = systemService;

            WoxContextService.AddQueryFetcher("unit", WoxUnitResultFinder);
        }

        public void Start()
        {
            // Make any initialisation that should occur AFTER instanciation.
        }

        private static string GetThisAssemblyDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var thisAssemblyCodeBase = assembly.CodeBase;
            var thisAssemblyDirectory = Path.GetDirectoryName(new Uri(thisAssemblyCodeBase).LocalPath);

            return thisAssemblyDirectory;
        }

        private string GetApplicationDataPath()
        {
            var thisAssemblyDirectory = GetThisAssemblyDirectory();
            var path = Path.Combine(Path.Combine(thisAssemblyDirectory, "AllGreen"), "AG_{0:yyyyMMdd-HHmmss-fff}_{1}".FormatWith(DateTime.Now, TestName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}