using AllGreen.Lib;
using System.Collections.Generic;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.AllGreen.Fixture
{
    public class Wox_results_fixture : FixtureBase<UnitConverterContext>
    {
        public class Result
        {
            public string Title { get; set; }

            public string SubTitle { get; set; }
        }

        public override IEnumerable<object> OnQuery()
        {
            foreach (var result in Context.ApplicationStarter.WoxContextService.Results)
            {
                yield return new Result
                {
                    Title = result.Title,
                    SubTitle = result.SubTitle,
                };
            }
        }
    }
}