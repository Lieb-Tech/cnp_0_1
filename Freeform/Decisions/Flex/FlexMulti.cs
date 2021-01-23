using Common;
using Common.DecisionTree;

namespace Freeform.Decisions.Flex
{
    public class FlexMulti<T> : IDecisionTrunk<DecisionContext, TextSpanInfoes<T>>
    {
        public IStrategy<TextSpanInfoes<T>> GetDecision(DecisionContext data)
        {
            throw new System.NotImplementedException();
        }
    }
}
