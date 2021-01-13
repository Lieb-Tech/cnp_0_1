﻿
using Common;
using System;
using System.Linq.Expressions;

namespace Common.MedicationParse.ParseStrat2
{
    public class QualSetSpecification : Specification<MedicationInfo>
    {
        public override Expression<Func<MedicationInfo, bool>> ToExpression()
        {
            return ctx => string.IsNullOrEmpty(ctx.Qualifier); 
        }
    }
}
