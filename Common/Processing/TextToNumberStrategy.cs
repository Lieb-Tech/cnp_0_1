using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class TextToNumberStrategy : IStrategy<TextSpan>
    {
        private readonly List<string> nums = new List<string>()
            {
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
            };
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {            
            var text = textRangeToNumber(context.Data.UpdatedText);
            text = singleTextToNumber(text);

            var data = context.Data with { UpdatedText = text };

            return new StrategyContext<TextSpan>(data, true);
        }

        internal string textRangeToNumber(string text)
        {
            var updated = text;
            for (int x = 0; x < 16 ; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    var num = $"{nums[x]} to {nums[y]} ";
                    var replace = $"{x}-{y} ";
                    updated = updated.Replace(num, replace);
                }
            }
            
            return updated;
        }

        internal string singleTextToNumber(string text)
        {
            var rr = new RegexReplacer();
            var updated = text;
            for (int x = 0; x < 16; x++)
            {
                Regex reg = new Regex($" {nums[x]} ");

                if (reg.IsMatch(updated))                
                    updated = rr.replaceValue(reg, updated, $" {x} ");
            }

            return updated;
        }

    }
}