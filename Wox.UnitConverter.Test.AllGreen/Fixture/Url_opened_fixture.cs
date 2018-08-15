using AllGreen.Lib;
using System.Collections.Generic;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.Test.AllGreen.Fixture
{
    public class Url_opened_fixture : FixtureBase<UnitConverterContext>
    {
        public override IEnumerable<object> OnQuery()
        {
            foreach (var url in Context.ApplicationStarter.SystemService.UrlOpened)
            {
                yield return new Result { Url = url };
            }
        }

        public class Result
        {
            public string Url { get; set; }
        }
    }
}