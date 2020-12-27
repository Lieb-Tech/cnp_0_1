using Medication.MedicationParse.ParseStrategies;
using Common;
using System.Collections.Generic;
using System.Linq;

namespace Medication.MedicationParse.ParseSpecifications
{
    public record SpecSetStrat(Specification<string> Spec,
        Specification<MedicationInfo> Set, 
        IProcessAndCompletedStrategy<MedicationInfo> Strat) { }
    public class TagRunner
    {
        private readonly List<SpecSetStrat> strategies;

        public TagRunner()
        {
            strategies = new List<SpecSetStrat>();
        }

        public void AddSpecificationStrategy(Specification<string> specification, Specification<MedicationInfo> set, IProcessAndCompletedStrategy<MedicationInfo> strategy)
        {
            strategies.Add(new SpecSetStrat(specification, set, strategy));
        }
        
        public ProcessAndCompletedContext<MedicationInfo> ProcessTag(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            if (tag.Contains("gen:li"))
                return context;

            foreach (var strategy in strategies)
            {
                if (!strategy.Spec.IsSatisfiedBy(tag))
                    continue;

                if (!strategy.Set.IsSatisfiedBy(context.InProcess))
                {
                    var nextInfo = new MedicationInfo();
                    (context.InProcess, nextInfo) = moveEnddingNonTaggedText(context.InProcess, nextInfo);
                    context.Completed.Add(context.InProcess);
                    context.InProcess = nextInfo;
                }
                
                strategy.Strat.Execute(context, tag);
                break;
            }

            context.InProcess.Tags.Add(tag);
            var newString = context.InProcess.OriginalText + " " + tag;
            context.InProcess = context.InProcess with { OriginalText = newString };

            return context;
        }

        internal (MedicationInfo, MedicationInfo ) moveEnddingNonTaggedText(MedicationInfo inProgress, MedicationInfo nextInfo)
        {            
            var idx = inProgress.OriginalText.LastIndexOf("}");            
            if (idx == -1 || idx == inProgress.OriginalText.Length - 1)
                return (inProgress, nextInfo);
            
            nextInfo.OriginalText = inProgress.OriginalText.Substring(idx + 1);
            nextInfo.Tags.Add(nextInfo.OriginalText);

            inProgress = inProgress with { OriginalText = inProgress.OriginalText.Substring(0, idx + 1) };
            return (inProgress, nextInfo);
        }
    }
}
