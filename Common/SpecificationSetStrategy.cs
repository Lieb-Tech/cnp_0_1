namespace Common
{
    public record SpecificationSetStrategy<T> (Specification<string> specification, 
        Specification<T> set, 
        IProcessAndCompletedStrategy<T> strategy)
    {
    }
}
