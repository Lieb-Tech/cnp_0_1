using Common;
using System.Collections.Generic;
using System.Linq;
using Medication.MedicationParse.ParseStrategies;
using Medication.MedicationParse.ParseSpecifications;
using Medication.MedicationParse.ParseStrat2;

namespace Medication.MedicationParse
{
    public class MedicationParser
    {
        private readonly TagRunner tagRunner = new();

        private readonly IStrategy<MedicationInfo> postStrategy = new NameExtractionStrategy(new NameExtractionSpecification());
        private readonly IStrategy<MedicationInfo> postPostStrategy = new NameExtractionStrategy(new LastTagNameExtractionSpecification());

        public MedicationParser()
        {            
            tagRunner.AddSpecificationStrategy(new UnitSpecification(), new UnitSetSpecification(), new UnitStrategy());
            tagRunner.AddSpecificationStrategy(new FrequencySpecification(), new FrequencySetSpecification(), new FrequencyStrategy());
            tagRunner.AddSpecificationStrategy(new SizeSpecification(), new SizeSetSpecification(), new SizeStrategy());
            tagRunner.AddSpecificationStrategy(new NameSpecification(), new NameSetSpecification(), new NameStrategy());
            tagRunner.AddSpecificationStrategy(new MethodSpecification(), new MethodSetSpecification(), new MethodStrategy());
            tagRunner.AddSpecificationStrategy(new FormatSpecification(), new FormatSetSpecification(), new FormatStrategy());
            tagRunner.AddSpecificationStrategy(new QualSpecification(), new QualSetSpecification(), new QualStrategy());
            tagRunner.AddSpecificationStrategy(new InstructionSpecification(), new InstructionSetSpecification(), new InstructionStrategy());
            tagRunner.AddSpecificationStrategy(new SecondaryNameSpecification(), new SecondaryNameSetSpecification(), new SecondaryNameStrategy());
        }

        public MedicationParseTag ParseLine(TextSpan span)
        {
            var meds = processLineOfText(span);
            meds = moveExtraNameForward(meds);

            // update the tag array, incase moveForward made changes
            foreach (var m in meds)
            {
                m.UpdateTags();
            }

            // try to get untagged names
            meds = extractNames(meds);

            // if the last word is untagged, move to seperate med
            meds = processLastTag(meds);

            // update the tag array, incase moveForward made changes
            foreach (var m in meds)
            {
                m.UpdateTags();
            }

            return new MedicationParseTag(meds, span);
        }

        /// <summary>
        /// If contains a med:name Tag, that's not the 1st entry, then move it to next MedicationInfo
        /// Ex: name size unit name; remove the 2nd name and assign to either Info in list
        /// </summary>
        /// <param name="medications"></param>
        /// <returns></returns>
        internal List<MedicationInfo>  moveExtraNameForward(List<MedicationInfo> medications)
        {
            List<MedicationInfo> meds = new List<MedicationInfo>();

            // "extra" name 
            string name = "";
            
            foreach (var medication in medications)
            {
                var med = medication with { };
                // if there's an extra name from previous med, use it here
                if (name != "")
                {
                    med = updateNameFromPrevious(name, medication);
                    name = "";
                }

                (med, name) = moveMedicationNameForward(med);

                meds.Add(med);
            }

            // if after all infoes processed, there's an "extra" name, create an entry for it
            if (!string.IsNullOrEmpty(name))
                meds.Add(new MedicationInfo() { PrimaryName = name });

            return meds;
        }

        internal (MedicationInfo, string) moveMedicationNameForward(MedicationInfo medication)
        {
            if (!medication.Tags.Any())
                return (medication, "");

            var med = medication with { };

            // if there are entries w/o a tag (potentially a name)
            var anyNonTagged = med.Tags.Any(z => !z.Contains("{"));

            // get any names that isn't first Tag in list
            var medName = med.Tags
                .Where((z, i) => i > 0 && z.Contains("med:name:"))
                .FirstOrDefault();

            if (!anyNonTagged || medName == null)
                return (medication, "");

            // save name for next med
            var name = medName;

            med.OriginalText = med.OriginalText.Replace(medName, "");
            med.PrimaryName = null;
            med.Tags.Remove(medName);

            return (med, name);
        }


        /// <summary>
        /// update current medication with name from previous one
        /// </summary>
        /// <param name="name"></param>
        /// <param name="medInfo"></param>
        /// <returns></returns>
        private MedicationInfo updateNameFromPrevious(string name, MedicationInfo medInfo)
        {
            // save name to string
            var orig = $"{name} {medInfo.OriginalText}";

            // if first entry is a tag, put name before so next steps can infer it
            var tags = new List<string>(medInfo.Tags);
            tags.Insert(0, name);

            return medInfo with { OriginalText = orig, Tags = tags };
        }

        /// <summary>
        /// Try to get name on last entry 
        /// </summary>
        /// <param name="meds"></param>
        /// <returns></returns>
        private List<MedicationInfo> processLastTag(List<MedicationInfo> meds)
        {
            if (!meds.Any())
                return meds;

            // look at last tag
            var last = meds.Last();
            // if it already has a name, then skip processing
            if (!string.IsNullOrEmpty(last.InferredName) || !string.IsNullOrEmpty(last.PrimaryName))
                return meds;

            // look at last tag and check if it's a name
            var context = new StrategyContext<MedicationInfo>(last, true);
            context = postPostStrategy.Execute(context);
            
            // create updated version of medication list, with potentially new inferred name
            var updatedMeds = new List<MedicationInfo>(meds);
            // update with new item
            updatedMeds[updatedMeds.Count - 1] = context.Data;

            return updatedMeds;
        }

        /// <summary>
        ///  Try to get infered names from medications
        /// </summary>
        /// <param name="meds"></param>
        /// <returns></returns>
        private List<MedicationInfo> extractNames(List<MedicationInfo> meds)
        {
            List<MedicationInfo> updatedMeds = new List<MedicationInfo>();
            foreach (var med in meds)
            {
                // check if first part is untagged
                var context = new StrategyContext<MedicationInfo>(med, true);
                context = postStrategy.Execute(context);
                updatedMeds.Add(context.Data);
            }
            return updatedMeds;
        }

       
        /// <summary>
        /// Main raw line of text processing
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        internal List<MedicationInfo> processLineOfText(TextSpan span)
        {
            // split text into parts -- individual tagged items + span of untagged
            TagStringSplit tss = new();
            var tags = tss.Split(span.UpdatedText);

            // if no tagged in string, then just move on
            if (!tags.Any(z => z.Contains("med:")))
                return new List<MedicationInfo>();

            // break tags into MedicationInfo objects
            var meds = processTags(tags);

            return meds;
        }

        /// <summary>
        /// Loop through the tags in a string
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        internal List<MedicationInfo> processTags(List<string> tags)
        {
            InprocessAndCompleted<MedicationInfo> context = new() { InProcess = new MedicationInfo() };

            // pass on to TagRunnger to proces the tag
            foreach (var tag in tags) 
            { 
                context = tagRunner.ProcessTag(context, tag);
            }

            // save the results to
            context.Completed.Add(context.InProcess);
            return context.Completed;
        }
    }
}
