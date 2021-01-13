
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class QualStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  Qualifier = tag.TagValue() };
            return context;
        }
    }
}
