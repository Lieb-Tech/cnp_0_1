using Common;
using Common.Processing;

namespace Medication.MedicationTag
{
    public class MedicationTagger
    {
        private readonly IStrategy<TextSpan> preBuilder;
        private readonly IStrategy<TextSpan> builder;
        
        public MedicationTagger()
        {
            preBuilder = new TextToNumberStrategy()
                .Then(new TagRegex(@"(^\d{1,2}\.?\s)", "gen:li:"))
                .Then(new TagRegex(@"(\d{1,2}/\d{1,2}/\d{2,4})( \d{1,2}:\d{2}(:\d{0,2})? [AaPp][Mm])", "gen:datetime:"))
                .Then(new TagRegex(@"\d{1,2}:\d{1,2}:\d{2,4} [AaPp][Mm]", "gen:time:"))
                .Then(new TagRegex(@"(\d{1,2}/\d{1,2}/\d{2,4})", "gen:date:"));

             builder = new TagRegex(@"\d+\.\d+", "med:num:")
                .Then(new DotStrategy())

                .Then(new TimesADayBuilder())
                .Then(new InstructionBuilder())

                .Then(new TagRegex("prn [a-z]* pain", "med:qual:"))
                .Then(new TagRegex("(prn pain)|(prn)", "med:qual:"))

                // .Then(new TagRegex("for \\d\\sday(s)?", "med:qual:"))
                
                .Then(new TagRegex("(\\sSC\\s)|(\\ssc\\s)|(topical tp)|(TOPICAL TP)", "med:format:"))
                .Then(new TagRegex("(\\spo\\s)|(\\sPO\\s)|(sublingual)|(SUBLINGUAL)|(\\siv\\s)", "med:format:"))

                .Then(new TagRegex("(tropical\\s*tp)|(TROPICAL\\s*TP)", "med:format:"))

                .Then(new TagRegex("[qQ][0-9][DdHhMm]", "med:freq:"))
                .Then(new TagRegex("([Qq][0-9]-[0-9][hH])", "med:freq:"))

                .Then(new TagRegex("(per day)|(PER DAY)", "med:freq:"))
                .Then(new TagRegex("(every day)|(EVERY DAY)", "med:freq:"))
                .Then(new TagRegex("(at bedtime)|(AT BEDTIME)", "med:freq:"))

                .Then(new TagRegex("(bid)|(tid)|(qid)|(qd)|(qhs)|(q[0-9]-[0-9]h)|(q[0-9]h)", "med:freq:"))
                .Then(new TagRegex("(BID)|(TID)|(QID)|(QD)|(QHS)|(Q[0-9]-[0-9]H)|(Q[0-9]H)", "med:freq:"))

                .Then(new TagRegex("\\s((UNIT(S?))|(mg)|(MG)|([Tt][aA][Bb]([lL][eE][Tt]([sS])?)?))", "med:unit:"))

                .Then(new TagRegex("\\d+-\\d+-\\d+", "gen:id:"))
                .Then(new TagRegex("\\d+-\\d+", "med:num:"))

                .Then(new TagRegex(@"\d+-\d+", "med:num:"))
                .Then(new TagRegex(@"(\d+,)*\d+(\.\d+)?", "med:num:"))
                .Then(new TagRegex(@"\d+", "med:num:"));
                
        }
        public TextSpan ProcessString(string text)
        {
            var span = new TextSpan(text, text);
            var newSpan = tagString(span);
            return newSpan;
        }
        
        internal TextSpan tagString(TextSpan span)
        {             
            var context = new StrategyContext<TextSpan>(span, true);
            context = preBuilder.Execute(context);

            context = builder.Execute(context);

            var mns = new MedicationNameBuilder();
            context = mns.Execute(context);

            var s = new TagRegex(@"\s\d+\s", "med:num:");
            context = s.Execute(context);

            return context.Data;
        }
    }
}
