﻿
using Common;

namespace Medication.MedicationParse.ParseStrategies
{
    public class UnitStrategy : IProcessAndCompletedStrategy<MedicationInfo>
    {
        public ProcessAndCompletedContext<MedicationInfo> Execute(ProcessAndCompletedContext<MedicationInfo> context, string tag)
        {
            context.InProcess = context.InProcess with { Unit = tag.TagValue() };
            return context;
        }
    }
}
