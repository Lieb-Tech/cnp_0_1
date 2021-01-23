using Common;
using Common.DecisionTree;
using Freeform.Decisions;
using Freeform.Decisions.Measurements;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.FreeformParse
{
    public class MeasurementTreeParse
    {
        // descision trees for only tagged items
        private readonly List<IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>> taggedForest = new();
        // decision trees using all entries in tags
        private readonly List<IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>> allForest = new();
        public MeasurementTreeParse()
        {
            plantForest();
        }

        void plantForest()
        {
            allForest.Add(new MeasureDescriptive());
            allForest.Add(new DescriptiveMeasure());

            taggedForest.Add(new DescriptiveMeasure());
            taggedForest.Add(new MeasureDescriptive());

            taggedForest.Add(new TimeMeasureNum());
            taggedForest.Add(new Ultra_Condition());
            taggedForest.Add(new VitalsMeasure());
            taggedForest.Add(new MeasureNegative());
            taggedForest.Add(new MeasurePositive());
            taggedForest.Add(new UltrasoundNumPart());
            taggedForest.Add(new MeasureNumToNum());
            taggedForest.Add(new MeasureNumRangeNum());
            taggedForest.Add(new MeasureNumChangeNum());
            taggedForest.Add(new MeasureNum());
            taggedForest.Add(new OnlyMeasure());
        }

        public List<MeasurementInfo> ProcessLine(TextSpan span)
        {
            // if bulletlist, then remove it
            if (span.UpdatedText.StartsWith("{med:li"))
                span = span with { UpdatedText = span.UpdatedText.Substring(span.UpdatedText.IndexOf("}") + 1) };

            var values = allValues(span);
            values.AddRange(taggedValues(span));
            return values;
        }

        private List<MeasurementInfo> allValues(TextSpan span)
        {
            // split text line - only get Tagged items
            TagStringSplit tss = new();
            var tags = tss.Split(span.UpdatedText);

            // for processing after match
            var tsp = new TextSpanInfoes<MeasurementInfo>(span,
                ImmutableList<MeasurementInfo>.Empty,
                tags.ToImmutableList());

            var ctx = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(tsp, true);

            // keep going while there's data process
            while (ctx.Data.TagsToProcess.Any())
            {
                // prepare context for processing
                ctx = ctx with { Continue = false };

                // submit to tree to process
                ctx = processTaggedTrees(allForest, span, ctx);

                // if tree matched, then start over again
                if (!ctx.Continue)
                    break;
            }
            return ctx.Data.Infoes.ToList();
        }

        private List<MeasurementInfo> taggedValues(TextSpan span)
        {
            // split text line - only get Tagged items
            TagStringSplit tss = new();
            var tags = tss.SplitTagged(span.UpdatedText);

            // for processing after match
            var tsp = new TextSpanInfoes<MeasurementInfo>(span,
                ImmutableList<MeasurementInfo>.Empty,
                tags.ToImmutableList());

            var ctx = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(tsp, true);

            // keep going while there's data process
            while (ctx.Data.TagsToProcess.Any())
            {
                // prepare context for processing
                ctx = ctx with { Continue = false };

                // submit to tree to process
                ctx = processTaggedTrees(taggedForest, span, ctx);

                // if tree matched, then start over again
                if (!ctx.Continue)
                    break;
            }
            return ctx.Data.Infoes.ToList();
        }

        private StrategyContext<TextSpanInfoes<MeasurementInfo>> processTaggedTrees(List<IDecisionTrunk<DecisionContext, TextSpanInfoes<MeasurementInfo>>>  trees, TextSpan span, StrategyContext<TextSpanInfoes<MeasurementInfo>> ctx)
        {
            // prepare data for processor
            var md = new DecisionContext()
            {
                Span = span,
                Tags = ctx.Data.TagsToProcess.ToList()
            };

            // now check for matches, and then process
            foreach (var tree in trees)
            {
                // get result from tree
                var result = tree.GetDecision(md with { });

                // if returned a strategy continue
                if (result != null)
                {
                    // do it
                    ctx = result.Execute(ctx);
                    // log which strategy used
                    ctx.Data.Infoes.Last().StrategyUsed = tree.ToString();

                    break;
                }
            }

            return ctx;
        }
    }
}
