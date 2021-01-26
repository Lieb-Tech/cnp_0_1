using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    class NegativeConditionConditionCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, 4, new ConditionInfoMap(1, 0, 2, 3));
            else
                return null;
        }

        public NegativeConditionConditionCondition()
        {
            var step4 = new IsTagOfType("condition", 3,
                "is a condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step3 = new IsTagOfType("condition", 2,
                "is a condition",
                step4,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("condition", 1,
                "is a condition",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("negative",
                "negative",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(4,
                "number of tags = 4",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}
