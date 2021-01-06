
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

            meds.Add("Starting Today NEXIUM 20 MG PO QD ZETIA ( EZETIMIBE ) 10 MG PO QD ZOCOR ( SIMVASTATIN ) 40 MG PO QHS",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                         InferredName = "NEXIUM",                          
                          Size = "20",
                          Unit = "MG",
                          Frequency = "QD",
                          Format = "PO"
                    },
                    new MedicationInfo()
                    {
                         InferredName = "ZETIA",
                          SecondaryName = "EZETIMIBE",
                          Size = "10",
                          Unit = "MG",
                          Frequency = "QD",
                          Format = "PO"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "ZOCOR",
                          SecondaryName = "SIMVASTATIN",
                          Size = "40",
                          Unit = "MG",
                          Frequency = "QHS",
                          Format = "PO"
                    }
                });

            meds.Add("1. Aspirin",
                new List<MedicationInfo>()
            {
                new MedicationInfo()
                {
                     PrimaryName = "Aspirin",                      
                }
            });

            meds.Add("Starting Today NEXIUM ( ESOMEPRAZOLE ) 20 MG PO QD ZETIA ( EZETIMIBE ) 10 MG PO QD ZOCOR ( SIMVASTATIN ) 40 MG PO QHS",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                         InferredName = "NEXIUM",
                          SecondaryName = "ESOMEPRAZOLE",
                          Size = "20",
                          Unit = "MG",
                          Frequency = "QD",
                          Format = "PO"
                    },
                    new MedicationInfo()
                    {
                         InferredName = "ZETIA",
                          SecondaryName = "EZETIMIBE",
                          Size = "10",
                          Unit = "MG",
                          Frequency = "QD",
                          Format = "PO"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "ZOCOR",
                          SecondaryName = "SIMVASTATIN",
                          Size = "40",
                          Unit = "MG",
                          Frequency = "QHS",
                          Format = "PO"
                    }
                });

            meds.Add("1. Aspirin 81 mg every day .", 
                new List<MedicationInfo>()
            {
                new MedicationInfo()
                {
                     PrimaryName = "Aspirin",
                      Size = "81",
                       Unit = "mg",
                        Frequency = "every day"
                }
            });

            meds.Add("2. Amitriptyline 25 mg at bedtime.",
                new List<MedicationInfo>()
            {
                new MedicationInfo()
                {
                     InferredName = "Amitriptyline",
                      Size = "25",
                       Unit = "mg",
                        Frequency = "at bedtime"
                }
            });

            meds.Add("3.Atenolol 50 mg per day.", 
                new List<MedicationInfo>()
            {
                new MedicationInfo()
                {
                     InferredName = "Atenolol",
                      Size = "50",
                       Unit = "mg",
                        Frequency = "per day"
                }
            });

            meds.Add("Take with food DILAUDID ( HYDROMORPHONE HCL ) 2-4 MG PO Q4H PRN Pain TYLENOL ( ACETAMINOPHEN ) 650 MG PO Q4H PRN Pain",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                         InferredName = "DILAUDID",
                         SecondaryName = "HYDROMORPHONE HCL",
                         Size = "2-4",
                         Unit = "MG",
                         Frequency = "Q4H",
                         Qualifier = "PRN",
                    },
                    new MedicationInfo()
                    {
                         InferredName = "TYLENOL",
                         SecondaryName = "ACETAMINOPHEN",
                         Size = "650",
                         Unit = "MG",
                         Frequency = "Q4H",
                         Qualifier = "PRN",
                    },
                });

            meds.Add("ECASA ( ASPIRIN ENTERIC COATED ) 325 MG PO QD LOPRESSOR ( METOPROLOL TARTRATE ) 25 MG PO BID Starting Today ",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                        InferredName = "ECASA",
                        Size = "325",
                        Unit = "MG",
                        Format = "PO",
                        Frequency = "QD",
                        SecondaryName = "ASPIRIN ENTERIC COATED"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "LOPRESSOR",
                        Size = "25",
                        Unit = "MG",
                        Format = "PO",
                        Frequency = "BID",
                        SecondaryName = "METOPROLOL TARTRATE"
                    },
                });

            meds.Add("4 NEXIUM ( ESOMEPRAZOLE ) 20 MG PO QD LANTUS ( INSULIN GLARGINE ) 12 UNITS SC QHS METFORMIN 1,000",
                new List<MedicationInfo>()
                {
                    new MedicationInfo()
                    {
                        InferredName = "NEXIUM",
                        SecondaryName = "ESOMEPRAZOLE",
                        Size = "20",
                        Unit = "MG",
                        Format = "PO",
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
                        Format = "PO",
                        Frequency = "Q3H",
                        Unit = "MG",
                        Qualifier = "PRN"
                    },
                    new MedicationInfo()
                    {
                        InferredName = "CELEXA",
                        SecondaryName = "CITALOPRAM",
                        Size = "20",
                        Unit = "MG",
                        Format = "PO",
                        Frequency = "QD",
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
                    AssertX.NotNull(matched, extractedMed + " ---- " + extractedNme);
                }                
            }
        }

        [Fact]
        public void SizeTests()
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
                    Assert.Equal(matched.Size, extractedMed.Size);
                }
            }
        }

        [Fact]
        public void UnitTests()
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
                    Assert.Equal(matched.Unit, extractedMed.Unit);
                }
            }
        }

        [Fact]
        public void MethodTests()
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
                    Assert.Equal(matched.Method, extractedMed.Method);
                }
            }
        }

        [Fact]
        public void FrequencyTests()
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
                    Assert.Equal(matched.Frequency, extractedMed.Frequency);
                }
            }
        }
    }
}

