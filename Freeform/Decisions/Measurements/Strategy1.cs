using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Measurements
{
    public class Strategy1 : RemoveTagsStrategy<MeasurementInfo>
    {
        public Strategy1(int offset): base(1) 
        {
            Offset = offset;
        }
        public int Offset { get; set; }
        public override StrategyContext<TextSpanInfoes<MeasurementInfo>> Execute(StrategyContext<TextSpanInfoes<MeasurementInfo>> context)
        {
            var info = new MeasurementInfo(context.Data.TagsToProcess[Offset].TagValue(),
                null, null, null);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);

            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
