using Common;
using Common.Processing;
using System.Collections.Generic;

namespace Medication.MedicationTag
{
    // tag "x times a day" parts
    public class TimesADayBuilder : IStrategy<TextSpan>
    {
        private readonly DictionaryReplaceAndTagStrategy dictionaryReplaceStrategy;

        public TimesADayBuilder()
        {
            var listing = new Dictionary<string, string>()
            {
                {  "2 times a day", "bid" },
                {  "2 times per day", "bid" },
                {"2x a day", "bid" },

                { "3 times a day", "tid" },
                { "3 times per day", "tid" },
                {"3x a day", "tid" },

                {"4 times a day", "qid" },
                {"4 times per day", "qid" },
                { "4x a day", "qid" }                
            };

            dictionaryReplaceStrategy = new DictionaryReplaceAndTagStrategy(listing, "med:freq:");
        }
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            return dictionaryReplaceStrategy.Execute(context);
        }
    }
}
