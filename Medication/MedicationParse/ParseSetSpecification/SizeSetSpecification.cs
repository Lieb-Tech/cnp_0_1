
using Common;
using System;
using System.Linq.Expressions;

namespace Medication.MedicationParse.ParseStrat2
{
    public class SizeSetSpecification : Specification<MedicationInfo>
    {
        public override Expression<Func<MedicationInfo, bool>> ToExpression()
        {
            return ctx => string.IsNullOrEmpty(ctx.Size); 
        }
    }
}
