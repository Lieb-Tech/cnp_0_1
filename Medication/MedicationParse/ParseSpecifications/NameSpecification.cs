﻿using Common;
using System;
using System.Linq.Expressions;

namespace Common.MedicationParse.ParseSpecifications
{
    public class NameSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => text.Contains("med:name");
        }
    }
}
