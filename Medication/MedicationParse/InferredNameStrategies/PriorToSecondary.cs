using System.Collections.Generic;
using System.Linq;

namespace Medication.MedicationParse.InferredNameStrategies
{
    public class PriorToSecondary
    {
        public TagAndIndex GetTagPriorToSecondary(List<string> tags)
        {
            // get the first 
            var secondaryIdx = tags
                .Select((x, i) => new { tag = x, index = i })
                .Where(x => x.tag.Trim().ToLower().StartsWith("{med:secondary:"))
                .Select(x => x.index)
                .FirstOrDefault();

            // if 0 then it's either not found or the 1st element
            // either logic section assumes that name is before secondary
            if (secondaryIdx < 1)
                return null;

            // get the first 
            var untagged = tags
                .Where((x, i) => i < secondaryIdx && !x.Contains("{"))
                .Select((x, i) => new TagAndIndex(x, i))
                .LastOrDefault();

            return untagged;
        }
    }

    public record TagAndIndex(string Tag, int Index) { }
}
