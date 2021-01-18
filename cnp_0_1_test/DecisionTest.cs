
using Freeform.Decisions.Measurements;
using Freeform.FreeformParse;
using System.Linq;
using Xunit;

namespace cnp_0_1_test
{
    public class DecisionTest
    {
        [Fact]
        public void DecisionTree1()
        {
            var span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium}");
            var mtp = new MeasurementTreeParse();
            var ctx = mtp.ProcessLine(span);

            Assert.NotNull(ctx.Data);
            Assert.Empty(ctx.Data.TagsToProcess);
            Assert.Single(ctx.Data.Infoes);
            Assert.Equal("calcium", ctx.Data.Infoes[0].Measurement);
        }

        [Fact]
        public void DecisionTree2()
        {
            var span = new Common.TextSpan("calcium 5.2", "{gen:measure:calcium} {gen:num:5.2}");
            var mtp = new MeasurementTreeParse();
            var ctx = mtp.ProcessLine(span);

            Assert.NotNull(ctx.Data);
            Assert.Empty(ctx.Data.TagsToProcess);
            Assert.Single(ctx.Data.Infoes);
            Assert.Equal("calcium", ctx.Data.Infoes[0].Measurement);
            Assert.Equal("5.2", ctx.Data.Infoes[0].Value1);
        }

        /***************************/
        
        [Fact]
        public void DecisionTree2_2()
        {
            var span = new Common.TextSpan("calcium 5.2 potassium", "{gen:measure:calcium} {gen:num:5.2} {gen:measure:potassium}");
            var mtp = new MeasurementTreeParse();
            var ctx = mtp.ProcessLine(span);

            Assert.NotNull(ctx.Data);
            Assert.Empty(ctx.Data.TagsToProcess);
            Assert.Equal(2,ctx.Data.Infoes.Count);
            Assert.Equal("calcium", ctx.Data.Infoes[0].Measurement);
            Assert.Equal("5.2", ctx.Data.Infoes[0].Value1);
            Assert.Equal("potassium", ctx.Data.Infoes[1].Measurement);            
        }

        /***************************/

        [Fact]
        public void DecisionTree2_Other()
        {
            var span = new Common.TextSpan("calcium other 5.2", "{gen:measure:calcium} other {gen:num:5.2}");
            var mtp = new MeasurementTreeParse();
            var ctx = mtp.ProcessLine(span);

            Assert.NotNull(ctx.Data);
            Assert.Empty(ctx.Data.TagsToProcess);
            Assert.Single(ctx.Data.Infoes);
            Assert.Equal("calcium", ctx.Data.Infoes[0].Measurement);
        }

        [Fact]
        public void Decision1()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement1();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.NotNull(strat);
            Assert.IsType<Strategy1>(strat);
        }

        [Fact]
        public void Decision1_1()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement1();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium} {med:num:8.3}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}","{med:num:8.3}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.Null(strat);
        }

        [Fact]
        public void Decision2()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement2();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.Null(strat);
        }

        [Fact]
        public void Decision2_1()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement2();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium} {med:num:8.3}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}","{med:num:8.3}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.NotNull(strat);
            Assert.IsType<Strategy2>(strat);
        }

        [Fact]
        public void Decision3()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement3();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.Null(strat);
        }

        [Fact]
        public void Decision3_1()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement3();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium} {med:num:8.3} {med:range:down} {med:num:3.3}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}","{med:num:8.3}","{gen:change:down to}","{med:num:8.3}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.NotNull(strat);
            Assert.IsType<Strategy3>(strat);
        }
        
        [Fact]
        public void Decision4()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement3();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.Null(strat);
        }

        [Fact]
        public void Decision4_1()
        {
            var tree1 = new Freeform.Decisions.Measurements.Measurement4();
            var data = new Freeform.Decisions.Measurements.MeasurementData()
            {
                Span = new Common.TextSpan("gen:measure:calcium", "{gen:measure:calcium} {med:num:8.3} {med:range:to} {med:num:3.3}"),
                Tags = new System.Collections.Generic.List<string>()
                {
                    "{gen:measure:calcium}","{med:num:8.3}","{gen:range:to}","{med:num:8.3}"
                }
            };

            var strat = tree1.GetDecision(data);
            Assert.NotNull(strat);
            Assert.IsType<Strategy4>(strat);
        }
    }
}
