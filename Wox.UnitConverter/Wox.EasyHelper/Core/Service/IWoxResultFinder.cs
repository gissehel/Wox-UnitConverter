using System.Collections.Generic;
using Wox.EasyHelper.DomainModel;

namespace Wox.EasyHelper.Core.Service
{
    public interface IWoxResultFinder
    {
        IEnumerable<WoxResult> GetResults(WoxQuery query);
    }
}