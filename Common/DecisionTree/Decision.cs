namespace Common.DecisionTree
{
    public abstract class Decision<T>
    {
        public abstract void Evaluate(T info);
    }
}
