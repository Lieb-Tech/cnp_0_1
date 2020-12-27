using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Processing
{
    public class StringListStrategy : IStrategy<TextSpan>
    {
        private readonly string _tag;
        private readonly List<string> _replacments;
        public StringListStrategy(List<string> replacments, string tag)
        {
            _tag = tag;
            _replacments = replacments;
        }

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var data = context.Data;
            var updated = context.Data.UpdatedText;
            foreach (var value in _replacments.OrderByDescending(d => d.Length))
            {
                updated = processValue(value, updated);                
            }
            data = data with { UpdatedText = updated };
            return new StrategyContext<TextSpan>(data, true);
        }

        private string processValue(string value, string updatedText)
        {
            var idx = updatedText.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
            int lastMatchPos = 0;
            while (idx > -1)
            {
                // create replacement string
                var tagged = $" {{{_tag}{value.Trim()}}} ";

                // update index to continue past this update
                lastMatchPos = idx + tagged.Length;

                // do the update
                updatedText = tagText(updatedText, idx, value.Length, tagged);

                if (lastMatchPos < updatedText.Length)
                    // check if more to do
                    idx = updatedText.IndexOf(value, lastMatchPos, StringComparison.CurrentCultureIgnoreCase);
                else
                    idx = -1;
            }

            return updatedText;
        }

        private static string tagText(string updatedText, int idx, int matchLen, string tagged)
        {
            // skip if already tagged
            if (!updatedText.IsInTag(idx))
            {
                // good to go, so replace the text
                updatedText = updatedText.Substring(0, idx) +
                    tagged +
                    updatedText.Substring(idx + matchLen);
            }

            return updatedText;
        }
    }
}