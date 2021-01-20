
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NonTaggedStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            var newValue = context.InProcess.OriginalText += " " + tag.Replace(",","").Replace(" and ", "");
            context.InProcess = context.InProcess with { OriginalText = newValue.Trim() };
            return context;
        }
    }
}
