
using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Conditions
{
    class ChangeLocationPartDescription : IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<ConditionInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return new StrategyMulti(data.Index, 4, new ConditionInfoMap(2,0,1,3));
            else
                return null;
        }

        /// <summary>
        // {gen:change:CCC} {gen:location:CCC} {gen:part:CCC} {gen:description:CCC}
        // increase in left lung size
        /// </summary>

        public ChangeLocationPartDescription()
        {
            var step4 = new IsTagOfType("description", 3,
                "is a body part",
                DecisionResults<ITaggedData>.GetPositive(),
                DecisionResults<ITaggedData>.GetNegative());

            var step3 = new IsTagOfType("part", 2,
                "body part",
                step4,
                DecisionResults<ITaggedData>.GetNegative());

            var step2 = new IsTagOfType("loc", 1,
                "location",
                step3,
                DecisionResults<ITaggedData>.GetNegative());

            var step1 = new FirstTagOfType("change",
                "location",
                step2,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(4,
                "number of tags = 4",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }
    }
}
