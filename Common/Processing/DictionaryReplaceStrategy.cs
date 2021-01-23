using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class DictionaryReplaceStrategy : IStrategy<TextSpan>
    {
        private readonly Dictionary<string, string> _replacments;
        public DictionaryReplaceStrategy(Dictionary<string, string> replacments)
        {
            _replacments = replacments;
        }

        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var data = context;
            foreach (var kvp in _replacments.OrderByDescending(d => d.Key.Length))
            {
                data = replaceValue(data, kvp);
            }
            return data;
        }

        private static StrategyContext<TextSpan> replaceValue(StrategyContext<TextSpan> data, KeyValuePair<string, string> kvp)
        {
            var pattern = "\\b" + kvp.Key.Replace(".", "\\.").Replace(".", "\\.") + "\\b";
            var regEx = new Regex(pattern);

            int startIdx = 0;
            while (regEx.IsMatch(data.Data.UpdatedText, startIdx))
            {
                var match = regEx.Match(data.Data.UpdatedText);

                if (data.Data.UpdatedText.IsInTag(match.Index))
                    continue;

                var updated = data.Data.UpdatedText.Replace(kvp.Key, kvp.Value);
                var span = data.Data with { UpdatedText = updated };
                data = data with { Data = span };
                startIdx = match.Index + 1;
            }
            
            return data;
        }
    }
}