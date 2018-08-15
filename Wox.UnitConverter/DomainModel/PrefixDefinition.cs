namespace Wox.UnitConverter.DomainModel
{
    public class PrefixDefinition
    {
        public string Symbol { get; set; }

        public string Name { get; set; }

        public string Definition { get; set; }

        public float Factor { get; set; }

        public bool Inverted { get; set; }
    }
}