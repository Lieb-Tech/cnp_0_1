
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class UnitStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            /*
            if (context.InProcess.Unit != null)
            {
                context.Completed.Add(context.InProcess);
                context.InProcess = new MedicationInfo();
            }
            */
            context.InProcess = context.InProcess with { Unit = tag.TagValue() };
            return context;
        }
    }
}
