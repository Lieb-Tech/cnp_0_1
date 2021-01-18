using Common;
using System.Collections.Generic;

namespace Common.DecisionTree
{
    public interface ITaggedData
    {
        // orignal Span
        TextSpan Span { get; set; }

        // UpdatedText => only tagged
        List<string> Tags { get; set; } 

        // result
        bool Matched { get; set; }

        // orignal Span
        int Index { get; set; }
    }
}