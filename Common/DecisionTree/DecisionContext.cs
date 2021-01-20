using System.Collections.Generic;

namespace Common.DecisionTree
{
    public record DecisionContext : ITaggedData
    {
        // orignal Span
        public TextSpan Span { get; set; }

        // UpdatedText => only tagged
        public List<string> Tags { get; set; } = new();

        // result
        public bool Matched { get; set; }

        // orignal Span
        public int Index { get; set; }

        // extra info for 
        public dynamic Baggage { get; set; }
    }
}
