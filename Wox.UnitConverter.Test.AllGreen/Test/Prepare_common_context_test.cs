using AllGreen.Lib;
using Wox.UnitConverter.Test.AllGreen.Fixture;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.Test.AllGreen.Test
{
    public class Prepare_common_context_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .Using<Wox_bar_fixture>()

            .DoAction(f => f.Start_the_bar())
            .DoAction(f => f.Display_wox())
            .DoCheck(f => f.The_current_query_is(), "")

            .EndUsing()

            .EndTest();
    }
}