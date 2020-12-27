
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class FrequencyStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*
            if (context.InProcess.Frequency != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            context.InProcess = context.InProcess with {  Frequency = tag.TagValue() };
            return context;
        }
    }
}
