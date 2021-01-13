
namespace Common
{
    public interface IProcessAndCompletedStrategy<T>
    {
        ProcessAndCompletedContext<T> Execute(ProcessAndCompletedContext<T> context, string tag);
    }
}
