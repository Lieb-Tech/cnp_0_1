using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class FrequencyStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            if (tag.Contains("Q"))
                context.InProcess = context.InProcess with {  Frequency = tag.TagValue().Replace(" ","") };
            else
                context.InProcess = context.InProcess with { Frequency = tag.TagValue() };
            return context;
        }
    }
}
