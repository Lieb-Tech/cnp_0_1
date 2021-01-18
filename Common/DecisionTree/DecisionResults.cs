namespace Common.DecisionTree
{
    public static class DecisionResults<T>
    {
        public static Decision<T> GetPositive() => new PositiveDecisionResult<T>();
        public static Decision<T> GetNegative() => new NegativeDecisionResult<T>();
    }
}
