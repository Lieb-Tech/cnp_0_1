using Common;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NameExtractionStrategy : IStrategy<MedicationInfo>
    {
        private readonly Specification<MedicationInfo> _specification;
        public NameExtractionStrategy(Specification<MedicationInfo> specification)
        {
            _specification = specification;
        }
        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            // don't have name, but have 2 other items tagged
            bool tryToExtract = _specification.IsSatisfiedBy(context.Data);
            if (!tryToExtract)
                return context;
            
            // get the parenthetical name
            var (newText , secondary) = processSecondary(context.Data.Tags[0]);
            context.Data.Tags[0] = newText;

            var (newTagArray, inferred) = getInferred(context.Data.Tags);

            // return object
            var data = context.Data with
            {
                InferredName = inferred?.TagValue(),
                SecondaryName = secondary,
                Tags = newTagArray
            };
            return new StrategyContext<MedicationInfo>(data, true);
        }

        internal (List<string>, string) getInferred(List<string> tags)
        {
            if (!tags.Any())
                return (tags, null);

            // create local copy of original
            var newTags = new List<string>(tags);

            // split into seperate words 
            var values = newTags[0].Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            // if the value is not a tag, then skip
            if (newTags[0].Contains("{"))
                return (tags.ToList(), null);

            // if blank, or only 1 item, then nothing to process
            if (!values.Any())
                return (tags.ToList(), null);

            // get the last word in string
            var inferred = "{med:infer:" + values.Last() + "}";

            // save inferred name to tag list
            tags.Insert(0, inferred);
           
            // remove the name from the original string with the name
            var replaceValue = newTags[1].Replace(values.Last(), "");

            // if there were other words in original string, then save to list
            if (!string.IsNullOrEmpty(replaceValue))
                newTags[1] = replaceValue;

            return (newTags, inferred);
        }

        internal (string, string) processSecondary(string tag)
        {
            if (!tag.Trim().EndsWith(")"))
                return (tag, null);

            var idx = tag.LastIndexOf("(");
            var idx2 = tag.LastIndexOf(")");
            var secondary = tag.Substring(idx + 1, idx2 - idx - 1).Trim();
            var newTag = tag.Substring(0, idx) + tag.Substring(idx2 + 1);

            return (newTag, secondary);
        }
    }
}