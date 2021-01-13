
namespace Common
{
    public class MedicationProcessor
    {
        private readonly MedicationParse.MedicationParser medParse = new ();
        private readonly MedicationTag.MedicationTagger medTagger = new ();

        public MedicationParseTag Process(string line )
        {            
            var tagged = medTagger.ProcessString(line);
            var result = medParse.ParseLine(tagged);
            return result;
        }
    }
}
