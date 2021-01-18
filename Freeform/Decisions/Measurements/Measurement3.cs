using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class Measurement3 : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
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
        /// {gen:measurement:CCC} {gen:num:NNN} {gen:change:CCCC} {gen:num:NNN}
        /// {gen:measurement:calcium} {gen:num:5} {gen:change:down to} {gen:num:2}
        /// </summary>
        public Measurement3()
        {
            var step4 = new IsTagOfType("num", 3,
                "is a number",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step3 = new IsTagOfType("change", 2,
                "is a change",
                step4,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("num",1,
                "is a number",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("measure",
                "find a measurement",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(4,
                "number of tags = 4", 
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}
