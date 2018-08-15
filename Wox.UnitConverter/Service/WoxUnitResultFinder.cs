using System;
using System.Collections.Generic;
using System.IO;
using Wox.EasyHelper;
using Wox.EasyHelper.Core.Service;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Service
{
    public class WoxUnitResultFinder : WoxResultFinderBase
    {
        public IUnitConversionService UnitConversionService { get; }
        public ISystemService SystemService { get; }

        public WoxUnitResultFinder(IWoxContextService woxContextService, IUnitConversionService unitConversionService, ISystemService systemService)
            : base(woxContextService)
        {
            UnitConversionService = unitConversionService;
            SystemService = systemService;
        }

        public void Init()
        {
            UnitConversionService.Init();
            InitCommands();
        }

        public IEnumerable<WoxResult> CreateNewUnit(string search)
        {
            var result = UnitConversionService.PrepareNewUnitCreation(search);

            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, result.Item3);
            }
        }

        public IEnumerable<WoxResult> CreateNewPrefix(string search)
        {
            var result = UnitConversionService.PrepareNewPrefixCreation(search);

            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, result.Item3);
            }
        }

        private List<CommandInfo> _commandInfos = new List<CommandInfo>();

        private void InitCommands()
        {
            AddCommand("convert", "convert UNIT [ -> UNIT [ : UNIT ]]", "Convert a value to another unit (express in a third unit)", ConvertCommand);
            AddCommand("help", "help", "Get help on this extension (web)", HelpCommand);
            AddCommand("export", "export FILENAME", "Export unit and prefix definitions to the file", ExportCommand);
            AddCommand("import", "import FILENAME", "Import unit and prefix definitions from the file", ImportCommand);
        }

        private void AddCommand(string name, string title, string subtitle, Func<WoxQuery, int, IEnumerable<WoxResult>> func)
        {
            _commandInfos.Add(new CommandInfo { Name = name, Title = title, Subtitle = subtitle, ResultGetter = func });
        }

        private void AddCommand(string name, string title, string subtitle, Action action)
        {
            _commandInfos.Add(new CommandInfo { Name = name, Title = title, Subtitle = subtitle, FinalAction = action });
        }

        public WoxResult GetDefaultCommandResult(string commandName, IEnumerable<CommandInfo> commandInfos)
        {
            foreach (var commandInfo in commandInfos)
            {
                if (commandName == commandInfo.Name)
                {
                    if (commandInfo.FinalAction != null)
                    {
                        return GetActionResult(commandInfo.Title, commandInfo.Subtitle, commandInfo.FinalAction);
                    }
                    else
                    {
                        return GetCompletionResult(commandInfo.Title, commandInfo.Subtitle, () => commandName);
                    }
                }
            }
            return null;
        }

        public IEnumerable<WoxResult> SearchCommands(WoxQuery query, int position, IEnumerable<CommandInfo> commandInfos)
        {
            var results = new List<WoxResult>();
            var term = query.GetTermOrEmpty(position);
            foreach (var commandInfo in commandInfos)
            {
                var commandName = commandInfo.Name;
                if (commandName.MatchPattern(term))
                {
                    if (term == commandName)
                    {
                        if (commandInfo.FinalAction != null)
                        {
                            results.Add(GetActionResult(commandInfo.Title, commandInfo.Subtitle, commandInfo.FinalAction));
                        }
                        else if (commandInfo.ResultGetter != null)
                        {
                            foreach (var result in commandInfo.ResultGetter(query, position + 1))
                            {
                                results.Add(result);
                            }
                        }
                    }
                    else
                    {
                        if (commandInfo.FinalAction != null)
                        {
                            results.Add(GetActionResult(commandInfo.Title, commandInfo.Subtitle, commandInfo.FinalAction));
                        }
                        else
                        {
                            results.Add(GetCompletionResult(commandInfo.Title, commandInfo.Subtitle, () => commandName));
                        }
                    }
                }
            }
            if (results.Count == 0)
            {
                return null;
            }
            return results;
        }

        public override IEnumerable<WoxResult> GetResults(WoxQuery query)
        {
            var results = SearchCommands(query, 0, _commandInfos);

            if (results != null)
            {
                return results;
            }
            else
            {
                var search = query.Search;

                if (search.Contains("="))
                {
                    return CreateNewUnit(search);
                }
                else if (search.Contains("~"))
                {
                    return CreateNewPrefix(search);
                }
                else
                {
                    return ConvertUnit(search);
                }
            }
        }

        public IEnumerable<WoxResult> ExportCommand(WoxQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (string.IsNullOrEmpty(filename))
            {
                yield return GetDefaultCommandResult("export", _commandInfos);
            }
            else
            {
                yield return GetActionResult("export {0}".FormatWith(filename), "Export unit and prefix definitions to filename [{0}]".FormatWith(filename), () => Export(filename));
            }
        }

        private void Export(string filename)
        {
            var fullFilename = GetImportExportFullFilename(filename);
            UnitConversionService.ExportTo(fullFilename);
            SystemService.OpenUrl(SystemService.ApplicationDataPath);
        }

        private IEnumerable<WoxResult> ImportCommand(WoxQuery query, int position)
        {
            var filename = query.GetAllSearchTermsStarting(position);
            if (string.IsNullOrEmpty(filename))
            {
                yield return GetDefaultCommandResult("import", _commandInfos);
            }
            else
            {
                var fullFilename = GetImportExportFullFilename(filename);
                if (UnitConversionService.CanImportFrom(fullFilename))
                {
                    yield return GetActionResult("import {0}".FormatWith(filename), "Import unit and prefix definitions from filename [{0}]".FormatWith(filename), () => Import(fullFilename));
                }
                else
                {
                    yield return new WoxResult { Title = "import {0}".FormatWith(filename), SubTitle = "Import unit and prefix definitions from filename [{0}] (No file found!)".FormatWith(filename) };
                }
            }
        }

        private void Import(string fullFilename)
        {
            if (UnitConversionService.CanImportFrom(fullFilename))
            {
                UnitConversionService.ImportFrom(fullFilename);
            }
        }

        private string GetImportExportFullFilename(string filename) => Path.IsPathRooted(filename) ? filename : Path.Combine(SystemService.ApplicationDataPath, filename);

        public IEnumerable<WoxResult> ConvertUnit(string search)
        {
            var result = UnitConversionService.Convert(search);
            if (result != null)
            {
                yield return GetActionResult(result.Item1, result.Item2, () => { });
            }
        }

        private IEnumerable<WoxResult> ConvertCommand(WoxQuery query, int position)
        {
            return ConvertUnit(query.GetAllSearchTermsStarting(position));
        }

        private void HelpCommand()
        {
            SystemService.OpenUrl("https://github.com/gissehel/Wox-UnitConverter");
        }
    }
}