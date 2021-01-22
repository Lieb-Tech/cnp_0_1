using Common;
using Freeform.FreeformParse;
using System.Collections.Immutable;
using System.Linq;


namespace Freeform.Decisions.Procedures
{
    public class StrategyMulti : RemoveTagsStrategy<ProcedureInfo>
    {
        public readonly ProcedureInfoConfiguration FieldOrder;
        public int Offset { get; set; }
        public StrategyMulti(int offset, ProcedureInfoConfiguration fieldOrder) : base(4, offset)
        {
            FieldOrder = fieldOrder;
            Offset = offset;
        }
        public override StrategyContext<TextSpanInfoes<ProcedureInfo>> Execute(StrategyContext<TextSpanInfoes<ProcedureInfo>> context)
        {
            string procedure = null, part = null, location = null, condition = null;
            if (FieldOrder.Procedure.HasValue)
                procedure = context.Data.TagsToProcess[Offset + FieldOrder.Procedure.Value].TagValue();
            if (FieldOrder.BodyPart.HasValue)
                part = context.Data.TagsToProcess[Offset + FieldOrder.BodyPart.Value].TagValue();
            if (FieldOrder.Location.HasValue)
                location = context.Data.TagsToProcess[Offset + FieldOrder.Location.Value].TagValue();
            if (FieldOrder.Condition.HasValue)
                condition = context.Data.TagsToProcess[Offset + FieldOrder.Condition.Value].TagValue();

            var condInfo = new ProcedureInfo(procedure, part, location, condition);

            var infoes = context.Data.Infoes.ToList();
            infoes.Add(condInfo);

            var data = context.Data with { Infoes = infoes.ToImmutableList() };
            context = new StrategyContext<TextSpanInfoes<ProcedureInfo>>(data, true);

            return base.Execute(context);
        }
    }
}
