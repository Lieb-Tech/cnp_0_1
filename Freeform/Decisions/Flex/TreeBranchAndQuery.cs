
using Common.DecisionTree;

namespace Freeform.Decisions.Flex
{
    public record TreeBranchAndQuery(IFlexTreeBranch treeBranch, DecisionQuery<ITaggedData> query) { }
}
