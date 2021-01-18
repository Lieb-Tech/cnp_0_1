using Common;
using Common.DecisionTree;
using System.Collections.Generic;

namespace Freeform
{
    public class Forest
    {
        public class DecisionData
        {
            public TextSpan textSpan { get; set; }
            public List<string> tags { get; set; } = new();
        }

        public Forest()
        {
            var tree = new DecisionQuery<DecisionData>();
            
        }
    }
}
