namespace Common
{
    /// Stores the following info:
    /// Speification (if matches)
    /// SetSpecification (if already been set)
    /// Strategy to execute
    public record SpecificationSetStrategy<T> (Specification<string> specification, 
        Specification<T> set, 
        IProcessAndCompletedStrategy<T> strategy)
    {
    }
}
