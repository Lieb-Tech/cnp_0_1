using Common;
using System;
using System.Linq;

namespace Common.MedicationParse.InferredNameStrategies
{
    class TextPriorToSecondaryStrategy : IStrategy<MedicationInfo>
    {
        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            var pts = new PriorToSecondary();
            var untagged = pts.GetTagPriorToSecondary(context.Data.Tags);

            if (untagged == null)
                return context;

            // if single word, then this logic doesn't apply
            if (untagged.Tag.Split(" ", StringSplitOptions.RemoveEmptyEntries).Count() == 1)
                return context;

            // split into seperate words 
            var values = untagged.Tag.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            // if blank, or only 1 item, then nothing to process
            if (!values.Any())
                return context;

            // if potential word doesn;t start with capital letter
            if (values.Last()[0] != values.Last().ToUpper()[0])
                return context;

            // get the last word in string
            var inferred = "{med:infer:" + values.Last() + "}";
            var orig = context.Data.OriginalText.Replace(values.Last(), inferred);

            // save inferred name to tag list
            context.Data.Tags.Insert(untagged.Index + 1, inferred);

            // remove the name from the original string with the name
            var replaceValue = context.Data.Tags[untagged.Index].Replace(values.Last(), "");

            // if there were other words in original string, then save to list
            if (!string.IsNullOrWhiteSpace(replaceValue))
                context.Data.Tags[untagged.Index ] = replaceValue;
            else
                // or no other text, then remove blank element from array
                context.Data.Tags.RemoveAt(untagged.Index);

            // return updated data
            var data = context.Data with
            {
                InferredName = inferred?.TagValue(),
                OriginalText = orig,
            };
            return new StrategyContext<MedicationInfo>(data, false);
        }
    }
}
