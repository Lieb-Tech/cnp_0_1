using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    public class ChronicCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy2(data.Index);
            else
                return null;
        }

        /// <summary>
        /// <summary>
        /// {gen:descriptive:CC} {gen:condition:CCC}
        /// {gen:time:Chronic} {gen:condition:adenopathy}
        /// </summary>

        /// </summary>
        public ChronicCondition()
        {
            var step2 = new IsTagOfType("condition", 1,
                "is a condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagHasValue("chronic",
                "is a chronic",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(2,
                "number of tags = 2",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}