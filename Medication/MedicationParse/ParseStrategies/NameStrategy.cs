
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class NameStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*
            if (context.InProcess.PrimaryName != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            context.InProcess = context.InProcess with {  PrimaryName = tag.TagValue() };
            return context;
        }
    }
}
