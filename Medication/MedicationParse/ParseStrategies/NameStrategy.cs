
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NameStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  PrimaryName = tag.TagValue() };
            return context;
        }
    }
}
