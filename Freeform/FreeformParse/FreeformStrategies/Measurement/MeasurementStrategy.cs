using Common;

namespace Freeform.FreeformParse.FreeformStrategies.Measurement
{
    public class MeasurementStrategy : IProcessAndCompletedStrategy<MeasurementInfo>
    {
        public InprocessAndCompleted<MeasurementInfo> Execute(InprocessAndCompleted<MeasurementInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Measurement = tag.TagValue() };
            return context;
        }
    }
}
