using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Measurements
{
    public class Strategy3 : RemoveTagsStrategy<MeasurementInfo>
    {
        public Strategy3(int offset) : base(3) 
        {
            Offset = offset;
        }
        public int Offset { get; set; }

        public override StrategyContext<TextSpanInfoes<MeasurementInfo>> Execute(StrategyContext<TextSpanInfoes<MeasurementInfo>> context)
        {
            var info = new MeasurementInfo(context.Data.TagsToProcess[Offset + 0].TagValue(),
                context.Data.TagsToProcess[Offset + 1].TagValue(),
                context.Data.TagsToProcess[Offset + 2].TagValue(),
                null);
            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);
            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(data, true);

            return base.Execute(context);
        }
    }
}