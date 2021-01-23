using System.Text.RegularExpressions;

namespace Common.Processing
{
    /// <summary>
    /// TODO: Update update version with {tN:} --- where N == index of tag
    /// TODO: Update original version with {rN:} --- where N == corresponding index of tag
    /// </summary>
    public class TagRegex : IStrategy<TextSpan>
    {
        private readonly string _tag;
        private readonly Regex _regex = null;
        public TagRegex(string pattern, string tag, bool caseInsensative = true)
        {
            _tag = tag;
            if (!_tag.EndsWith(":"))
                _tag += ":";

            if (caseInsensative)
                _regex = new Regex(pattern, RegexOptions.IgnoreCase);
            else
                _regex = new Regex(pattern);
        }

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var rr = new RegexReplacer();
            var txt = rr.tagPattern(_regex, context.Data.UpdatedText, _tag);
            var span = context.Data with { UpdatedText = txt };
            return new StrategyContext<TextSpan>(span, true);
        }
    }
}