using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
            var regEx = new Regex($"\\b{value}\\b", RegexOptions.CultureInvariant);
            int lastMatchPos = 0;
            while (regEx.IsMatch(updatedText, lastMatchPos))
            {
                // create replacement string
                var tagged = $" {{{_tag}{value.Trim()}}} ";
                var idx = regEx.Match(updatedText, lastMatchPos).Index;

                // update index to continue past this update
                lastMatchPos = idx + tagged.Length;

                // do the update
                updatedText = tagText(updatedText, idx, value.Length, tagged);              
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