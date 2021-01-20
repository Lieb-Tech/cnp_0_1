
namespace Common
{
    public interface IInprocessAndCompletedStrategy<T>
    {
        InprocessAndCompleted<T> Execute(InprocessAndCompleted<T> context, string tag);
    }
}
