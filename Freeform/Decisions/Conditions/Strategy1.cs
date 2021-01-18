using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Conditions
{
    public class Strategy1 : RemoveTagsStrategy<ConditionInfo>
    {
        public Strategy1(int offset) : base(1)
        {
            Offset = offset;
        }
        public int Offset { get; set; }
        public override StrategyContext<TextSpanInfoes<ConditionInfo>> Execute(StrategyContext<TextSpanInfoes<ConditionInfo>> context)
        {
            var info = new ConditionInfo(context.Data.TagsToProcess[Offset].TagValue(),
                null);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);

            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<ConditionInfo>>(data, true);

            return base.Execute(context);
        }
    }
}