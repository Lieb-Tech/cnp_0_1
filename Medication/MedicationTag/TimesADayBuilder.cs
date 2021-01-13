using Common.Processing;
using System.Collections.Generic;

namespace Common.MedicationTag
{
    public class TimesADayBuilder : IStrategy<TextSpan>
    {
        private readonly DictionaryReplaceStrategy dictionaryReplaceStrategy;

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

            dictionaryReplaceStrategy = new DictionaryReplaceStrategy(listing, "med:freq:");
        }
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            return dictionaryReplaceStrategy.Execute(context);
        }
    }
}
