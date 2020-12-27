
using Medication.MedicationParse;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace cnp_0_1_test
{
    public class MedicationTagTests
    {
        private static readonly Dictionary<string, List<MedicationInfo>> meds = new Dictionary<string, List<MedicationInfo>>();

        private readonly List<string> sampleMeds;
        public MedicationTagTests()
        {
            sampleMeds = new List<string>(File.ReadAllLines(@"C:\\NLP\\n2c2\\med.txt"));
            if (meds.Any())
                return;

            meds.Add("4 NEXIUM ( ESOMEPRAZOLE ) 20 MG PO QD LANTUS ( INSULIN GLARGINE ) 12 UNITS SC QHS METFORMIN 1,000",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                        InferredName = "NEXIUM",
                        SecondaryName = "ESOMEPRAZOLE",
                        Size = "20",
                        Unit = "MG",
                        Method = "PO",
                        Frequency = "QD"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "LANTUS",
                        SecondaryName = "INSULIN GLARGINE",
                        Size = "12",
                        Unit = "UNITS",
                        Method = "SC",
                        Frequency = "QHS"
                    },
                     new MedicationInfo()
                     {
                         InferredName = "METFORMIN",
                         Size ="1,000"
                     }
                });
            meds.Add("OXYCODONE 5-10 MG PO Q3H Starting when TOLERATING FOOD PRN Pain CELEXA ( CITALOPRAM ) 20 MG PO QD",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                        InferredName = "OXYCODONE",
                        Size = "5-10",
                        Method = "PO",
                        Frequency = "Q3H",
                        Qualifier = "PRN"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "CELEXA",
                        SecondaryName = "CITALOPRAM",
                        Size = "20",
                        Unit = "MG",
                        Method = "PO",
                        Frequency = "Q3H",
                        Qualifier = "PRN"
                    }
            });
        }

        [Fact]
        public void MedsExtracted()
        {
            var medParse = new Medication.MedicationProcessor();

            foreach (var med in meds)
            {
                var result = medParse.Process(med.Key);
                Debug.WriteLine(med.Key);
                AssertX.Equal(med.Value.Count, result.medications.Count, med.Key);
            }
        }

        [Fact]
        public void NameTests()
        {
            var medParse = new Medication.MedicationProcessor();

            foreach (var med in meds)
            {
                var result = medParse.Process(med.Key);
                
                foreach (var extractedMed in result.medications)
                {
                    string extractedNme = (extractedMed.PrimaryName ?? "") + (extractedMed.InferredName ?? "");
                    var matched = med
                        .Value
                        .FirstOrDefault(z => z.PrimaryName?.ToLower() == extractedNme.ToLower()  
                                        || z.InferredName?.ToLower() == extractedNme.ToLower());
                    AssertX.NotNull(matched, extractedNme);
                }                
            }
        }
    }
}

