using Common;
using System;
using System.Linq.Expressions;

namespace Medication.MedicationParse.ParseStrat2
{
    public class InstructionSetSpecification : Specification<MedicationInfo>
    {
        public override Expression<Func<MedicationInfo, bool>> ToExpression()
        {
            return ctx => string.IsNullOrEmpty(ctx.Instruction); 
        }
    }
}
