using Common;
using Medication.MedicationParse.InferredNameStrategies;
using System.Collections.Generic;
using System.Linq;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NameExtractionStrategy : IStrategy<MedicationInfo>
    {
        private readonly Specification<MedicationInfo> _specification;
        private readonly ChainedStrategy<MedicationInfo> _inferred;
        public NameExtractionStrategy(Specification<MedicationInfo> specification)
        {
            _specification = specification;

            _inferred = new PriorToSecondaryStrategy()
                .Then(new UntaggedFirstTagStrategy());
        }
        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            bool tryToExtract = _specification.IsSatisfiedBy(context.Data);
            if (!tryToExtract)
                return context;

            MedicationInfo data = context.Data;
            if (string.IsNullOrWhiteSpace(data.SecondaryName))
                data = processSecondaryNextTag(context.Data);

            // now get the last word in untagged text 
            data = getInferred(data);

            return new StrategyContext<MedicationInfo>(data, true);
        }

        /// <summary>
        /// if primary name not set, try infer name from text
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal MedicationInfo getInferred(MedicationInfo data)
        {            
            if (!data.Tags.Any() || !string.IsNullOrEmpty(data.PrimaryName))
                return data;

            // try different strategies to extract values
            var context = new StrategyContext<MedicationInfo>(data, true);
            context = _inferred.Execute(context);

            return context.Data;
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