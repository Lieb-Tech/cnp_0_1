
namespace Common.Processing
{
    public class DotStrategy : IStrategy<TextSpan>
    {        
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {            
            var results = removeDot(context.Data);
            var span = context.Data with { UpdatedText = results };
            return new StrategyContext<TextSpan>(span, true);
        }

        internal string removeDot(TextSpan text)
        {            
            return text.UpdatedText.Replace(".", "");
        }
    }
}
