using System;
using Wox.EasyHelper.DomainModel;

namespace Wox.EasyHelper.Core.Service
{
    public interface IWoxContextService
    {
        void ChangeQuery(string query);

        string ActionKeyword { get; }

        string Seperater { get; }

        string IconPath { get; }

        WoxResult GetActionResult(string title, string subTitle, Action action);

        WoxResult GetCompletionResult(string title, string subTitle, Func<string> getNewQuery);

        WoxResult GetCompletionResultFinal(string title, string subTitle, Func<string> getNewQuery);
    }
}