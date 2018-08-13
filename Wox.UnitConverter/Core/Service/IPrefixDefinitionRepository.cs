using System.Collections.Generic;
using Wox.UnitConverter.DomainModel;

namespace Wox.UnitConverter.Core.Service
{
    public interface IPrefixDefinitionRepository
    {
        void Init();

        void AddPrefixDefinition(PrefixDefinition prefixDefinition);

        IEnumerable<PrefixDefinition> GetPrefixDefinitions();
    }
}