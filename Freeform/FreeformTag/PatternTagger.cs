using Common;
using Common.Processing;

namespace Freeform.FreeformTag
{
    public class PatternTagger
    {
        private readonly IStrategy<TextSpan> process;

        public PatternTagger()
        {
            process = new TagRegex(@"(\d+\/\d+)\s?,?\s?\d+\.?\d+\s?,?\s?\d+\s?,?\s?\d+", "gen:vitals")
                .Then(new DotStrategy())
                .Then(new LineNumBuilder())
                .Then(new DateBuilder())
                .Then(new TagRegex(@"(\d+(\.?\d?(\s?))x?)+ (centimeter(s?))?|(cm(s?))?|(millimeter(s?))?|(mm(s?)?){1}", "gen:1:num"))
                .Then(new TagRegex(@"(([1-9][0-9])|([1-9]))(st|nd|rd|th)", "gen:2:num"))   // 1st, 2nd
                .Then(new TagRegex(@"(([0-9]+\-[0-9]+)|[0-9]+) range", "gen:3:num"))   //range
                .Then(new TagRegex(@"\w\d+\+\w", "gen:4:num"))    // 1+

                .Then(new TagRegex(@"\w[0-9]+\s?(\\|\/|-|to)\s?[0-9]+\w", "gen:4:num:"))
                .Then(new TagRegex(@"\w([0-9]+,)*[0-9]+(\.[0-9]+)?\w", "gen:3:num:"))
                .Then(new TagRegex(@"\w\d+\w", "gen:2:num:"))
                
                .Then(new TagRegex(@"Gravida [0-9]{1,2}(\s?Para ([0-9]{1,2})([0-9]{1,2})([0-9]{1,2})([0-9]{1,2}))?", "gen:maternity"))
                .Then(new TagRegex("[0-9]{2-4] gram (fe)?male infant", "gen:maternity"))
                .Then(new TagRegex(@"apgar score(s?) (of\s)?[0-9](\sand|/|\\|\-)[0-9]", "gen:maternity"))
                .Then(new TagRegex(@"[0-9]{,2}(\.[0-9])? weeks(\sgestation)?", "gen:maternity"))

                .Then(new TagRegex(@"(S1 and S2)|(S1)|(S2)", "gen:measure"))

                .Then(new TagRegex(@"[0-9]{1,3}(\-|\s)(year(s?)|month(s?))(\-|\s)old", "gen:patient:age")) // age
                .Then(new TagRegex(@"\b(fe)?male|woman|man\b", "gen:gender"));
        }

        public TextSpan ProcessLine(string line)
        {
            var txt = new TextSpan(line, line);
            var ctx = new StrategyContext<TextSpan>(txt, true);
            ctx = process.Execute(ctx);
            return ctx.Data;
        }
    }
}
