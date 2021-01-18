namespace Common.DecisionTree
{
    public class NegativeDecisionResult<T> : Decision<T>
    {
        public bool Result => false;

        public override void Evaluate(T Client)
        {
            // nothing to do
        }
    }
}
