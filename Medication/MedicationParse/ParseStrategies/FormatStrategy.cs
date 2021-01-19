
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class FormatStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with { Format = tag.TagValue() };
            return context;
        }
    }
}
