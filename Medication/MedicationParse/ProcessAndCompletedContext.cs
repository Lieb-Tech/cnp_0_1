using System.Collections.Generic;

namespace Medication.MedicationParse
{
    public class ProcessAndCompletedContext<T>
    {        
        public T InProcess { get; set; }
        
        public List<T> Completed { get; set; }

        public ProcessAndCompletedContext()
        {
            Completed = new();
        }
    }
}
