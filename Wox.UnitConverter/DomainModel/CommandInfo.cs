using System;
using System.Collections.Generic;
using Wox.EasyHelper;

namespace Wox.UnitConverter.DomainModel
{
    public class CommandInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Action FinalAction { get; set; }
        public Func<WoxQuery, int, IEnumerable<WoxResult>> ResultGetter { get; set; }
    }
}