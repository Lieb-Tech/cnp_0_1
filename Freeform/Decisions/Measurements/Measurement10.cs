using Common;
using Common.DecisionTree;
using Common.DecisionTree.DecisionQueries;
using Freeform.FreeformParse;

namespace Freeform.Decisions.Measurements
{
    public class Measurement10 : IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>
    {
        private readonly DecisionQuery<ITaggedData> trunk;

        public IStrategy<TextSpanInfoes<MeasurementInfo>> GetDecision(DecisionContext data)
        {
            data = data with { Index = 0, Matched = false };
            trunk.Evaluate(data);

            if (data.Matched)
                return data.Baggage;
            else
                return null;
        }

        /// <summary>
        /// specific use case
        /// {gen:measure:vital signs} {gen:descriptive:stable} .
        /// </summary>

        public Measurement10()
        {
            var checkMeasure0 = new DecisionQuery<ITaggedData>()
            {
                Test = (client) =>
                {
                    if (client.Index == 0)
                        return false;
                    else
                        return client.Tags[client.Index - 1].Contains(":measure", System.StringComparison.InvariantCultureIgnoreCase);
                },
                Positive = new LabelNum<ITaggedData>(),
                Negative = DecisionResults<ITaggedData>.GetNegative()
            };

            var checkMeasure1 = new IsTagOfType("measure", 1,
                "is vital signs type",
                new NumLabel<ITaggedData>(),
                checkMeasure0);

            var step1 = new FirstTagOfType("vitals",
                "is vital signs type",
                checkMeasure1,
                DecisionResults<ITaggedData>.GetNegative());

            trunk = new NumberOfTags(2,
                "number of tags = 2",
                step1,
                DecisionResults<ITaggedData>.GetNegative());
        }

        public class NumLabel<T> : Decision<T>
        {
            public bool Result => true;

            public override void Evaluate(T info)
            {
                if (info is DecisionContext ctx)
                {
                    ctx.Matched = true;
                    ctx.Baggage = new VitalsStrategy(ctx.Index, true);
                }
            }
        }

        public class LabelNum<T> : Decision<T>
        {
            public bool Result => true;

            public override void Evaluate(T info)
            {
                if (info is DecisionContext ctx)
                {
                    ctx.Matched = true;
                    ctx.Baggage = new VitalsStrategy(ctx.Index, false);
                }
            }
        }
    }
}