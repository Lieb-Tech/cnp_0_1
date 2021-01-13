
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class FormatStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with { Format = tag.TagValue() };
            return context;
        }
    }
}
