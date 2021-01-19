
namespace Common
{
    /// <summary>
    /// Extension class to allow fluent chaining of strategies
    /// </summary>
    public static class StrategyExtension
    {
        public static ChainedStrategy<T> Then<T>(this IStrategy<T> first, IStrategy<T> next)
        {
            var chained = new ChainedStrategy<T>(first, next);
            return chained;
        }
    }
}
