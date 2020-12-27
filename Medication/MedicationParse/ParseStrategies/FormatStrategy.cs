
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class FormatStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*if (context.InProcess.Format != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            
            context.InProcess = context.InProcess with { Format = tag.TagValue() };
            return context;
        }
    }
}
