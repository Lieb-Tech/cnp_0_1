using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class Ultra_Condition : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<MeasurementInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy3(data.Index);
            else
                return null;
        }

        /// <summary>
        /// specific use case
        /// {gen:measurement:Ultrasound} {gen:negative:NNN} {gen:condition:CCCC}
        /// {gen:measurement:Ultrasound} {gen:negative:no} {gen:condition:stones}
        /// and
        /// {gen:measurement:Ultrasound} {gen:positive:NNN} {gen:condition:CCCC}
        /// {gen:measurement:Ultrasound} {gen:positive:within limits} {gen:condition:something}
        /// </summary>

        public Ultra_Condition()
        {
            var step_condition = new IsTagOfType("condition", 2,
                "is a part",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step_positive = new IsTagOfType("positive", 1,
                "is positive",
                step_condition,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("neg", 1,
                "is negative",
                step_condition,
                step_positive);

            var step1 = new FirstTagHasValue("ultrasound", 
                "ultrasound",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}