using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Procedures
{
    public class OnlyProcedure : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ProcedureInfoMap(0, null, null, null));
            else
                return null;
        }

        /// <summary>
        /// {med:li:1} {gen:procedure:dialysis}
        /// </summary>
        public OnlyProcedure()
        {
            var step2 = new IsTagOfType("procedure", 0,
                "is a procedure",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new DecisionQuery<ITaggedData>()
            {
                Positive = step2,
                Negative = DecisionResults<ITaggedData>.GetNegative(),
                Test = (d) =>
                     {
                         return d.Tags.Count == 1;
                     }
            };                
        }
    }
}