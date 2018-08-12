using AllGreen.Lib;
using Wox.UnitConverter.AllGreen.Fixture;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.AllGreen.Test
{
    public class Prepare_common_context : TestBase<UnitConverterContext>
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