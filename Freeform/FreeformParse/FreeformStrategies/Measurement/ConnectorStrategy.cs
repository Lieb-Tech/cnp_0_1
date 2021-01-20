
using Common;

namespace Freeform.FreeformParse.FreeformStrategies.Measurement
{
    public class ConnectorStrategy : IInprocessAndCompletedStrategy<MeasurementInfo>
    {
        public InprocessAndCompleted<MeasurementInfo> Execute(InprocessAndCompleted<MeasurementInfo> context, string tag)
        {
            var connector = context.InProcess.Connector;
            connector += " " + tag.TagValue();
            context.InProcess = context.InProcess with { Connector = connector.Trim() };

            return context;
        }
    }
}
