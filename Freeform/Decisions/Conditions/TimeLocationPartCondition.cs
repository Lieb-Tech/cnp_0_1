using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    public class TimeLocationPartCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ConditionInfoMap(3,2,1,0));
            else
                return null;
        }

        /// <summary>
        // {gen:time:ccc} {get:location:cccc} {gen:part:ccc} {gen:condition:ccc}
        // {gen:time:history of} {gen:loc:bilateral} {gen:part:shoulder} {gen:condition:pain}
        /// </summary>

        public TimeLocationPartCondition()
        {
            var step4 = new IsTagOfType("condition", 3,
                "is condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step3 = new IsTagOfType("part", 2,
                "is a body part",
                step4,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("loc", 1,
                "is a location",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("time",
                "history",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}