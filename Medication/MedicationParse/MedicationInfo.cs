using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Medication.MedicationParse
{
    // Object for medication
    public record MedicationInfo
    {
        public List<string> Tags { get; set; }
        public string OriginalText { get; set; }

        public string PrimaryName { get; set; }
        public string SecondaryName { get; set; }
        public string InferredName { get; set; }

        public string Unit { get; set; }    // mg, tablet,
        public string Size { get; set; }      // 200 mg
        public string Format { get; set; }  // Tablet, IV
        public string Method { get; set; }   // Sublingual, IV, PO
        public string Frequency { get; set; }   // day, qid,        
        public string Qualifier { get; set; }   // every day
        public string Instruction { get; set; }   // with meal

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(PrimaryName)) sb.Append($"{PrimaryName} ");
            if (!string.IsNullOrEmpty(InferredName)) sb.Append($"{InferredName} ");
            if (!string.IsNullOrEmpty(Size)) sb.Append($"{Size} ");
            if (!string.IsNullOrEmpty(Unit)) sb.Append($"{Unit} ");
            if (!string.IsNullOrEmpty(Format)) sb.Append($"{Format} ");
            if (!string.IsNullOrEmpty(Method)) sb.Append($"{Method} ");
            if (!string.IsNullOrEmpty(Frequency)) sb.Append($"{Frequency} ");
            if (!string.IsNullOrEmpty(Qualifier)) sb.Append($"{Qualifier} ");
            if (!string.IsNullOrEmpty(Instruction)) sb.Append($"{Instruction} ");

            return sb.ToString();
        }

        /// <summary>
        /// 1 == higher the confidence
        /// Max == no confidence
        /// </summary>
        public double Confidence
        {
            get
            {
                // if tags contain no spaces & all tags were set, then score would be 1
                // if tags contain phrases (free form text), then the score would be higher  

                // if none set - then no confidence
                if (ItemsSet == 0 || Tags.Count == 0)
                    return double.MaxValue;

                // if only Size set - and a few items in array, then no confidence
                if (ItemsSet == 1 && !string.IsNullOrEmpty(Size) && Tags.Count < 3)
                    return double.MaxValue;

                double offset = 0;
                var nonTag = Tags.Where(t => !t.Contains("{"));

                //if no tags, then get average # of words in the arrays 
                if (nonTag.Any())
                    offset = nonTag.Average(t => t.Split(" ").Length) + 1;

                // if 1 item is set, but not size
                if (ItemsSet == 1 && string.IsNullOrEmpty(Size))
                    offset += 1;

                // if name was inferred or is primary, then good confidence
                if (!string.IsNullOrEmpty(InferredName) || !string.IsNullOrEmpty(PrimaryName)) 
                {
                    if (Tags.Count > 1)
                        offset = -1.5;
                    else
                        offset = 0;
                }                    

                var value = (Tags.Count + offset) / (double)ItemsSet;
                return value;
            }
        }

        public void UpdateTags()
        {
            TagStringSplit tss = new TagStringSplit();
            Tags = tss.Split(OriginalText);
        }

        public int ItemsSet
        {
            get
            {
                return (string.IsNullOrEmpty(Unit) ? 0 : 1) +
                        (string.IsNullOrEmpty(PrimaryName) ? 0 : 1) +
                        (string.IsNullOrEmpty(Size) ? 0 : 1) +
                        (string.IsNullOrEmpty(Format) ? 0 : 1) +
                        (string.IsNullOrEmpty(Method) ? 0 : 1) +
                        (string.IsNullOrEmpty(Frequency) ? 0 : 1) +
                        (string.IsNullOrEmpty(Qualifier) ? 0 : 1);
            }
        }
        
        /// <summary>
        /// 6 or better has multiple items tagged
        /// </summary>
        public int TaggedScore 
        { 
            get 
            {
                return  (string.IsNullOrEmpty(Unit) ? 0 : 5) +
                        (string.IsNullOrEmpty(PrimaryName) ? 0 : 5) +                        
                        (string.IsNullOrEmpty(Frequency) ? 0 : 3) +
                        (string.IsNullOrEmpty(Method) ? 0 : 1) +
                        (string.IsNullOrEmpty(Size) ? 0 : 2);
            } 
        }
        public MedicationInfo()
        {
            Tags = new List<string>();
        }
    }
}
