
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class SizeStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Size = tag.TagValue() };
            return context;
        }
    }
}
