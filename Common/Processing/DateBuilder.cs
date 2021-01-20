
namespace Common.Processing
{
    public class DateBuilder : IStrategy<TextSpan>
    {
        private readonly TagRegex _tagger;
        private readonly string pattern = @"(Jan(uary)?|Feb(ruary)?|Mar(ch)?|Apr(il)?|May|Jun(e)?|Jul(y)?|Aug(ust)?|Sep(tempber|t)?|Oct(ober)?|Nov(ember)?|Dec(ember)?)\s[0-9]{1,2}\s?(st|nd|rd|th)?";
        public DateBuilder()
        {
            _tagger = new TagRegex(pattern, "date");
        }
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            return _tagger.Execute(context);
        }
    }
}