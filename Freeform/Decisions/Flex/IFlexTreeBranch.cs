namespace Freeform.Decisions.Flex
{
    public interface IFlexTreeBranch
    {
        bool isTrunk { get; init; }
        string label { get; init ; }
        string query { get; init ; }
        string argument { get; init ; }
        int? index { get; init ; }
        string positive { get; init ; }
        string negative { get; init ; }
    }
}
