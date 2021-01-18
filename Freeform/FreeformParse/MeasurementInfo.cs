namespace Freeform.FreeformParse
{
    public record MeasurementInfo(string Measurement, string Value1, string Connector, string Value2) 
    {
        public string StrategyUsed { get; set; }
    }    
}