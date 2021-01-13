using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Processing
{
    public class DictionaryTagStrategy : IStrategy<TextSpan>
    {        
        private readonly Dictionary<string, string> _replacments;
        public DictionaryTagStrategy(Dictionary<string, string> replacments)
        {            
            _replacments = replacments;
        }

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var data = context;
            foreach (var kvp in _replacments.OrderByDescending(d => d.Key.Length))
            {
                var pattern = "\\b" + kvp.Key.Replace(".", "\\.").Replace(".", "\\.") + "\\b";
                data = (new TagRegex(pattern, kvp.Value)).Execute(data);
            }
            return data;
        }
        
        /*********
        private string processValue(KeyValuePair<string, string> kvp, TextSpan data)
        {
            // since am replacing matches with modified + original, can't use .Replace
            // since updatedText is being modified each time, re-evaluate

            string updatedText = data.UpdatedText;
            var idx = updatedText.IndexOf(kvp.Key, StringComparison.CurrentCultureIgnoreCase);
            int lastMatchPos = -1;
            while (idx > -1)
            {                
                // create replacement string
                var tagged = $" {{{kvp.Value.Trim()}{kvp.Key.Trim()}}} ";

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
        
        private string tagText(string updatedText, int idx, int matchLen, string tagged)
        {
            // skip if already tagged
            if (updatedText.IsInTag(idx))
                return updatedText;

            // good to go, so replace the text
            updatedText = updatedText.Substring(0, idx) +
                tagged +
                updatedText.Substring(idx + matchLen);
            

            return updatedText;
        }
        *****/
    }
}