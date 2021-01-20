using Common;
using System;
using System.Linq;

namespace Medication.MedicationParse.InferredNameStrategies
{
    public class SinglePriorToSecondaryStrategy : IStrategy<MedicationInfo>
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
            var pts = new PriorToSecondary();
            var untagged = pts.GetTagPriorToSecondary(context.Data.Tags);            
            
            if (untagged == null)
                return context;

            // if multiple words, then this logic doesn't apply
            if (untagged.Tag.Split(" ", StringSplitOptions.RemoveEmptyEntries).Count() > 1)
                return context;

            // if potential word doesn;t start with capital letter
            if (untagged.Tag[0] != untagged.Tag.ToUpper()[0])
                return context;

            // if single word, then process it
            var data = context.Data with { InferredName = untagged.Tag };
            return new StrategyContext<MedicationInfo>(data, false);            
        }        
    }
}
