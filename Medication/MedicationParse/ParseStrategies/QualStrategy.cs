
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class QualStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*
            if (context.InProcess.Qualifier != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            context.InProcess = context.InProcess with {  Qualifier = tag.TagValue() };
            return context;
        }
    }
}
