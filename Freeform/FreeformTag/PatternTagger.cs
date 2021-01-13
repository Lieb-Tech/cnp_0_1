using Common;
using Common.Processing;

namespace Freeform.FreeformTag
{
    public class PatternTagger
    {
        private readonly IStrategy<TextSpan> preProcess;
        private readonly IStrategy<TextSpan> process;

        public PatternTagger()
        {
            preProcess = new DotStrategy()
                .Then(new TagRegex(@"(([1-9][0-9])|([1-9]))(st|nd|rd|th)", "gen:num"))   // 1st, 2nd
                .Then(new TagRegex(@"(([0-9]+\-[0-9]+)|[0-9]+) range", "gen:num"))   //range
                .Then(new TagRegex(@"[0-9]+\+", "gen:num"))    // 1+

                .Then(new TagRegex(@"[0-9]+\s?(\\|/|-|to)\s?[0-9]+", "med:num:"))
                .Then(new TagRegex(@"([0-9]+,)*[0-9]+(\.[0-9]+)?", "med:num:"))
                .Then(new TagRegex(@"[0-9]+", "med:num:"));


            process = new TagRegex("Gravida [0-9]{1,2} Para ([0-9]{1,2})([0-9]{1,2})([0-9]{1,2})([0-9]{1,2})", "gen:maternity")
                .Then(new TagRegex("[0-9]{2-4] gram (fe)?male infant", "gen:maternity"))
                .Then(new TagRegex(@"apgar score(s?) (of\s)?[0-9](\sand|/|\\|\-)[0-9]", "gen:maternity"))
                .Then(new TagRegex(@"[0-9]{,2}(\.[0-9])? weeks(\sgestation)?", "gen:maternity"))

                .Then(new TagRegex(@"[0-9]{1,3}(\-|\s)(year(s?)|month(s?))(\-|\s)old", "gen:age")) // age
                .Then(new TagRegex(@"(fe)?male|woman|man", "gen:patient"));
        }

        public TextSpan ProcessLine(string line)
        {
            var txt = new TextSpan(line, line);
            var ctx = new StrategyContext<TextSpan>(txt, true);
            ctx = preProcess.Execute(ctx);
            ctx = process.Execute(ctx);
            return ctx.Data;
        }
    }
}
