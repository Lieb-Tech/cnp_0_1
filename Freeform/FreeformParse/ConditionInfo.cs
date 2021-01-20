
namespace Freeform.FreeformParse
{
    public record ConditionInfo(string Condition, string Info, string History)
    {
        public string StrategyUsed { get; set; }
    }
}