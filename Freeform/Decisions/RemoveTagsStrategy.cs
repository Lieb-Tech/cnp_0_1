using Common;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions
{
    public abstract class RemoveTagsStrategy<T> : IStrategy<TextSpanInfoes<T>>
    {
        private int places { get;  init; }
        private int offset { get; init; }
        protected RemoveTagsStrategy(int places, int offset)
        {
            this.offset = offset;
            this.places = places;
        }
        public virtual StrategyContext<TextSpanInfoes<T>> Execute(StrategyContext<TextSpanInfoes<T>> context)
        {
            // remove these from the head of the list
            var remainingToProcess = context.Data.TagsToProcess.Take(offset).ToList();
            remainingToProcess.AddRange(context.Data.TagsToProcess.Skip(offset + places).ToList());

            var tsp = new TextSpanInfoes<T>(context.Data.Span, context.Data.Infoes, remainingToProcess.ToImmutableList());
            return context with { Data = tsp };
        }
    }
}