using Common;
using System.Linq;

namespace Medication.MedicationParse.InferredNameStrategies
{
    class PriorToSecondaryStrategy : IStrategy<MedicationInfo>
    {
        /// <summary>
        /// Given: Primary name is not tagged
        /// and: Secondary is tagged
        /// Then: Get 1st untagged value before secondary
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            // get the first 
            var secondaryIdx = context.Data
                .Tags
                .Select((x, i) => new { tag = x, index = i })
                .Where(x => x.tag.Trim().ToLower().StartsWith("{med:secondary:"))
                .Select(x => x.index)
                .FirstOrDefault();

            // if 0 then it's either not found or the 1st element
            // either logic section assumes that name is before secondary
            if (secondaryIdx < 1)
                return context;

            // get the first 
            var untagged = context.Data
                .Tags
                .Where((x, i) => i < secondaryIdx && !x.Contains("{"))
                .LastOrDefault();

            if (untagged != null)
            {
                var data = context.Data with { InferredName = untagged };
                return new StrategyContext<MedicationInfo>(data, false);
            }

            return context;
        }        
    }
}
