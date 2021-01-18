namespace Common.DecisionTree.DecisionQueries
{
    public class NumberOfTags : DecisionQuery<ITaggedData>
    {
        public NumberOfTags(int numTags, string label, Decision<ITaggedData> positve, Decision<ITaggedData> negative)
        {
            Positive = positve;
            Negative = negative;
            Label = label;

            Test = (d) =>
            {                
                var test = d.Tags.Count >= numTags;
                d.Matched = test;
                return test;
            };
        }
    }
}