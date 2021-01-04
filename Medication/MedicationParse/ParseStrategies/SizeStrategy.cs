
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class SizeStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Size = tag.TagValue() };
            return context;
        }
    }
}
