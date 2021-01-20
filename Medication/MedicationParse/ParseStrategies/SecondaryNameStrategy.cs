using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class SecondaryNameStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
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