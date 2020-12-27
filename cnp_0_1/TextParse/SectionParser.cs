using Common;
using System.Collections.Generic;
using System.Linq;

namespace cnp_0_1.TextParser
{
    public class SectionParser
    {   
        public List<SectionTextInfo>  ParseText(string[] fullText)
        {
            var textLines = StripXML(fullText);
            var sections = new List<SectionTextInfo>();
            getSections(textLines, sections);

            return sections;
        }

        private void getSections(string[] fullText, List<SectionTextInfo> sections)
        {            
            var section = new SectionTextInfo();
            var spec = new SectionSpecification();

            int lineNumber = -1;
            foreach (var textLine in fullText)
            {
                lineNumber++;
                if (!spec.IsSatisfiedBy(textLine))
                {
                    section.Lines.Add(new SectionLine() { Original = textLine });
                }
                else
                {
                    sections.Add(section);
                    section = getNewSection(textLine, lineNumber);
                }
            }            
        }

        private SectionTextInfo getNewSection(string header, int line)
        {
            return new SectionTextInfo()
            {
                Header = header,
                HeaderLineNumber = line
            };
        }

        private string[] StripXML(string[] fullText)
        {
            var pts = new PlainTextSpecification();
            var gsr = new SpecificationRunner<string>();
            var text = new List<string>(fullText).AsQueryable();

            return gsr.Find(pts, text).ToArray();
        }
    }
}
