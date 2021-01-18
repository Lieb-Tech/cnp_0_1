using Common;
using System.Collections.Immutable;

namespace Freeform.Decisions
{
    // MeasurementInfo
    public record TextSpanInfoes<T>(TextSpan Span, ImmutableList<T> Infoes, ImmutableList<string> TagsToProcess)
    {
        public int Index { get; set; }
    }
}
