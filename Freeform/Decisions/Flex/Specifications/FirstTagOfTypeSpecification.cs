using Common;
using System;
using System.Linq.Expressions;

namespace Freeform.Decisions.Flex.Maps
{
    public class FirstTagOfTypeSpecification : Specification<IFlexTreeBranch>
    {
        public override Expression<Func<IFlexTreeBranch, bool>> ToExpression()
        {
            return (branch) => branch.label.Equals("FirstTagOfType", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
