using Wox.EasyHelper.Core.Service;
using Wox.EasyHelper.DomainModel;
using Wox.Plugin;

namespace Wox.EasyHelper.Service
{
    public class QueryService : IQueryService
    {
        public WoxQuery GetWoxQuery(Query pluginQuery)
        {
            var searchTerms = pluginQuery.Search.Split(' ');
            return new WoxQuery
            {
                InternalQuery = pluginQuery,
                RawQuery = pluginQuery.RawQuery,
                Search = pluginQuery.Search,
                SearchTerms = searchTerms,
                Command = pluginQuery.RawQuery.Split(' ')[0],
            };
        }
    }
}