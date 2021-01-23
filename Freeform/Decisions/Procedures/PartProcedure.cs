using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Procedures
{
    public class PartProcedure : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ProcedureInfoMap(1, null, 0, null));
            else
                return null;
        }

        /// <summary>
        /// {gen:procedure:lumpectomy} {gen:part:left} {gen:part:lung} {gen:condition:fibroma}       
        /// </summary>
        public PartProcedure()
        {

            var step2 = new IsTagOfType("procedure", 1,
                "is a procedure",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("part",
                "is a bodyPart",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(2,
                "number of tags = 2",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}