using Medication.MedicationParse.ParseSpecifications;
using Medication.MedicationParse.ParseStrategies;
using Common;
using System.Collections.Generic;
using System.Linq;
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
        }

        public MedicationParseTag ParseLine(TextSpan span)
        {
            var meds = extractMeds(span);
            moveExtraNameForward(meds);
            // meds = moveText(meds);

            foreach (var m in meds)
            {
                m.UpdateTags();
            }
            meds = extractNames(meds);

            meds = processLastTag(meds);

            return new MedicationParseTag(meds, span);
        }

        /// <summary>
        /// If contains a med:name Tag, that's not the 1st entry, then move it to next MedicationInfo
        /// Ex: name size unit name; remove the 2nd name and assign to either Info in list
        /// </summary>
        /// <param name="meds"></param>
        /// <returns></returns>
        internal void moveExtraNameForward(List<MedicationInfo> meds)
        {
            // "extra" name 
            string name = "";

            foreach (var m in meds)
            {
                // if there's an extra name from previous med, use it here
                if (name != "")
                {
                    m.Tags.Insert(0, name);
                    name = "";
                }

                if (!m.Tags.Any())
                    continue;

                // if there are entries w/o a tag (potentially a name)
                var anyNonTagged = m.Tags.Any(z => !z.Contains("{"));

                // get any names that isn't first Tag in list
                var last = m.Tags
                    .Where((z, i) => i > 0 && z.Contains("med:name:"))
                    .FirstOrDefault();

                if (!anyNonTagged || last == null)
                    continue;

                name = last;
                m.PrimaryName = null;
                m.Tags.Remove(last);                
            }

            // if after all infoes processed, there's an "extra" name, create an entry for it
            if (!string.IsNullOrEmpty(name))
                meds.Add(new MedicationInfo() { PrimaryName = name });
        }


        /// <summary>
        /// Try to get naem on last entry 
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
            
            updatedMeds.Remove(last);
            updatedMeds.Add(context.Data);

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
                var context = new StrategyContext<MedicationInfo>(med, true);
                context = postStrategy.Execute(context);
                updatedMeds.Add(context.Data);
            }
            return updatedMeds;
        }

        /*
        internal List<MedicationInfo> moveText(List<MedicationInfo> completed)
        {
            if (!completed.Any())
                return new List<MedicationInfo>();

            var newCompleted = new List<MedicationInfo>();
            
            for (int i = 0; i <= completed.Count - 2; i++)
            {
                var idx = completed[i].OriginalText.LastIndexOf("}");
                var current = completed[i];
                if (idx == -1 || idx == current.OriginalText.Length - 1)
                    newCompleted.Add(completed[i]);
                else
                {
                    var txt = current.OriginalText.Substring(idx + 1);
                    var updated = current with { OriginalText = current.OriginalText.Substring(0, idx + 1) };
                    newCompleted.Add(updated);

                    txt = txt + completed[i + 1].OriginalText;
                    completed[i + 1] = completed[i + 1] with { OriginalText = txt };
                }
            }

            newCompleted.Add(completed.Last());

            return newCompleted;
        }
        */
        internal List<MedicationInfo> extractMeds(TextSpan span)
        {
            TagStringSplit tss = new();
            var tags = tss.Execute(span.UpdatedText);

            if (!tags.Any(z => z.Contains("med:")))
                return new List<MedicationInfo>();

             var meds = processTags(tags);

            return meds;
        }

        internal List<MedicationInfo> processTags(List<string> tags)
        {
            ProcessAndCompletedContext<MedicationInfo> context = new() { InProcess = new MedicationInfo() };

            foreach (var tag in tags) 
            { 
                context = tagRunner.ProcessTag(context, tag);
            }

            context.Completed.Add(context.InProcess);
            return context.Completed;
        }
    }
}
