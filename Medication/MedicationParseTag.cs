using Common;
using Medication.MedicationParse;
using System.Collections.Generic;

namespace Medication
{
    /// <summary>
    /// Holds results of tagging and extraction
    /// </summary>
    public record MedicationParseTag(List<MedicationInfo> medications, TextSpan textSpan)
    {
        
    }
}
