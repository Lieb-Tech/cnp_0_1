﻿using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.FreeformParse.FreeformSetSpecification.Measurement
{
    public class ConnectorSetSpecification : Specification<MeasurementInfo>
    {
        public override Expression<Func<MeasurementInfo, bool>> ToExpression()
        {
            // connector is appened, so it's not used to determine
            return ctx => !string.IsNullOrWhiteSpace(ctx.Measurement);
        }
    }
}