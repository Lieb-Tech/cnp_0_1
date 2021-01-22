﻿using Common;
using Common.DecisionTree;
using Freeform.Decisions;
using Freeform.Decisions.Conditions;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.FreeformParse
{
    public class ConditionTreeParse
    {
        private readonly List<IDecisionTrunk<DecisionContext, TextSpanInfoes<ConditionInfo>>> taggedForest = new();
        public ConditionTreeParse()
        {
            plantForest();
        }

        void plantForest()
        {
            taggedForest.Add(new Condition10());
            taggedForest.Add(new Condition9());
            taggedForest.Add(new Condition8());
            taggedForest.Add(new Condition7());
            taggedForest.Add(new Condition6());
            taggedForest.Add(new Condition5());
            taggedForest.Add(new Condition4());
            taggedForest.Add(new Condition3());
            taggedForest.Add(new Condition2());
            taggedForest.Add(new Condition1());
        }

        public List<ConditionInfo> ProcessLine(TextSpan span)
        {
            // if bulletlist, then remove it
            if (span.UpdatedText.StartsWith("{med:li"))
                span = span with { UpdatedText = span.UpdatedText.Substring(span.UpdatedText.IndexOf("}") + 1) };

            // split text line - only get Tagged items
            TagStringSplit tss = new();
            var tags = tss.SplitTagged(span.UpdatedText);

            // initial value for processing context
            var tsp = new TextSpanInfoes<ConditionInfo>(span,
                ImmutableList<ConditionInfo>.Empty,
                 tags.ToImmutableList());

            var ctx = new StrategyContext<TextSpanInfoes<ConditionInfo>>(tsp, true);

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

        private StrategyContext<TextSpanInfoes<ConditionInfo>> processTrees(TextSpan span, StrategyContext<TextSpanInfoes<ConditionInfo>> ctx)
        {
            // prepare data for processor
            var md = new DecisionContext()
            {
                Span = span,
                Tags = ctx.Data.TagsToProcess.ToList()
            };

            // now check for matches, and then process
            foreach (var tree in taggedForest)
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
