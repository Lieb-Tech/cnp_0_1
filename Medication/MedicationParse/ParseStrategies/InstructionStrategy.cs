
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class InstructionStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            var newValue = context.InProcess.Instruction += " " + tag;
            context.InProcess = context.InProcess with { Instruction = newValue.TagValue() };
            return context;

        }
    }
}
