using System.Collections.Generic;
using System.Collections.Immutable;

namespace LabWork
{
    public class LabTag
    {
        private readonly List<string> Measurement = new List<string>() 
        {
            "HEIGHT",
            "HEENT",
            "Potassium",   // 6.3 down to 4.8
            "calcium",
            "bun",
            "creatinine",
            "phosphorus",
            "hematocrit",  // hematocrit was 24.0 ; hematocrit of 41.8            
            "blood pressure", // 90/40 to 110/60            
            "S1 and S2",
            "tibial pulses",           

        };

        private readonly ImmutableList<string> Tests = new List<string>()
        {
            "elevated",
            "elevate",
            "CT scan",
            "films",
            "film",
            "plain films",
            "plain film",

        }.ToImmutableList<string>();

        private readonly List<string> Status = new List<string>()
        {
            "Vital signs stable",
            "rrr",
            "Regular rate and rhythm",
            "Afebrile",
            "Alert",
            "orientated",
            "Pleasant",
            "cardiac sounds",            
        };

        // rrr 
        // 

        // VAGINA CERVIX 3-4/100/-1/vtx
    }
}
