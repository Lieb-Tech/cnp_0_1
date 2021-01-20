using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class FormatStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with { Format = tag.TagValue() };
            return context;
        }
    }
}
