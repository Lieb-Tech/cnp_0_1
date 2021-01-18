using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSpecification.Measurement
{
    public class ValueSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => text.Contains(":num", StringComparison.InvariantCultureIgnoreCase)
                || text.Contains(":descr", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
