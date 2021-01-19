
namespace Common
{
    public interface IProcessAndCompletedStrategy<T>
    {
        InprocessAndCompleted<T> Execute(InprocessAndCompleted<T> context, string tag);
    }
}
