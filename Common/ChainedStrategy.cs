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
            return _second.Execute(_first.Execute(context));    
        }
    }
}
