using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;


namespace Freeform.Decisions.Conditions
{
    public class StrategyMulti : RemoveTagsStrategy<ConditionInfo>
    {
        public readonly ConditionInfoMap FieldOrder;
        public int Offset { get; set; }
        public StrategyMulti(int offset, int count, ConditionInfoMap fieldOrder) : base(count, offset)
        {
            FieldOrder = fieldOrder;
            Offset = offset;
        }
        public override StrategyContext<TextSpanInfoes<ConditionInfo>> Execute(StrategyContext<TextSpanInfoes<ConditionInfo>> context)
        {
            string condition = null, info = null, history = null, other = null;
            if (FieldOrder.Condition.HasValue)
                condition = context.Data.TagsToProcess[Offset + FieldOrder.Condition.Value].TagValue();
            if (FieldOrder.Info.HasValue)
                info = context.Data.TagsToProcess[Offset + FieldOrder.Info.Value].TagValue();
            if (FieldOrder.History.HasValue)
                history = context.Data.TagsToProcess[Offset + FieldOrder.History.Value].TagValue();
            if (FieldOrder.Other.HasValue)
                other = context.Data.TagsToProcess[Offset + FieldOrder.Other.Value].TagValue();

            var condInfo = new ConditionInfo(condition, info, history, other);
                
            var infoes = context.Data.Infoes.ToList();
            infoes.Add(condInfo);

            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<ConditionInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
