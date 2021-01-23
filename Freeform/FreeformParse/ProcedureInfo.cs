namespace Freeform.FreeformParse
{
    public record ProcedureInfo(string Procedure, string Location, string BodyPart, string Condition)
    {
        public string StrategyUsed { get; set; }
    }

    public record ProcedureInfoMap(int? Procedure, int? Location, int? BodyPart, int? Condition) { }
}