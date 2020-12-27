using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class TagNumbers : IStrategy<TextSpan>
    {
        private readonly Regex regex = null;
        public TagNumbers()
        {
            var pattern = @"0?\\d+";
            regex = new Regex(pattern);
        }
        
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var txt = tagNumbers(context.Data.UpdatedText);
            var span = context.Data with { UpdatedText = txt };
            return new StrategyContext<TextSpan>(span, true);
        }

        private string tagNumbers(string updatedText)
        {
            foreach (Match match in regex.Matches(updatedText))
            {
                updatedText = updatedText.Substring(0, match.Index) +
                    " {" + match + "} " +
                    updatedText.Substring(match.Index + match.Length);
            }

            return updatedText;
        }
    }
}
