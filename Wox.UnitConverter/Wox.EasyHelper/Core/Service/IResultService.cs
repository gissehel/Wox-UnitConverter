using System.Collections.Generic;
using Wox.EasyHelper.DomainModel;
using Wox.Plugin;

namespace Wox.EasyHelper.Core.Service
{
    public interface IResultService
    {
        List<Result> MapResults(IEnumerable<WoxResult> results);
    }
}