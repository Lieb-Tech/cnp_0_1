
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class UnitStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Unit = tag.TagValue() };
            return context;
        }
    }
}
