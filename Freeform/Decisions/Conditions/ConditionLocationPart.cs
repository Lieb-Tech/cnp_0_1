using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    
    public class ConditionLocationPart : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, 3, new ConditionInfoMap(0,1,null,2));
            else
                return null;
        }

        /// <summary>
        // {gen:condition:CCC} {gen:part:ccc}
        // Carcinoma of the colon
        /// </summary>

        public ConditionLocationPart()
        {
            var step3 = new IsTagOfType("part", 2,
                "is a body part",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("loc", 1,
                "is location",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("condition",
                "condition ",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(2,
                "number of tags = 2",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}