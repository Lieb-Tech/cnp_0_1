
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class QualStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  Qualifier = tag.TagValue() };
            return context;
        }
    }
}
