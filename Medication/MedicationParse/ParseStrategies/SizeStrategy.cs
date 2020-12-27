
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class SizeStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*
            if (context.InProcess.Size != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            context.InProcess = context.InProcess with { Size = tag.TagValue() };
            return context;
        }
    }
}
