using System.Collections.Generic;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Core.Service
{
    public interface IUnitDefinitionRepository
    {
        void Init();

        void AddUnitDefinition(UnitDefinition unitDefinition);

        IEnumerable<UnitDefinition> GetUnitDefinitions();
    }
}