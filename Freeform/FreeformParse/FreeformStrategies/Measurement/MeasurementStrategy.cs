using Common;

namespace Freeform.FreeformParse.FreeformStrategies.Measurement
{
    public class MeasurementStrategy : IProcessAndCompletedStrategy<MeasurementInfo>
    {
        public ProcessAndCompletedContext<MeasurementInfo> Execute(ProcessAndCompletedContext<MeasurementInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Measurement = tag.TagValue() };
            return context;
        }
    }
}
