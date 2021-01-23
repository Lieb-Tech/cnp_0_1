using Common.DecisionTree;
using System.Collections.Generic;

namespace Freeform.Decisions.Flex
{
    public class FlexTreeBuilder
    {
        private readonly FlexQueryBuilder queryBuilder = new();

        public IDecisionTrunk<DecisionContext, TextSpanInfoes<T>> GetTree<T>(List<IFlexTreeBranch> branchConfigs)
        {
            var branches = GetQueries(branchConfigs);

            foreach (var branchConfig in branchConfigs)
            {
                updatePositive(branchConfig, branches);
                updateNegative(branchConfig, branches);
            }

            return null;
        }

        private void updatePositive(IFlexTreeBranch branchConfig, Dictionary<string, DecisionQuery<ITaggedData>> branches)
        {
            var branch = branches[branchConfig.positive];
            var curBranch = branches[branchConfig.label];
            curBranch.Positive = branch;
        }

        private void updateNegative(IFlexTreeBranch branchConfig, Dictionary<string, DecisionQuery<ITaggedData>> branches)
        {
            var branch = branches[branchConfig.negative];
            var curBranch = branches[branchConfig.label];
            curBranch.Negative = branch;
        }

        public Dictionary<string, DecisionQuery<ITaggedData>> GetQueries(List<IFlexTreeBranch> branchConfigs)
        {
            Dictionary<string, DecisionQuery<ITaggedData>> branches = new Dictionary<string, DecisionQuery<ITaggedData>>();
            foreach (var config in branchConfigs)
            {
                branches.Add(config.label, queryBuilder.GetQuery(config));
            }
            return branches;
            
        }
    }
}
