using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    class NegativeDescriptiveCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, 3, new ConditionInfoMap(2, 1,0,null));
            else
                return null;
        }

        public NegativeDescriptiveCondition()
        {
            var step3 = new IsTagOfType("condition", 2,
                "is a condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("descrip", 1,
                "descritive",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("negative",
                "negative",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}
