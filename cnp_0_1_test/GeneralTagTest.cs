using System.Collections.Generic;
using Xunit;

namespace cnp_0_1_test
{
    public class GeneralTagTest
    {
        private readonly List<string> lines = new List<string>()
        {
"Ms. First is an otherwise healthy 32 year old female attorney who was vacationing at Dacorpston when she fell off her moped at a speed of approximately 25 miles per hour.",
"She remembers the accident with no loss of consciousness.",
"She landed on her right knee and noted immediate pain and swelling.",
"She was taken by ambulance to Hasring Healthcare where she had plain films that revealed a comminuted bicondylar tibial plateau fracture on the right.",
"She was transferred to the Ver Medical Center for further evaluation and treatment.",
"Potassium 6.3 down to 4.8, calcium 8.3, phosphorus 5.",
"BUN 35, creatinine 5.9, hematocrit 34.9.",
"Blood pressure 90 / 40 to 110 / 60."

        };

        [Fact]
        public void MeasureExtracted()
        {
            var descParse = new Freeform.FreeformTag.PatternTagger();
            var info = descParse.ProcessLine(lines[5]);

            var gen = new Freeform.FreeformTag.GeneralTagger();
            info = gen.ProcessLine(info);

            var parse = new Freeform.FreeformParse.MeasurementTreeParse();
            parse.ProcessLine(info);

            Assert.NotNull(info);
        }
    }
}
