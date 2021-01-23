using Common;

namespace Freeform.Decisions.Flex
{
    public class FlexMap
    {
        public IStrategy<TreeBranchAndQuery> Strategy { get; set; }
        public Specification<IFlexTreeBranch> Specification { get; set; }
    }
}
