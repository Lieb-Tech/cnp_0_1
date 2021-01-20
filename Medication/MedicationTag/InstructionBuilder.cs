﻿using Common;
using Common.Processing;
using System.Collections.Generic;

namespace Medication.MedicationTag
{
    /// <summary>
    /// tag instructions for taking medication 
    /// </summary>
    public class InstructionBuilder : IStrategy<TextSpan>
    {
        private readonly DictionaryReplaceStrategy dictionaryReplaceStrategy;
        public InstructionBuilder()
        {
            var listing = new Dictionary<string, string>()
            {
                { "with meals", "with meals" },
                { "with meal", "with meal" },
                { "with a meal", "with a meal" },
            };

            dictionaryReplaceStrategy = new DictionaryReplaceStrategy(listing, "med:instr:");
        }
        public StrategyContext<TextSpan> Execute(StrategyContext<TextSpan> context)
        {
            return dictionaryReplaceStrategy.Execute(context);
        }
    }
}
