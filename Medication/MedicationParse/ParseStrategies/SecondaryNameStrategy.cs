
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class SecondaryNameStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {            
            context.InProcess = context.InProcess with {  
                SecondaryName = tag.TagValue()
                                    .Replace("(", "")
                                    .Replace(")", "")
                                    .Trim() 
            };
            return context;
        }
    }
}