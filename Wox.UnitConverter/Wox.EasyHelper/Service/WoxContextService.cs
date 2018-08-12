using System;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.DomainModel;
using Wox.Plugin;

namespace Wox.EasyHelper.Service
{
    public class WoxContextService : IWoxContextService
    {
        private PluginInitContext Context { get; set; }

        public WoxContextService(PluginInitContext context)
        {
            Context = context;
        }

        public string ActionKeyword => Context.CurrentPluginMetadata.ActionKeyword;

        public string Seperater => Query.Seperater;

        public void ChangeQuery(string query) => Context.API.ChangeQuery(query);

        public string IconPath => Context.CurrentPluginMetadata.FullIcoPath;

        public WoxResult GetActionResult(string title, string subTitle, Action action) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () =>
            {
                action();
                // ChangeQuery("");
            },
            ShouldClose = true,
        };

        public WoxResult GetCompletionResult(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => ChangeQuery(ActionKeyword + Seperater + getNewQuery() + Seperater),
            ShouldClose = false,
        };

        public WoxResult GetCompletionResultFinal(string title, string subTitle, Func<string> getNewQuery) => new WoxResult
        {
            Title = title,
            SubTitle = subTitle,
            Action = () => ChangeQuery(ActionKeyword + Seperater + getNewQuery()),
            ShouldClose = false,
        };
    }
}