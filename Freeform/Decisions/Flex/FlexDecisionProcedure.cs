using Common;
using Common.DecisionTree;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Flex
{
    public class FlexDecisionProcedure : IDecisionTrunk<DecisionContext, TextSpanInfoes<ProcedureInfo>>
    {
        private DecisionQuery<ITaggedData> trunk;
        private ProcedureInfoMap config;

        public void SetTrunk(DecisionQuery<ITaggedData> trunkQuery) => trunk = trunkQuery;

        public void SetConfiguration(ProcedureInfoMap config) => this.config = config;

        public IStrategy<TextSpanInfoes<ProcedureInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return null; //  new StrategyMulti<ProcedureInfo>(data.Index, config)
            else
                return null;
        }
    }
}
