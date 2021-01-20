using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class SizeStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Size = tag.TagValue() };
            return context;
        }
    }
}
