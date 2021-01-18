using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSpecification.Measurement
{
    public class NonTagSpecification : ISpecification<string>
    {
        public bool IsSatisfiedBy(string entity)
        {
            return !entity.Contains("{");
        }

        public Expression<Func<string, bool>> ToExpression()
        {
            return ctx => false;
            // throw new NotImplementedException();
        }
    }
}
