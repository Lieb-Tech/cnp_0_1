using Common;
using System;
using System.Linq.Expressions;

namespace Medication.MedicationParse.ParseSpecifications
{
    public class NonTaggedSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => !text.Contains("{");
        }
    }
}
