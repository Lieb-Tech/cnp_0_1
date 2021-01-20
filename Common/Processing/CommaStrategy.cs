using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class CommaStrategy  : IStrategy<TextSpan>
    {
        private readonly Regex _regex = new Regex("\\,");
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var results = removeComma(context.Data);
            var span = context.Data with { UpdatedText = results };
            return new StrategyContext<TextSpan>(span, true);
        }

        internal string removeComma(TextSpan text)
        {
            var updated = text.UpdatedText;
            while (_regex.IsMatch(updated))
            {
                var match = _regex.Match(updated);
                updated = updated.Replace(match.Value, match.Value.Replace(",", " "));
            }

            return updated;
        }
    }
}
