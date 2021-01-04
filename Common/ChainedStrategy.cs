namespace Common
{
    public class ChainedStrategy<T> : IStrategy<T>
    {
        protected readonly IStrategy<T> _first;
        protected readonly IStrategy<T> _second;
        public ChainedStrategy(IStrategy<T> first, IStrategy<T> second) 
        {
            _first = first;
            _second = second;
        }
        public StrategyContext<T> Execute(StrategyContext<T> context)
        {
            // execute first action
            context = _first.Execute(context);

            // if can continue, then do next
            if (context.Continue)
                context = _second.Execute(context);

            return context;
        }
    }
}
