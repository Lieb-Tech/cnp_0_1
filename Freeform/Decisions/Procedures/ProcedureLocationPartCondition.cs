using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Procedures
{
    public class ProcedureLocationPartCondition : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ProcedureInfoMap(0, 1, 2, 3));
            else
                return null;
        }

        /// <summary>
        /// {gen:procedure:lumpectomy} {gen:part:left} {gen:part:lung} {gen:condition:fibroma}       
        /// </summary>
        public ProcedureLocationPartCondition()
        {
            var step4 = new IsTagOfType("condition", 3,
                "is a condition",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step3 = new IsTagOfType("part", 2,
                "is a body part",
                step4,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("loc", 1,
                "is a location",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("proced",
                "is a Procedure",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(4,
                "number of tags = 4",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}