﻿using Common;
using Common.Processing;
using System.Collections.Generic;

namespace Medication.MedicationTag
{
    /// <summary>
    /// Tag medications by their name
    /// Eventually would be database driven
    /// </summary>
    public class MedicationNameBuilder : IStrategy<TextSpan>
    {        
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            var names = new List<string>()
            {
                "Aspirin",
                "Atenolol",
                "Calcium acetate",
                "Celexa",
                "Lipitor"
            };

            var innerStrategy = new StringListStrategy(names, "med:name:");
            return innerStrategy.Execute(context);
        }        
    }
}