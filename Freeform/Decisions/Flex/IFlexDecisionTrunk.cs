using Common.DecisionTree;

namespace Freeform.Decisions.Flex
{
    public interface IFlexDecisionTrunk<TInput, TOutput>
    {
        void SetTrunk(IDecisionTrunk<TInput, TOutput> trunk);
    }
}
