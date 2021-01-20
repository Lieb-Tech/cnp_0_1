using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NameStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  PrimaryName = tag.TagValue() };
            return context;
        }
    }
}
