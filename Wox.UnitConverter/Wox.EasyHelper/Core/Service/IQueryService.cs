using Wox.EasyHelper.DomainModel;
using Wox.Plugin;

namespace Wox.EasyHelper.Core.Service
{
    public interface IQueryService
    {
        WoxQuery GetWoxQuery(Query pluginQuery);
    }
}