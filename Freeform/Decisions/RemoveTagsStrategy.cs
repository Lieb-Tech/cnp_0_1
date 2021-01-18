using Common;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions
{
    public abstract class RemoveTagsStrategy<T> : IStrategy<TextSpanInfoes<T>>
    {
        private int places { get;  init; }
        protected RemoveTagsStrategy(int places)
        {
            this.places = places;
        }
        public virtual StrategyContext<TextSpanInfoes<T>> Execute(StrategyContext<TextSpanInfoes<T>> context)
        {
            // remove these from the head of the list
            var remainingToProcess = context.Data.TagsToProcess.Skip(places).ToImmutableList();            
            var tsp = new TextSpanInfoes<T>(context.Data.Span, context.Data.Infoes, remainingToProcess);
            return context with { Data = tsp };
        }
    }
}
