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
            allForest.Add(new Measurement9());

            taggedForest.Add(new Measurement12());
            taggedForest.Add(new Measurement11());
            taggedForest.Add(new Measurement10());
            taggedForest.Add(new Measurement8());
            taggedForest.Add(new Measurement7());
            taggedForest.Add(new Measurement6());
            taggedForest.Add(new Measurement5());
            taggedForest.Add(new Measurement4());
            taggedForest.Add(new Measurement3());
            taggedForest.Add(new Measurement2());
            taggedForest.Add(new Measurement1());
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
