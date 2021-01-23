using Common;
using Common.DecisionTree;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class OnlyMeasure : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
    {
        private readonly DecisionQuery<DecisionContext> trunk;

        public IStrategy<TextSpanInfoes<MeasurementInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy1(data.Index);
            else
                return null;
        }

        /// <summary>
        /// {gen:measurement:CCC}
        /// {gen:measurement:alert}
        /// </summary>

        public OnlyMeasure()
        {
            trunk = new DecisionQuery<DecisionContext>()
            {
                Label = "OnlyTag",
                Test = (d) =>
                {                   
                    var test = d.Tags.Count == 1 && d.Tags[0].Contains("measure");
                    d.Matched = test;
                    return test;
                },
                Positive = DecisionResults<DecisionContext>.GetPositive(),
                Negative = DecisionResults<DecisionContext>.GetNegative(),
            };            
        }
    }
}
