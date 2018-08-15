using AllGreen.Lib;
using Wox.UnitConverter.Test.AllGreen.Fixture;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.Test.AllGreen.Test
{
    public class Get_help_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_test>()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit el"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("help", "Get help on this extension (web)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Select_line(1))
            .EndUsing()

            .UsingList<Url_opened_fixture>()
            .With<Url_opened_fixture.Result>(f => f.Url)
            .Check("https://github.com/gissehel/Wox-UnitConverter")
            .EndUsing()

            .EndTest();
    }
}