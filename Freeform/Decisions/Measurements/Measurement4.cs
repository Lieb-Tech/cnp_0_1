using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class Measurement4 : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>> 
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
        /// {gen:measurement:CCC} {gen:num:NNN} {gen:range:CCCC} {gen:num:NNN}
        /// {gen:measurement:calcium} {gen:num:2} {gen:range:to} {gen:num:5}
        /// </summary>
        public Measurement4()
        {
            var step3 = new IsTagOfType("num", 3,
                "is a number",
                new PositiveDecisionResult<ITaggedData>(),
                new NegativeDecisionResult<ITaggedData>());

            var step2 = new IsTagOfType("range", 2,
                "is a range",
                step3,
                new NegativeDecisionResult<ITaggedData>());

            var step1 = new FirstTagOfType("num",
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