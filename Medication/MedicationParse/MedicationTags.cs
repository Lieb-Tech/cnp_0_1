namespace Medication.MedicationParse
{
    public enum MedicationTags
    {
        /// <summary>
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> Folate
        /// </summary>
        PrimaryName,
        /// <summary>
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> ( Folic Acid )
        /// </summary>
        SecondaryName,
        /// <summary>
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD -> 1
        /// </summary>
        SingleDosageSize,
        /// <summary>
        /// PERCOCET 1-2 TAB PO Q4H PRN pain -> 1
        /// </summary>
        RangeDosageSize1,
        /// <summary>
        /// PERCOCET 1-2 TAB PO Q4H PRN pain -> 2
        /// </summary>
        RangeDosageSize2,

        /// <summary>
        /// PERCOCET 1-2 TAB PO Q4H PRN pain -> TAB
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> MG
        /// </summary>
        DosageUnit,
        /// <summary>
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> PO
        /// </summary>
        Method,
        /// <summary>
        /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> QD
        /// </summary>
        SingleDosageFrequency,
        /// <summary>
        /// PERCOCET 1-2 TAB PO q.4-6h. PRN pain -> 4
        /// </summary>
        RangeDosageFrequency1,
        /// <summary>
        /// PERCOCET 1-2 TAB PO q.4-6h. PRN pain -> 6
        /// </summary>
        RangeDosageFrequency2,
        /// <summary>
        /// PERCOCET 1-2 TAB PO q.4-6h. PRN pain -> h
        /// /// FOLATE ( FOLIC ACID ) 1 MG PO QD  -> QD
        /// </summary>
        RangeDosageFrequencyType,
        /// <summary>
        /// Dicloxacillin 500 mg p.o. q.i.d. for ten days -> ten days
        /// </summary>
        Duration,
    }
}
