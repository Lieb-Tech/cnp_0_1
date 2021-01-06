using Common;
using Common.Processing;

namespace Medication.MedicationTag
{
    public class MedicationTagger
    {
        private readonly IStrategy<TextSpan> genericBuilder;
        private readonly IStrategy<TextSpan> medBuilder;

        public MedicationTagger()
        {
            genericBuilder = new TextToNumberStrategy()                
                .Then(new TagRegex(@"(\d{1,2}/\d{1,2}/\d{2,4})( \d{1,2}:\d{2}(:\d{0,2})? [AaPp][Mm])", "gen:datetime:"))
                .Then(new TagRegex(@"\d{1,2}:\d{1,2}:\d{2,4} [AaPp][Mm]", "gen:time:"))
                .Then(new TagRegex(@"(\d{1,2}/\d{1,2}/\d{2,4})", "gen:date:"));

            medBuilder = new LineNumBuilder()                
                .Then(new DotStrategy())

               .Then(new TimesADayBuilder())
               .Then(new InstructionBuilder())

               .Then(new TextNumberSplitBuilder())

               .Then(new TagRegex("prn [a-z]* pain", "med:qual:"))
               .Then(new TagRegex("(prn pain)|(prn)", "med:qual:"))

               .Then(new TagRegex("\\s((SC)|(topical tp)|(TP))\\s", "med:format:"))
               .Then(new TagRegex("\\s((po)|(sublingual)|(iv)|(oral(ly)?))\\s", "med:format:"))
               .Then(new TagRegex("INHALER", "med:format:"))

               .Then(new TagRegex("(tropical\\s*tp)|(TROPICAL\\s*TP)", "med:format:"))

               .Then(new TagRegex("[qQ][0-9]\\s?[DdHhMm]", "med:freq:"))
               .Then(new TagRegex("([Qq][0-9]-[0-9]\\s?[hH])", "med:freq:"))
               .Then(new TagRegex("((every)\\s?[0-9]-[0-9]\\s?((hr(s)?)|(h))?)", "med:freq:"))

               .Then(new TagRegex("(per day)|(PER DAY)", "med:freq:"))
               .Then(new TagRegex("(every day)|(EVERYDAY)", "med:freq:"))
               .Then(new TagRegex("(at bedtime)|(breakfast)", "med:freq:"))

               .Then(new TagRegex("\\s((bid)|(tid)|(QAM)|(QPM)|(qid)|(qd)|(qhs)|(q[0-9]-[0-9]\\s?h)|(q\\s?[0-9]\\s?h))", "med:freq:"))

               .Then(new TagRegex(@"\s(puff(s?))", "med:unit:"))
               .Then(new TagRegex(@"\s(unit(s?))", "med:unit:"))
               .Then(new TagRegex(@"\s(mg(s?))", "med:unit:"))
               .Then(new TagRegex(@"\s(tab(s?))", "med:unit:"))
               .Then(new TagRegex(@"\s(cap(s?)(ule(s?)))", "med:unit:"))

               .Then(new TagRegex("\\d+-\\d+-\\d+", "gen:id:"))
               .Then(new TagRegex("\\d+-\\d+", "med:num:"))

               .Then(new TagRegex("take with food", "med:instr:"))

               .Then(new TagRegex("(\\([\\sa-zA-Z0-9]+\\))", "med:secondary:"))

               .Then(new TagRegex(@"[0-9]+\s?(\\|/|-|to)\s?[0-9]+", "med:num:"))
               .Then(new TagRegex(@"([0-9]+,)*[0-9]+(\.[0-9]+)?", "med:num:"))
               .Then(new TagRegex(@"[0-9]+", "med:num:"));

                // .Then(new TagRegex("for \\d day(s)?", "med:other:"));
                // .Then(new TagRegex("starting \\w?", "med:other:"));

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
            context = genericBuilder.Execute(context);
            context = medBuilder.Execute(context);

            var mns = new MedicationNameBuilder();
            context = mns.Execute(context);

            var s = new TagRegex(@"\s\d+\s", "med:num:");
            context = s.Execute(context);

            return context.Data;
        }
    }
}
