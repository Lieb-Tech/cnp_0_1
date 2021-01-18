using System.Text.RegularExpressions;

namespace Common
{
    // Todo: Split into strategies
    public class LineNumBuilder : IStrategy<TextSpan>
    {
        private readonly Regex _dotSpace = new Regex(@"^(\d{1,2}\.\s?)");
        private readonly Regex _space = new Regex(@"^(\d{1,2}\s?)");
        private readonly Regex _dotChar = new Regex(@"^(\d{1,2}\.?[a-zA-Z])");

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            // line starts with:  "1. abcedfghi"
            var match = _dotSpace.Match(context.Data.UpdatedText);
            if (match.Success)
            {
                var updated = _dotSpace.Replace(context.Data.UpdatedText, $"{{med:li:{match.Value.Trim()}}} ");
                var data = context.Data with { UpdatedText = updated };
                return new StrategyContext<TextSpan>(data, true);
            }

            // line starts with:  "1.abcedfghi"
            match = _dotChar.Match(context.Data.UpdatedText);
            if (match.Success)
            {
                var num = match.Value.Substring(0, match.Value.Length - 1);
                var updated = _dotChar.Replace(context.Data.UpdatedText, $"{{gen:li:{num}}}");
                var data = context.Data with { UpdatedText = updated };
                return new StrategyContext<TextSpan>(data, true);
            }

            // line starts with:  "1 abcedfghi"
            match = _space.Match(context.Data.UpdatedText);
            if (match.Success)
            {
                var updated = _space.Replace(context.Data.UpdatedText, $"{{med:li:{match.Value.Trim()}}} ");
                var data = context.Data with { UpdatedText = updated };
                return new StrategyContext<TextSpan>(data, true);
            }

            return context;
        }
    }
}
