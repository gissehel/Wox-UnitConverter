using AllGreen.Lib;
using Wox.UnitConverter.Test.AllGreen.Fixture;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.Test.AllGreen.Test
{
    public class Simple_conversion_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_test>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit convert 30g"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("30g ( gram -> kilogram )", "0.03 kg")
            .EndUsing()

            .EndTest();
    }
}