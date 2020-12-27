using Common;

namespace Medication.MedicationTag
{
    public class PreperationStrategy : IStrategy<TextSpan>
    {
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {            
            var data = context.Data with { UpdatedText = context.Data.OriginalText };
            return new StrategyContext<TextSpan>(data, true);
        }
    }
}
