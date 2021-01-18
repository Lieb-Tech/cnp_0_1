﻿namespace Common.DecisionTree
{
    public class DecisionResult<T>: Decision<T>
    {
        public bool Result { get; set; }

        public override void Evaluate(T Client)
        {
            // nothing to do
        }
    }
}
