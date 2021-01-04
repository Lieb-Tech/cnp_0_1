
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class FrequencyStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with {  Frequency = tag.TagValue() };
            return context;
        }
    }
}
