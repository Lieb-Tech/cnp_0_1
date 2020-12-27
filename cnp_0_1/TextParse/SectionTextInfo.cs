using Common;
using System.Collections.Generic;

namespace cnp_0_1.TextParser
{
    public class SectionTextInfo
    {
        public string Header { get; set; }

        public int HeaderLineNumber { get; set; }
        
        public List<SectionLine> Lines { get; set; }

        public SectionTextInfo()
        {
            Lines = new List<SectionLine>();
        }
    }

    public class SectionLine
    {
        public string Original { get; set; }

        public TextSpan Replacements { get; set; }
    }
}
