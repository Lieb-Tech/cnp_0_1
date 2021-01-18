using Common;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSpecification.Measurement
{
    public class NonMeasurementSpecification : Specification<string>
    {
        private readonly List<string> tags = new List<string>()
        {
            ":measure",
            ":num",
            ":desc",
            ":connect",
            ":location",
            ":part",
        };

        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => 
                text.Contains("{") && 
                !tags.Any(t => text.Contains(t, StringComparison.InvariantCultureIgnoreCase));
            }
    }
}
