using Common;
using System;
using System.Linq.Expressions;

namespace cnp_0_1.TextParser
{
    public class PlainTextSpecification : Specification<string>
    {
        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => !text.Trim().StartsWith("<");
        }
    }
}
