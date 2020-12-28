using Common;
using System.Collections.Generic;
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
            var data = processSecondaryInLine(context.Data);

            if (string.IsNullOrWhiteSpace(context.Data.SecondaryName))
                data = processSecondaryNextTag(data);

            // now get the last word in untagged text 
            data = getInferred(data);

            return new StrategyContext<MedicationInfo>(data, true);
        }

        internal MedicationInfo getInferred(MedicationInfo data)
        {
            if (!data.Tags.Any())
                return data;
            
            // if the value is not a tag, then skip
            if (data.Tags[0].Contains("{"))
                return data;

            // split into seperate words 
            var values = data.Tags[0].Split(" ", System.StringSplitOptions.RemoveEmptyEntries);

            // if blank, or only 1 item, then nothing to process
            if (!values.Any())
                return data;

            // get the last word in string
            var inferred = "{med:infer:" + values.Last() + "}";

            // save inferred name to tag list
            data.Tags.Insert(0, inferred);
           
            // remove the name from the original string with the name
            var replaceValue = data.Tags[1].Replace(values.Last(), "");

            // if there were other words in original string, then save to list
            if (!string.IsNullOrWhiteSpace(replaceValue))
                data.Tags[1] = replaceValue;
            else
                // or no other text, then remove blank element from array
                data.Tags.RemoveAt(1);

            // return updated data
            return data with
            {
                InferredName = inferred?.TagValue()
            };
        }

        /// <summary>
        /// Extract secondary names in lines like:  primary (secondary)   
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        internal MedicationInfo processSecondaryInLine(MedicationInfo data)
        {
            if (!data.Tags[0].Trim().EndsWith(")"))
                return data;
           
            var idx = data.Tags[0].LastIndexOf("(");
            var idx2 = data.Tags[0].LastIndexOf(")");
            if (idx == -1)
                return data;

            var tags = new List<string>(data.Tags);
            var secondary = data.Tags[0].Substring(idx + 1, idx2 - idx - 1).Trim();
            tags[0] = data.Tags[0].Substring(0, idx) + data.Tags[0].Substring(idx2 + 1);

            return data with { SecondaryName = secondary, Tags = tags };
        }

        /// <summary>
        /// If first tag is med:name, but second entry is ( NNNNN )
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        internal MedicationInfo processSecondaryNextTag(MedicationInfo data)
        {
            // if not enough tags to process
            if (data.Tags.Count < 2)
                return data;

            // if first isn't a name tag, then this branch doesn't apply
            if (!data.Tags[0].Contains("med:name:"))
                return data;

            // if 2nd element doesn't have parans
            if (!data.Tags[1].Contains("(") || !data.Tags[1].Contains(")"))
                return data;

            // extract value from parans,and update data
            var tags = new List<string>(data.Tags);
            var idx = data.Tags[1].LastIndexOf("(");
            var idx2 = data.Tags[1].LastIndexOf(")");
            
            var secondary = data.Tags[1].Substring(idx + 1, idx2 - idx - 1).Trim();
            tags[1] = data.Tags[1].Substring(0, idx) + data.Tags[1].Substring(idx2 + 1);
            
            return data with { SecondaryName = secondary, Tags = tags };
        }
    }
}