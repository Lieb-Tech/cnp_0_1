using Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Medication.MedicationTag
{
    internal class TextNumberSplitBuilder : IStrategy<TextSpan>
    {
        private readonly Regex _textNum = new Regex("[0-9][a-zA-Z]");
        private readonly List<char> _skip = new List<char>() { 'd', 'D', 'h', 'H' };
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var text = context.Data.UpdatedText;
            var match = _textNum.Match(text);
            while (match != null && match.Value != "")
            {                
                if (_skip.Contains(match.Value[1]))
                    text = text.Substring(0, match.Index + 1) + " " + text.Substring( match.Index + 1);
                match = _textNum.Match(text, match.Index + 2);
            }
            return new StrategyContext<TextSpan>(context.Data with { UpdatedText = text }, true);
        }
    }
}