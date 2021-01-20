using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class UnitStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Unit = tag.TagValue() };
            return context;
        }
    }
}
