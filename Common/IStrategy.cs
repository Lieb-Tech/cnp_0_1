namespace Common
{
    public interface IStrategy<T>
    {
        StrategyContext<T> Execute(StrategyContext<T> context);
    }
}