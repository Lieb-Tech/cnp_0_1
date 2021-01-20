using Common;
using System.Collections.Generic;

namespace Medication.MedicationParse.ParseSpecifications
{
    /// <summary>
    /// Process the string of text
    /// </summary>
    public class TagRunner
    {
        // processing strategies
        private readonly List<SpecificationSetStrategy<MedicationInfo>> strategies = new();              

        /// <summary>
        /// Process a tag value, setting the respective value in the Medication object
        /// </summary>
        /// <param name="specification">Should this tag be processed</param>
        /// <param name="set">Has this tag already been set in the current medication</param>
        /// <param name="strategy">How to process this tag</param>
        public void AddSpecificationStrategy(Specification<string> specification, Specification<MedicationInfo> set, IInprocessAndCompletedStrategy<MedicationInfo> strategy)
        {
            strategies.Add(new SpecificationSetStrategy<MedicationInfo>(specification, set, strategy));
        }
        
        /// <summary>
        /// Process the tag, returning updated medicated
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public InprocessAndCompleted<MedicationInfo> ProcessTag(InprocessAndCompleted<MedicationInfo> context, string tag)
        {
            // skip ListItem
            if (tag.Contains("gen:li") || tag.Contains("med:li"))
                return context;

            // find the matching strategy 
            foreach (var strategy in strategies)
            {
                // check if this entry is matching
                if (!strategy.specification.IsSatisfiedBy(tag))
                    continue;
                
                // check if current MedicationInfo has this tag already set
                if (!strategy.set.IsSatisfiedBy(context.InProcess))
                {
                    // if so, start a new one
                    var nextInfo = new MedicationInfo();
                    (context.InProcess, nextInfo) = moveEnddingNonTaggedText(context.InProcess, nextInfo);
                    context.Completed.Add(context.InProcess);
                    context.InProcess = nextInfo;
                }
                
                // now process it
                strategy.strategy.Execute(context, tag);
                break;
            }

            // save to tag array - for future processing & analysis
            context.InProcess.Tags.Add(tag);
            var newString = context.InProcess.OriginalText + " " + tag;
            context.InProcess = context.InProcess with { OriginalText = newString };

            return context;
        }

        /// <summary>
        /// When creating a new tag, if there's any untagged text that needs to be moved to new Info
        /// ex: {med:name} {med:size} {med:unit} asprin {med:size} -- the first info ends with "med:unit", move "today"
        /// to next tag, incase untagged is a name
        /// </summary>
        /// <param name="inProgress"></param>
        /// <param name="nextInfo"></param>
        /// <returns></returns>
        internal (MedicationInfo, MedicationInfo ) moveEnddingNonTaggedText(MedicationInfo inProgress, MedicationInfo nextInfo)
        {   
            // find close } of last tag
            var idx = inProgress.OriginalText.LastIndexOf("}");
            if (idx == -1 || idx == inProgress.OriginalText.Length - 1)
                return (inProgress, nextInfo);
            
            // move ending text from current, and move next medication 
            nextInfo.OriginalText = inProgress.OriginalText.Substring(idx + 1);
            nextInfo.Tags.Add(nextInfo.OriginalText);

            // keep up to last } in current
            inProgress = inProgress with { OriginalText = inProgress.OriginalText.Substring(0, idx + 1) };
            return (inProgress, nextInfo);
        }
    }
}
