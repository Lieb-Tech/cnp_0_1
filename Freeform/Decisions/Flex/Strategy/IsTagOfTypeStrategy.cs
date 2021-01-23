using Common;
using Common.DecisionTree.DecisionQueries;

namespace Freeform.Decisions.Flex.Strategy
{
    public class IsTagOfTypeStrategy : IStrategy<TreeBranchAndQuery>
    {
        StrategyContext<TreeBranchAndQuery> IStrategy<TreeBranchAndQuery>.Execute(StrategyContext<TreeBranchAndQuery> context)
        {
            var firstTag = new IsTagOfType(context.Data.treeBranch.argument,
                 context.Data.treeBranch.index.Value,
                 context.Data.treeBranch.label,
                 null, null);

            var data = context.Data with { query = firstTag };

            return context with { Data = data, Continue = false };
        }
    }
}