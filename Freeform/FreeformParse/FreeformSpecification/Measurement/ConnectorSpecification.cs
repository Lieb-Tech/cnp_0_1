using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSpecification.Measurement
{
    public class ConnectorSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => text.Contains("Connector", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
