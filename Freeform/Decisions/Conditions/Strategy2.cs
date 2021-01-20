using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.Decisions.Conditions
{
    public class Strategy2 : RemoveTagsStrategy<ConditionInfo>
    {
        public int Offset { get; set; }
        public Strategy2(int offset) : base(2)
        {
            Offset = offset;
        }
        public override StrategyContext<TextSpanInfoes<ConditionInfo>> Execute(StrategyContext<TextSpanInfoes<ConditionInfo>> context)
        {
            var info = new ConditionInfo(context.Data.TagsToProcess[Offset + 1].TagValue(),
                context.Data.TagsToProcess[Offset + 0].TagValue(), 
                null);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(info);
            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<ConditionInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
