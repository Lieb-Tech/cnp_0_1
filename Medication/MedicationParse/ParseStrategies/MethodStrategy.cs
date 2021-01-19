
using Common;

namespace Common.MedicationParse.ParseStrategies
{
    public class MethodStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            if (context.InProcess.Method != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }

            context.InProcess = context.InProcess with { Method = tag.TagValue() };
            return context;
        }
    }
}
