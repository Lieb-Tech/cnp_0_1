using Common;
using System;
using System.Linq;

namespace Medication.MedicationParse.InferredNameStrategies
{
    public class UntaggedFirstTagStrategy : IStrategy<MedicationInfo>
    {
        /// <summary>
        /// Given: Primary name is last word in 1st Tag 
        /// and: 1st Tag is untagged
        /// Then: Extract primary from the string
        /// and: if any remainder, insert as new Tag entry
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            if (context.Data.Tags[0].Contains("{"))
                return context;

            // split into seperate words 
            var values = context.Data.Tags[0].Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            // if blank, or only 1 item, then nothing to process
            if (!values.Any())
                return context;

            // get the last word in string
            var inferred = "{med:infer:" + values.Last() + "}";
            var orig = context.Data.OriginalText.Replace(values.Last(), inferred);

            // save inferred name to tag list
            context.Data.Tags.Insert(1, inferred);

            // remove the name from the original string with the name
            var replaceValue = context.Data.Tags[1].Replace(values.Last(), "");

            // if there were other words in original string, then save to list
            if (!string.IsNullOrWhiteSpace(replaceValue))
                context.Data.Tags[0] = replaceValue;
            else
                // or no other text, then remove blank element from array
                context.Data.Tags.RemoveAt(1);

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
