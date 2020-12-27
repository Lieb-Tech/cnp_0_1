
using Xunit;

namespace cnp_0_1_test
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(1, "1. Aspirin 81 mg every day .")]
        [InlineData(1, "2. Amitriptyline 25 mg at bedtime.")]
        [InlineData(1, "3. Atenolol 50 mg per day.")]
        [InlineData(1, "4. Lipitor 10 mg per day.")]
        [InlineData(1, "5. Calcium acetate three tablets three times a day with meals.")]
        [InlineData(1, "6. Celexa 40 mg per day.")]
        [InlineData(1, "7. Nexium 20 mg per day.")]
        [InlineData(1, "8. Mirapex 0.5 mg pre - dialysis.")]
        [InlineData(1, "9. Quinine 325 mg per day.")]
        [InlineData(1, "10. Renagel 800 mg four times per day.")]
        [InlineData(1, "Meclizine 25 mg po bid .")]
        [InlineData(4, "Those on admission as well as Coumadin 1 mg p.o. q.h.s. or as directed by PT draws which will occur q.Monday and Thursday at home , also Percocet one to two tablets q.4-6h. p.r.n. , Colace 100 mg p.o. t.i.d. , and Niferex 150 mg p.o. b.i.d.")]
        [InlineData(1, "LOPRESSOR (METOPROLOL TARTRATE) 25 MG PO BID")]
        [InlineData(1, "PROSCAR ( FINASTERIDE ) 5 MG PO QD")]
        [InlineData(1, "LEVOFLOXACIN 250 MG PO QD")]
        [InlineData(2, "FLOMAX( TAMSULOSIN ) 0.4 MG PO QD BACITRACIN TOPICAL TP BID")]
        /// CARRINGTON PERINEAL CLEANSER 1 TOPICAL TP BID
        /// CARRINGTON HYDROGEL 1 TOPICAL TP BID
        /// CARRINGTON ANTIFUNGAL CREAM 1 TOPICAL TP BID
        
        public void Test1(int num, string input)
        {            
            var medParse = new Medication.MedicationProcessor();
            var result = medParse.Process(input);

            Assert.Equal(num, result.medications.Count);
            Assert.True(result.medications[0].Confidence < 4.999);
            Assert.True(!string.IsNullOrEmpty(result.medications[0].InferredName)
                || !string.IsNullOrEmpty(result.medications[0].PrimaryName));
        }
        [Fact]

        public void Test2()
        {
            var medParse = new Medication.MedicationProcessor();
            string input = "FLOMAX( TAMSULOSIN ) 0.4 MG PO QD BACITRACIN TOPICAL TP BID";
            var result = medParse.Process(input);

            Assert.Equal(2, result.medications.Count);
            Assert.True(result.medications[0].Confidence < 4.999);
            Assert.True(!string.IsNullOrEmpty(result.medications[0].InferredName) 
                || !string.IsNullOrEmpty(result.medications[0].PrimaryName));
        }

        
        [InlineData("FINASTERIDE", "PROSCAR ( FINASTERIDE ) 5 MG PO QD")]
        [InlineData("METOPROLOL TARTRATE", "LOPRESSOR (METOPROLOL TARTRATE) 25 MG PO BID")]
        [Theory]
        public void Test3(string extracted, string input)
        {
            var medParse = new Medication.MedicationProcessor();
            var result = medParse.Process(input);
            
            Assert.Equal(extracted, result.medications[0].SecondaryName);
        }

        [Fact]
        public void Test4()
        {            
            var medParse = new Medication.MedicationProcessor();

            string input = "1. Aspirin 81 mg every day .";
            var result = medParse.Process(input);

            Assert.Contains("Aspirin" , result.medications[0].PrimaryName);

            input = "1. ASPIRIN 81 mg every day .";
            result = medParse.Process(input);

            Assert.Contains("Aspirin", result.medications[0].PrimaryName);
        }
    }
}
