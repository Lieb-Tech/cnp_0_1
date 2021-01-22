
namespace Freeform.FreeformParse
{
    public record ConditionInfo(string Condition, string Info, string History, string Other)
    {
        public string StrategyUsed { get; set; }
    }

    public record ConditionInfoConfiguration(int? Condition, int? Info, int? History, int? Other)
    {

    }
}