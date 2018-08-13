using FluentDataAccess.Core.Service;
using System.Collections.Generic;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Service
{
    public class PrefixDefinitionRepository : IPrefixDefinitionRepository
    {
        public IDataAccessService DataAccessService { get; }

        public PrefixDefinitionRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init()
        {
            DataAccessService
                .GetQuery("create table if not exists prefixdefinition (id integer primary key, name text, symbol text, factor float, inverted int);")
                .Execute();
        }

        public void AddPrefixDefinition(PrefixDefinition prefixDefinition)
        {
            DataAccessService
                .GetQuery("insert into prefixdefinition (name, symbol, factor, inverted) values (@name, @symbol, @factor, @inverted);")
                .WithParameter("name", prefixDefinition.Name)
                .WithParameter("symbol", prefixDefinition.Symbol)
                .WithParameter("factor", prefixDefinition.Factor)
                .WithParameter("inverted", prefixDefinition.Inverted)
                .Execute();
        }

        public IEnumerable<PrefixDefinition> GetPrefixDefinitions()
        {
            return DataAccessService
                 .GetQuery("select name, symbol, factor, inverted from prefixdefinition order by id;")
                 .Returning<PrefixDefinition>()
                 .Reading("name", (u, value) => u.Name = value)
                 .Reading("symbol", (u, value) => u.Symbol = value)
                 .Reading("factor", (PrefixDefinition u, float value) => u.Factor = value)
                 .Reading("inverted", (u, value) => u.Inverted = value)
                 .Execute();
        }
    }
}