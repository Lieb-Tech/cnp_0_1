using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class TimeMeasureNum : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<MeasurementInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy5(data.Index);
            else
                return null;
        }

        /// <summary>
        /// {gen:measurement:CCC} {gen:num:NNN}
        /// {gen:measurement:calcium} {gen:num:2}
        /// </summary>
        public TimeMeasureNum()
        {
            var step3 = new IsTagOfType("num", 2,
                "is a number",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("measure", 1,
                "is a measurement",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("time",
                "is a measurement",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}