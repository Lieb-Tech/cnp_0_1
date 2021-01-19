namespace Common
{
    /// <summary>
    /// Data: POCO of data to be used in stretegy
    /// Continue: Should next step be processed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public record StrategyContext<T>(T Data, bool Continue) { }

}
