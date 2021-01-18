using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;


namespace Freeform.Decisions.Measurements
{
    public class Strategy2 : RemoveTagsStrategy<MeasurementInfo>
    {
        public int Offset { get; set; }
        public Strategy2(int offset) : base(2) 
        {
            Offset = offset;
        }
        public override StrategyContext<TextSpanInfoes<MeasurementInfo>> Execute(StrategyContext<TextSpanInfoes<MeasurementInfo>> context)
        {
            var info = new MeasurementInfo(context.Data.TagsToProcess[Offset + 0].TagValue(), 
                context.Data.TagsToProcess[Offset + 1].TagValue(),
                null, null);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);
            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
