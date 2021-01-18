namespace Common.DecisionTree.DecisionQueries
{
    public class TagHasValue : DecisionQuery<ITaggedData>
    {
        public TagHasValue(string value,
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

                var test = d.Tags[d.Index + index]
                    .TagValue()
                    .Equals(value.Trim(), System.StringComparison.InvariantCultureIgnoreCase);
                d.Matched = test;

                return test;
            };
        }
    }
}
