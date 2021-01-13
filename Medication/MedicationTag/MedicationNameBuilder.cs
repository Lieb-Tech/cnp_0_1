using Common;
using Common.Processing;
using System.Collections.Generic;

namespace Common.MedicationTag
{
    public class MedicationNameBuilder : IStrategy<TextSpan>
    {        
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var names = new List<string>()
            {
                "Aspirin",
                "Atenolol",
                "Calcium acetate",
                "Celexa",
                "Lipitor"
            };

            var innerStrategy = new StringListStrategy(names, "med:name:");
            return innerStrategy.Execute(context);
        }        
    }
}