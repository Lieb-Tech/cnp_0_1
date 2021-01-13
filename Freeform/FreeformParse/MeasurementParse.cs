using Common;
using System.Collections.Generic;

namespace Freeform.FreeformParse
{
    public class MeasurementParse
    {
        private readonly List<SpecificationSetStrategy<MeasurementInfo>> tagRunner = new List<SpecificationSetStrategy<MeasurementInfo>>()
        {
            new SpecificationSetStrategy<MeasurementInfo>(
                new FreeformSpecification.Measurement.ConnectorSpecification(),
                new FreeformSetSpecification.Measurement.ConnectorSetSpecification(),
                new FreeformStrategies.Measurement.ConnectorStrategy()
            ),
            new SpecificationSetStrategy<MeasurementInfo>(
                new FreeformSpecification.Measurement.ValueSpecification(),
                new FreeformSetSpecification.Measurement.ValueSetSpecification(),
                new FreeformStrategies.Measurement.ValueStrategy()
            ),
            new SpecificationSetStrategy<MeasurementInfo>(
                new FreeformSpecification.Measurement.MeasurementSpecification(),
                new FreeformSetSpecification.Measurement.MeasurementSetSpecification(),
                new FreeformStrategies.Measurement.MeasurementStrategy()
            )
        };

        public void ProcessLine(TextSpan Line)
        {
            var tags = getTags(Line);
            
            var ctx = new ProcessAndCompletedContext<MeasurementInfo>();
            ctx.InProcess = new MeasurementInfo("", "", "", "");
            List<MeasurementInfo> processed = new List<MeasurementInfo>();
            foreach (var tag in tags)
            {
                foreach (var sss in tagRunner)
                {
                    if (!sss.specification.IsSatisfiedBy(tag))
                        continue;

                    if (sss.set.IsSatisfiedBy(ctx.InProcess))
                    {
                        processed.Add(ctx.InProcess);
                        ctx.InProcess = new MeasurementInfo("", "", "", "");
                    }

                    ctx = sss.strategy.Execute(ctx, tag);
                }
            }
            processed.Add(ctx.InProcess);
        }

        internal List<string> getTags(TextSpan Line)
        {
            TagStringSplit tss = new();
            return tss.Execute(Line.UpdatedText);
        }
        
    }    
}
/*        

{ gen: measure: Blood pressure} { med: num: 90 / 40} { gen: Connector: to} { med: num: 110 / 60}
{ gen: measure: Potassium} { med: num: 6.3} { gen: Connector: down } { gen: Connector: to} { med: num: 4.8} 
{ gen: measure: calcium} { med: num: 8.3} 
{ gen: measure: phosphorus} { med: num: 5}
{ gen: measure: BUN} { med: num: 35} 
{ gen: measure: creatinine} { med: num: 5.9} 
{ gen: measure: hematocrit} { med: num: 34.9}

*/