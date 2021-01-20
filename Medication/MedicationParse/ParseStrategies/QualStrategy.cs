using Common;
namespace Medication.MedicationParse.ParseStrategies
{
    public class QualStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  Qualifier = tag.TagValue() };
            return context;
        }
    }
}
