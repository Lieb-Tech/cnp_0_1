namespace Common.DecisionTree
{
    public class PositiveDecisionResult<T> : Decision<T>
    {
        public bool Result => true;

        public override void Evaluate(T Client)
        {
            // nothing to do
        }
    }
}
