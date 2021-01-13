using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSetSpecification.Measurement
{
    public class MeasurementSetSpecification : Specification<MeasurementInfo>
    {
        public override Expression<Func<MeasurementInfo, bool>> ToExpression()
        {
            return ctx => !string.IsNullOrEmpty(ctx.Measurement);
        }
    }
}