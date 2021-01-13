
using Common;

namespace Freeform.FreeformParse.FreeformStrategies.Measurement
{
    public class ConnectorStrategy : IProcessAndCompletedStrategy<MeasurementInfo>
    {
        public ProcessAndCompletedContext<MeasurementInfo> Execute(ProcessAndCompletedContext<MeasurementInfo> context, string tag)
        {
            var connector = context.InProcess.Connector;
            connector += " " + tag.TagValue();
            context.InProcess = context.InProcess with { Connector = connector.Trim() };

            return context;
        }
    }
}
