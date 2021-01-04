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

            // configure logics to extract name
            _inferred = new SinglePriorToSecondaryStrategy()
                .Then(new TextPriorToSecondaryStrategy())
                .Then(new UntaggedFirstTagStrategy());
        }

        public StrategyContext<MedicationInfo> Execute(StrategyContext<MedicationInfo> context)
        {
            bool tryToExtract = _specification.IsSatisfiedBy(context.Data);
            if (!tryToExtract)
                return context;

            MedicationInfo data = context.Data;
            // try to extract names
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
    }
}