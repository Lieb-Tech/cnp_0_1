using Common;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace cnp_0_1.TextParser
{
    public class SectionSpecification : Specification<string>
    {
        private const string pattern = @"([A-Z\s]+)\s?:";
        private readonly Regex regex;

        public SectionSpecification()
        {
            regex = new Regex(pattern);
        }

        public override Expression<Func<string, bool>> ToExpression()
        {
            return text => regex.IsMatch(text);
        }
    }
}
