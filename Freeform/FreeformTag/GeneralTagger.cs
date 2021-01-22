using Common;
using Common.Processing;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Freeform.FreeformTag
{
    public class GeneralTagger 
    {
        private readonly DictionaryTagStrategy _proceessingStrategy;
        private readonly Dictionary<string,string> _tagInfoes = new Dictionary<string, string>();

        public TextSpan ProcessLine(TextSpan span)
        {
            var ctx = new StrategyContext<TextSpan>(span, true);
            var updated = _proceessingStrategy.Execute(ctx);
            return updated.Data;
        }

        public GeneralTagger()
        {
            addValues(Location, "loc");
            addValues(Descriptive, "descriptive");
            addValues(BodyPart, "part");
            addValues(other, "other");
            addValues(Procedures, "procedure");
            addValues(Conditions, "condition");
            addValues(Devices, "device");
            addValues(Behaviors, "behavior");
            addValues(Measurement, "measure");
            addValues(Tests, "test");
            addValues(Status, "status");

            addValuesWithDash(Location, "loc");

            addKeyValuePairs();

            var ordered = _tagInfoes.OrderByDescending(z => z.Key.Split(" ").Length).ThenByDescending(z => z.Key.Length).ToDictionary(z => z.Key, z => z.Value);
            _proceessingStrategy = new DictionaryTagStrategy(ordered);
        }

        private void addValuesWithDash(ImmutableList<string> values, string key)
        {
            foreach (var value in values)
            {
                _tagInfoes.Add(value + "-", $"gen:{key}:");
            }
        }

        private void addValues(List<string> values, string key)
        { 
            foreach (var value in values)
            {
                _tagInfoes.Add(value, $"gen:{key}:");
            }
        }
        private void addValues(ImmutableList<string> values, string key)
        {
            foreach (var value in values)
            {
                _tagInfoes.Add(value, $"gen:{key}:");
            }
        }

        private void addKeyValuePairs()
        {
            foreach (var kvp in TagPairs)
            {
                _tagInfoes.Add(kvp.Key, $"gen:{kvp.Value}:");
            }

            foreach (var kvp in NormalizationPairs)
            {
                _tagInfoes.Add(kvp.Key, $"gen:{kvp.Value}:");
            }
        }

        /******************/

        private readonly ImmutableList<string> Stopwords = new List<string>()
        {
            "however"
        }.ToImmutableList<string>();

        private readonly ImmutableDictionary<string, string> TagPairs = new Dictionary<string, string>()
        {
            {"is a" , "link" },
            {"is" , "link" },

            { "also", "connector" },
            {"in", "connector" },
            {"to", "connector" },
            { "as well as", "connector" },            
            
            {"with an", "with" },
            {"with a", "with" },
            {"with", "with" },
            { "without", "negative:with" },

            {"on", "connector" },
            {"due to", "connector" },
            
            {"has a", "posses" },
            {"has", "posses" },
            {"had a", "posses" },
            {"had", "posses" },
            {"their", "posses" },

            {"need for", "connector" },
            {"episodes of", "connector" },
            {"including", "connector" },
            { "which were", "connector" },
            { "were", "connector" },
            
            {"and at", "conjunction" },
            {"and", "conjunction" },

            { "decrease from", "change" },
            { "decrease to", "change" },
            { "decrease", "change" },
            { "increase from", "change" },
            { "increase to", "change" },
            { "increase in", "change" },
            { "increase", "change" },

            { "down from", "change" },
            { "down to", "change" },
            { "up to", "change" },
            { "up from", "change" },

            {"within normal limits", "Positive" },

            {"no", "Negative" },
            {"non", "Negative" },
            {"non-", "Negative" },
            {"not", "Negative" },
            {"negative", "Negative" },
            {"hasnt", "Negative" },
            {"hasn't", "Negative" },
            {"has not", "Negative" },
            {"was not", "Negative" },
            {"wasnt", "Negative" },
            {"wasn't", "Negative" },
            
            {"at this stage", "time" },
            {"status post", "time" },
            {"postdialysis" , "time" },
            {"post", "time" },
            {"known", "time" },
            {"history", "time" },
            {"history of", "time" },
            {"before", "time" },
            {"pre-", "time" },
            {"pre", "time" },
            {"Post-operatively", "time" },
            {"Post-operative", "time" },
            {"Post operatively", "time" },
            {"Post operative", "time" },
            {"Postop", "time" },
            {"Post-op", "time" },
            {"Postoperatively", "time" },
            {"Postoperative", "time" },

            {"Pre-operatively", "time" },
            {"Pre-operative", "time" },
            {"Pre operatively", "time" },
            {"Pre operative", "time" },
            {"Preop", "time" },
            {"Pre-op", "time" },
            {"Preoperatively", "time" },
            {"Preoperative", "time" },

            {"subsequently", "time" },

            {"chronic", "time" },

            {"is married", "patient:status" },
            {"is not married", "patient:status" },
            {"is single", "patient:status" },
            
            {"attending physician", "physician" },
            {"attending", "physician" },
            {"PCP", "physician" },
            {"nephrologist", "physician" },
            {"physician on-call", "physician" },
            {"on-call", "physician" },
            {"oncall", "physician" },
            {"on call", "physician" },
            {"nurse practitioner", "physician" },
            {"fellow", "physician" },
            {"nurse", "physician" },
            {"nurses", "physician" },
            
            // {"who", "patient" },
            // {"they", "patient:pro" },            
            // {"patient", "patient" },
            // {"patients", "patient" },
            // {"her", "patient:pro" },
            // {"his", "patient:pro" },
            // {"he", "patient:pro" },
            // {"she", "patient:pro" },

            {"admitted", "patient" },
            {"admit", "patient" },
            
            {"measuring","verb" },
            {"kept", "verb" },
            {"keep", "verb" },
            {"complicated", "verb" },
            {"underwent", "verb" },
            {"poor result", "verb" },
            {"taken down", "verb" },

        }.ToImmutableDictionary<string, string>();

        private readonly ImmutableDictionary<string, string> NormalizationPairs = new Dictionary<string, string>()
        {
            { "angio access", "angioaccess" },
            {"AV graft", "arteriovenous graft" },
            {"CKD", "chronic kidney disease" },
            { "GI", "Gastrointestinal"},
            { "EXTREM", "Extremities" },

            { "first", "1st" },
            { "second", "2nd" },
            { "third", "3rd" },
            { "fourth", "4th" },
            { "fifth", "5th" },
            { "sixth", "6th" },
            { "seventh", "7th" },
            { "eigth", "8th" },
            { "nineth", "9th" },

        }.ToImmutableDictionary<string, string>();

        private readonly ImmutableList<string> Descriptive = new List<string>()
        {
            "size",
            "heavy",
            "calcified",
            "exophytic",
            "discomfort",
            "stable",
            "poor",
            "immediate",
            "full",
            "fully",            
            "significant amount",
            "fixation",
            "distended",            
            "fibrosed",
            "swelling",
            "occluded",
            "ongoing",
            "failed",
            "clear",
            "clean",
            "dry",
            "intact",
            "normal",
            "Benign",
            "extensive",
            "low",
            "labile",
            "Abnormal",
            "palpable",
            "Soft",
            "everted",
            "tense",
            "tender",
            "comminuted",            
            "full range of motion",
            "reactive to light",
            "equally",
            "round",
            "small",
        }.ToImmutableList();

        private readonly ImmutableList<string> Location = new List<string>()
        {
            "in the",
            "mid",
            "left",
            "right",
            "proximal",
            "anteriorly",
            "anterior",
            "base",
            "bases",
            "bilaterally",
            "bilateral",
            "posterior",
            "lower",
            "extremity",
            "lateral",
            "medial",

        }.ToImmutableList();

        private readonly ImmutableList<string> BodyPart = new List<string>()
        {
            "rotator cuff",
            "shoulder",
            "Ankle",
            "abdominal",
            "Uterine",
            "colon",
            "uterine",
            "breast",
            "pelvic",
            "pelvis",
            "bladder",
            "tibiofibular",
            "web space",
            "dorsalis pedis",
            "calf",
            "leg",
            "oropharynx",
            "pupils",
            "pupil",
            "Abdomen",
            "Lung",
            "Lungs",
            "Heart",
            "forearm",
            "arm",
            "Abdominal",
            "Vaginal",
            "MOUTH",
            "Ear",
            "Neck",
            "Thyroid",
            "breasts",
            "NIPPLES",
            "CHEST",
            "VULVA",
            "CERVIX",
            "ADNEXAE",
            "UTERUS",
            "BICONDYLAR TIBIAL PLATEAU",
            "knee",
            "extensor hallucis longus",
            "tibialis anterior",
            "peroneals",
            "gastrocnemius",
            "foot"

        }.ToImmutableList();

        private readonly List<string> other = new List<string>() 
        {
            "motor function",
            "motor",
            "sensory function",
            "note",
            "notes",
        };

        private readonly ImmutableList<string> Procedures = new List<string>() 
        {
            "lumpectomy",
            "myomectomy",
            "tonsillectomy",
            "adenoidectomy",
            "surgery",
            "Bohler frame",
            "fluid removal",
            "AV graft thrombectomy",
            "Hemodialysis",
            "dialysis",
            "dialysis AV graft thrombectomy",
            "dialysis angioaccess",
            "transplant",
            "catheter placement",
            "fluid bolus",
            "fluid boluses",
            "Head CT",
            "Normal Vaginal Delivery",
            "Spontaneous Vertex Vaginal Delivery",
            "Pap Test",
            "pap smear",
            "immobilizer",
            "dressing"

        }.ToImmutableList();

        private readonly ImmutableList<string> Devices = new List<string>()
        {
            "dialysis tunneled angiocatheter",
            "Tunneled hemodialysis catheter",
            "dialysis arteriovenous graft",
            "arteriovenous graft",
            "graft",
            

        }.ToImmutableList();

        private readonly ImmutableList<string> Behaviors = new List<string>()
        {
            "alcohol use",
            "smoker",
            "outpatient dialysis"
        }.ToImmutableList();

        private readonly List<string> Conditions = new List<string>()
        {
            "kidney stones",
            "stones",
            "hepatitis",
            "urinary tract infection",
            "cirrhosis of the liver",
            "cirrhosis",
            "cancer",
            "orthostatic dizziness",
            "jugular venous distention",
            "systolic ejection murmur", 
            "fibroma",
            "pressure",
            "fundus",            
            "fibroid",
            "fibroids",
            "spasm",
            "wound",
            "wounded",
            "tear",
            "pain",
            "neurovascular",
            "compartment syndrome",
            "loss of consciousness",
            "FRACTURE",
            "edema",
            "murmer",
            "masses",
            "Depression",
            "positional vertigo",
            "labyrinthitis",
            "DIZZINESS",
            "GI bleeding",
            "ischemic heart disease",
            "ecchymosis",
            "hematoma",
            "Ejection systolic murmur",
            "systolic Ejection murmur",
            "Coronary artery disease status post myocardial infarction",
            "Coronary artery disease status post MI",
            "coronary artery disease",
            "Hyperlipidemia",
            "Abdominal aortic aneurysm",
            "Restless leg syndrome",
            "Chronic obstructive pulmonary disease",
            "polycystic kidney disease",
            "Thrombosed dialysis arteriovenous graft",
            "Endstage renal disease",
            "Hyperkalemia",             
            "Anemia",
            "Dialysis",
            "stenosis",
        };

        private readonly List<string> Measurement = new List<string>()
        {
            "urinalysis",
            "Vital signs",
            "Vital sign",
            "Ultrasound",
            "heart rate",
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

        }.ToImmutableList();

        private readonly List<string> Status = new List<string>()
        {
            
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

    public record TagInfo(string key, string tag)
    {
        public int Size => key.Split(" ").Length;
    }    

}
