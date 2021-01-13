
namespace Common.MedicationParse.ParseStrategies
{
    public class NonTaggedStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            var newValue = context.InProcess.OriginalText += " " + tag.Replace(",","").Replace(" and ", "");
            context.InProcess = context.InProcess with { OriginalText = newValue.Trim() };
            return context;
        }
    }
}
