using Common;
using System;
using System.Linq.Expressions;

namespace Common.MedicationParse.ParseSpecifications
{
    public class LastTagNameExtractionSpecification : Specification<MedicationInfo>
    {
        public override Expression<Func<MedicationInfo, bool>> ToExpression()
        {
            return data => string.IsNullOrEmpty(data.PrimaryName) 
                    || string.IsNullOrEmpty(data.InferredName);
            
        }
    }
}
