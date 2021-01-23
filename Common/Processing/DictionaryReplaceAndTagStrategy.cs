using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Processing
{
    public class DictionaryReplaceAndTagStrategy : IStrategy<TextSpan>
    {
        private string _tag;
        private readonly Dictionary<string, string> _replacments;
        public DictionaryReplaceAndTagStrategy(Dictionary<string, string> replacments, string tag)
        {
            _tag = tag;
            _replacments = replacments;
        }

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var data = context.Data;
            foreach (var kvp in _replacments.OrderByDescending(d => d.Key.Length))
            {
                var updated = processValue(kvp, data);
                data = data with { UpdatedText = updated };
            }
            return new StrategyContext<TextSpan>(data, true);
        }

        private string processValue(KeyValuePair<string, string> kvp, TextSpan data)
        {
            // since am replacing matches with modified + original, can't use .Replace
            // since updatedText is being modified each time, re-evaluate

            string updatedText = data.UpdatedText;
            var idx = updatedText.IndexOf(kvp.Key, StringComparison.CurrentCultureIgnoreCase);
            int lastMatchPos = 0;
            while (idx > -1)
            {
                // create replacement string
                var tagged = $" {{{_tag}{kvp.Value.Trim()}}} ";

                // update index to continue past this update
                lastMatchPos = idx + tagged.Length;

                // do the update
                updatedText = tagText(updatedText, idx, kvp.Key.Length, tagged);

                if (lastMatchPos < updatedText.Length)
                    // check if more to do
                    idx = updatedText.IndexOf(kvp.Key, lastMatchPos, StringComparison.CurrentCultureIgnoreCase);
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