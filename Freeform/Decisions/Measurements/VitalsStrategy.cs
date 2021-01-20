using Common;
using Freeform.FreeformParse;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Measurements
{
    public class VitalsStrategy : RemoveTagsStrategy<MeasurementInfo>
    {
        private readonly bool numFirst = false;
        public VitalsStrategy(int offset, bool numFirst) : base(3)
        {
            this.numFirst = numFirst;
            Offset = offset;
        }
        public int Offset { get; set; }

        public override StrategyContext<TextSpanInfoes<MeasurementInfo>> Execute(StrategyContext<TextSpanInfoes<MeasurementInfo>> context)
        {
            MeasurementInfo info;
            
            if (numFirst)
                info = new MeasurementInfo(context.Data.TagsToProcess[Offset + 1].TagValue(),
                    context.Data.TagsToProcess[Offset + 0].TagValue(),
                    null,
                    null);
            else
                info = new MeasurementInfo(context.Data.TagsToProcess[Offset + 0].TagValue(),
                    context.Data.TagsToProcess[Offset + 1].TagValue(),
                    null,
                    null);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);
            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<MeasurementInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
