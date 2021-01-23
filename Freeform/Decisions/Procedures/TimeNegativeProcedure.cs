using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Procedures
{
    class TimeNegativeProcedure : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ProcedureInfoMap(2, null, 0, 1));
            else
                return null;
        }

        /// <summary>
        /// {time negative procedure}
        /// </summary>
        public TimeNegativeProcedure()
        {

            var step3 = new IsTagOfType("procedure", 2,
                "is a procedure",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("negative", 1,
                "is negative",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("time",
                "is a time",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}