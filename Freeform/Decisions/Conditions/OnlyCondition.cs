using Common;
using Common.DecisionTree;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    public class OnlyCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<DecisionContext> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy1(data.Index);
            else
                return null;
        }

        /// <summary>
        /// {gen:condition:CCC}
        /// {gen:condition:adenopathy}
        /// </summary>

        public OnlyCondition()
        {
            trunk = new DecisionQuery<DecisionContext>()
            {
                Label = "OnlyTag",
                Test = (d) =>
                {
                    var test = d.Tags.Count == 1 && d.Tags[0].Contains("condition");
                    d.Matched = test;
                    return test;
                },
                Positive = DecisionResults<DecisionContext>.GetPositive(),
                Negative = DecisionResults<DecisionContext>.GetNegative(),
            };
        }
    }
}
