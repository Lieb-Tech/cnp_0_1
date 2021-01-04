
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class SecondaryNameStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
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