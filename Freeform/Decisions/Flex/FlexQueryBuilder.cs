
using Common;
using Common.DecisionTree;
using Freeform.Decisions.Flex.Maps;
using Freeform.Decisions.Flex.Strategy;
using System.Collections.Generic;

namespace Freeform.Decisions.Flex
{
    public class FlexQueryBuilder
    {
        private readonly List<FlexMap> maps = new();

        public FlexQueryBuilder()
        {
            maps.Add(new FlexMap()
            {
                Specification = new FirstTagOfTypeSpecification(),
                Strategy = new FirstTagOfTypeStrategy(),
            });
        }

        public void Add(FlexMap flexMap) => maps.Add(flexMap);

        public DecisionQuery<ITaggedData> GetQuery(IFlexTreeBranch branch)
        {
            foreach (var map in maps)
            {                
                if (map.Specification.IsSatisfiedBy(branch))
                {
                    var ctx = new StrategyContext<TreeBranchAndQuery>(new TreeBranchAndQuery(branch, null), true);
                    ctx = map.Strategy.Execute(ctx);
                    return ctx.Data.query;
                }
            }

            return null;
        }
    }
}
