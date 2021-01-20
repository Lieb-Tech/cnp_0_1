using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class InstructionStrategy : IInprocessAndCompletedStrategy<MedicationInfo>
    {
        public InprocessAndCompleted<MedicationInfo> Execute(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            var newValue = context.InProcess.Instruction += " " + tag;
            context.InProcess = context.InProcess with { Instruction = newValue.TagValue() };
            return context;

        }
    }
}
