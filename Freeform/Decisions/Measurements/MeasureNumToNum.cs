using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class MeasureNumToNum : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<MeasurementInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new Strategy4(data.Index);
            else
                return null;
        }

        /// <summary>
        /// {gen:measurement:CCC} {gen:num:NNN} {gen:connector:to} {gen:num:NNN}
        /// {gen:measurement:calcium} {gen:num:2} {gen:connector:to} {gen:num:5}
        /// </summary>

        public MeasureNumToNum()
        {
            var step4 = new IsTagOfType("num", 3,
                "is a number",
                new PositiveDecisionResult<ITaggedData>(),
                new NegativeDecisionResult<ITaggedData>());

            var step3 = new TagHasValue("to", 2,
                "is a range",
                step4,
                new NegativeDecisionResult<ITaggedData>());

            var step2 = new IsTagOfType("num", 1,
                "is a number",
                step3,
                new NegativeDecisionResult<ITaggedData>());

            var step1 = new FirstTagOfType("measure",
                "is a number",
                step2,
                new NegativeDecisionResult<ITaggedData>());

            trunk = new NumberOfTags(4,
                "number of tags = 4",
                step1,
                new NegativeDecisionResult<ITaggedData>());
        }
    }
}