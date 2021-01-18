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
        private readonly List<IDecisionTrunk<DecisionContext,TextSpanInfoes<MeasurementInfo>>> forest = new();
        public MeasurementTreeParse()
        {
            plantForest();
        } 

        void plantForest()
        {
            forest.Add(new Measurement8());
            forest.Add(new Measurement7());
            forest.Add(new Measurement6());
            forest.Add(new Measurement5());
            forest.Add(new Measurement4());
            forest.Add(new Measurement3());            
            forest.Add(new Measurement2());
            forest.Add(new Measurement1());
        }

        public List<MeasurementInfo> ProcessLine(TextSpan span)
        {
            // if bulletlist, then remove it
            if (span.UpdatedText.StartsWith("{med:li"))
                span = span with { UpdatedText = span.UpdatedText.Substring(span.UpdatedText.IndexOf("}") + 1) };

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
                ctx = processTrees(span, ctx);

                // if tree matched, then start over again
                if (!ctx.Continue)                
                    break;                
            }
            return ctx.Data.Infoes.ToList();
        }
       
        private StrategyContext<TextSpanInfoes<MeasurementInfo>> processTrees(TextSpan span, StrategyContext<TextSpanInfoes<MeasurementInfo>> ctx)
        {
            // prepare data for processor
            var md = new DecisionContext()
            {
                Span = span,
                Tags = ctx.Data.TagsToProcess.ToList()
            };

            // now check for matches, and then process
            foreach (var tree in forest)
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
