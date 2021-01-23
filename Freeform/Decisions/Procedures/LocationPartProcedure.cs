using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Procedures
{
    public class LocationPartProcedure : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, new ProcedureInfoMap(2, 0, 1, null));
            else
                return null;
        }

        /// <summary>
        /// {gen:procedure:lumpectomy} {gen:part:left} {gen:part:lung} {gen:condition:fibroma}       
        /// </summary>
        public LocationPartProcedure()
        {

            var step3 = new IsTagOfType("procedure", 2,
                "is a procedure",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("part", 1,
                "is a part",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("loc",
                "is a location",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(3,
                "number of tags = 3",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}