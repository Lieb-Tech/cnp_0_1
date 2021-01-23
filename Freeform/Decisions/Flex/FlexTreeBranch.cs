namespace Freeform.Decisions.Flex
{
    public record FlexTreeBranch<T>(string label, 
        string query,
        string argument,
        int? index,
        string positive,
        string negative,
        bool isTrunk) :  IFlexTreeBranch
    {

    }    
}
