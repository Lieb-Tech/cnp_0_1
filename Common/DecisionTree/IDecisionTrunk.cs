namespace Common.DecisionTree
{
    public interface IDecisionTrunk<TInput, TContext>
    {
        IStrategy<TContext> GetDecision(TInput data);
    }
}