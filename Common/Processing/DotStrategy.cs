
using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class DotStrategy : IStrategy<TextSpan>
    {
        private readonly Regex _regex = new Regex("[a-zA-Z]\\.([0-9a-zA-Z]|\\s|$)");
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {            
            var results = removeDot(context.Data);
            var span = context.Data with { UpdatedText = results };
            return new StrategyContext<TextSpan>(span, true);
        }

        internal string removeDot(TextSpan text)
        {
            var updated = text.UpdatedText;            
            while (_regex.IsMatch(updated))
            {
                var match = _regex.Match(updated);
                updated = updated.Replace(match.Value, match.Value.Replace(".", ""));
            }

            return updated;
        }
    }
}
