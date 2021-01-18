using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSetSpecification.Measurement
{
    public class OtherTagSetSpecification : Specification<MeasurementInfo>
    {
        public override Expression<Func<MeasurementInfo, bool>> ToExpression()
        {
            // connector is appened, so it's not used to determine
            return ctx => false;
        }
    }
}