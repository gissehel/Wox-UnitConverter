using AllGreen.Lib;
using Wox.UnitConverter.Test.AllGreen.Fixture;
using Wox.UnitConverter.Test.AllGreen.Helper;

namespace Wox.UnitConverter.Test.AllGreen.Test
{
    public class Show_commands_test : TestBase<UnitConverterContext>
    {
        public override void DoTest() =>
            StartTest()

            .IsRunnable()

            .Include<Prepare_common_context_test>()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert UNIT [ -> UNIT [ : UNIT ]]", "Convert a value to another unit (express in a third unit)")
            .Check("help", "Get help on this extension (web)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit e"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert UNIT [ -> UNIT [ : UNIT ]]", "Convert a value to another unit (express in a third unit)")
            .Check("help", "Get help on this extension (web)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit e"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert UNIT [ -> UNIT [ : UNIT ]]", "Convert a value to another unit (express in a third unit)")
            .Check("help", "Get help on this extension (web)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit n"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("convert UNIT [ -> UNIT [ : UNIT ]]", "Convert a value to another unit (express in a third unit)")
            .EndUsing()

            .Using<Wox_bar_fixture>()
            .DoAction(f => f.Write_query(@"unit nm"))
            .EndUsing()

            .UsingList<Wox_results_fixture>()
            .With<Wox_results_fixture.Result>(f => f.Title, f => f.SubTitle)
            .Check("nm ( nanometre -> metre )", "1E-09 m")
            .EndUsing()

            .EndTest();
    }
}