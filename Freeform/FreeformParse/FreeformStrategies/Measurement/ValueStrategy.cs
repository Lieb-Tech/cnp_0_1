using Common;

namespace Freeform.FreeformParse.FreeformStrategies.Measurement
{
    public class ValueStrategy : IProcessAndCompletedStrategy<MeasurementInfo>
    {
        public InprocessAndCompleted<MeasurementInfo> Execute(InprocessAndCompleted<MeasurementInfo> context, string tag)
        {
            if (string.IsNullOrEmpty(context.InProcess.Value1))
                context.InProcess = context.InProcess with { Value1 = tag.TagValue() };
            else
                context.InProcess = context.InProcess with { Value2 = tag.TagValue() };

            return context;
        }
    }
}
