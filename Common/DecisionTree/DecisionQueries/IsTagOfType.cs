
namespace Common.DecisionTree.DecisionQueries
{
    public class IsTagOfType : DecisionQuery<ITaggedData>
    {
        public IsTagOfType(string tagType,
            int index, 
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

                if (d.Tags.Count < d.Index + index + 1)
                    return false;

                var test = d.Tags[d.Index + index].Contains(":" + tagType, System.StringComparison.InvariantCultureIgnoreCase);
                d.Matched = test;
                
                return test;
            };
        }
    }
}
