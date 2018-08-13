using FluentDataAccess.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.UnitConverter.Core.Service;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Service
{
    public class UnitDefinitionRepository : IUnitDefinitionRepository
    {
        public IDataAccessService DataAccessService { get; }

        public UnitDefinitionRepository(IDataAccessService dataAccessService)
        {
            DataAccessService = dataAccessService;
        }

        public void Init()
        {
            DataAccessService
                .GetQuery("create table if not exists unitdefinition (id integer primary key, name text, symbol text, definition text);")
                .Execute();
        }

        public void AddUnitDefinition(UnitDefinition unitDefinition)
        {
            DataAccessService
                .GetQuery("insert into unitdefinition (name, symbol, definition) values (@name, @symbol, @definition);")
                .WithParameter("name", unitDefinition.Name)
                .WithParameter("symbol", unitDefinition.Symbol)
                .WithParameter("definition", unitDefinition.Definition)
                .Execute();
        }

        public IEnumerable<UnitDefinition> GetUnitDefinitions()
        {
            return DataAccessService
                .GetQuery("select name, symbol, definition  from unitdefinition order by id;")
                .Returning<UnitDefinition>()
                .Reading("name", (u, value) => u.Name = value)
                .Reading("symbol", (u, value) => u.Symbol = value)
                .Reading("definition", (u, value) => u.Definition = value)
                .Execute();
        }
    }
}