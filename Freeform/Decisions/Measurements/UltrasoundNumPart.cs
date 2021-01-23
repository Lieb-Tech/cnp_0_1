using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class UltrasoundNumPart : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
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
        /// {gen:measurement:Ultrasound} {gen:num:NNN} {gen:part:CCCC}
        /// </summary>

        public UltrasoundNumPart()
        {
            var step3 = new IsTagOfType("part", 2,
                "is a part",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("num", 1,
                "is a number",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

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