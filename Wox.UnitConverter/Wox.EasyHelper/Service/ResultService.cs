using System.Collections.Generic;
using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.DomainModel;
using Wox.Plugin;

namespace Wox.EasyHelper.Service
{
    public class ResultService : IResultService
    {
        private IWoxContextService WoxContextService { get; set; }

        public ResultService(IWoxContextService woxContextService)
        {
            WoxContextService = woxContextService;
        }

        public List<Result> MapResults(IEnumerable<WoxResult> results)
        {
            var resultList = new List<Result>();
            foreach (var result in results)
            {
                var action = result.Action;
                resultList.Add(new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                    IcoPath = result.Icon ?? WoxContextService.IconPath,
                    Action = e =>
                    {
                        if (e.SpecialKeyState.CtrlPressed)
                        {
                            if (result.CtrlAction != null)
                            {
                                return result.CtrlAction();
                            }
                            return false;
                        }
                        else if (e.SpecialKeyState.WinPressed)
                        {
                            if (result.WinAction != null)
                            {
                                return result.WinAction();
                            }
                            return false;
                        }
                        else
                        {
                            action();
                            return result.ShouldClose;
                        }
                    }
                });
            }
            return resultList;
        }
    }
}