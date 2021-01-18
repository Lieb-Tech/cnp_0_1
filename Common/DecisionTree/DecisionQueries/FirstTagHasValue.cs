using System;
using System.Linq;

namespace Common.DecisionTree.DecisionQueries
{
    public class FirstTagHasValue : DecisionQuery<ITaggedData>
    {
        public FirstTagHasValue(string tagType,
            string label,
            Decision<ITaggedData> positve,
            Decision<ITaggedData> negative)
        {
            Positive = positve;
            Negative = negative;
            Label = label;

            Test = (d) =>
            {
                d.Matched = false;

                var idx = d.Tags
                .Select((x, i) => new { x, i })
                .FirstOrDefault(o => o.x.TagValue().Contains(tagType, StringComparison.InvariantCultureIgnoreCase));

                if (idx == null)
                    return false;

                d.Index = idx.i;
                d.Matched = true;                
                return true;
            };
        }
    }
}
