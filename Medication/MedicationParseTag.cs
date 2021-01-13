using Common;
using Common.MedicationParse;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Holds results of tagging and extraction
    /// </summary>
    public record MedicationParseTag(List<MedicationInfo> medications, TextSpan textSpan)
    {
        
    }
}
