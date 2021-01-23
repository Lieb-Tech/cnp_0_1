using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    public class TimePartCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy3(data.Index);
            else
                return null;
        }
        
        /// <summary>
        // {gen:time:ccc} {gen:part:ccc} {gen:condition:ccc}
        // {gen:time:history of} {gen:part:colon} {gen:condition:cancer}
        /// </summary>

        public TimePartCondition()
        {
            var step3 = new IsTagOfType("condition", 2,
                "is condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("part", 1,
                "is a body part",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("time",
                "history ",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}