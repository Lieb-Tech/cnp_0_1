namespace Common.DecisionTree
{
    public class DecisionResult<T> : Decision<T>
    {
        public T Result { get; set; }

        public override void Evaluate(T Client)
        {
            // nothing to do, as it's a result
        }
    }
}
