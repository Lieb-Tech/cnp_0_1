using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Conditions
{
    class Strategy3 : RemoveTagsStrategy<ConditionInfo>
    {
        public Strategy3(int offset) : base(3, offset)
        {
            Offset = offset;
        }
        public int Offset { get; set; }

        public override StrategyContext<TextSpanInfoes<ConditionInfo>> Execute(StrategyContext<TextSpanInfoes<ConditionInfo>> context)
        {
            var info = new ConditionInfo(context.Data.TagsToProcess[Offset + 2].TagValue(),
                context.Data.TagsToProcess[Offset + 1].TagValue(),
                context.Data.TagsToProcess[Offset + 0].TagValue());
            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);
            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<ConditionInfo>>(data, true);

            return base.Execute(context);
        }
    }
}