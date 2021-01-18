using System;

namespace Common.DecisionTree
{
    public class DecisionQuery<T> : Decision<T>
    {
        public string Label { get; set; }
        public Decision<T> Positive { get; set; }
        public Decision<T> Negative { get; set; }
        // Primitive operation to be provided by the user
        public Func<T, bool> Test { get; set; }

        public override void Evaluate(T info)
        {
            // Test a client using the primitive operation
            bool res = Test(info);   

            // Select a branch to follow
            if (res)
                Positive.Evaluate(info);
            else
                Negative.Evaluate(info);
        }
    }
}
