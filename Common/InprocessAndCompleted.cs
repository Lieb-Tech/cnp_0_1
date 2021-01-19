using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// stores currently being process item, along with an array of completed items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InprocessAndCompleted<T>
    {        
        public T InProcess { get; set; }
        
        public List<T> Completed { get; set; }

        public InprocessAndCompleted()
        {
            Completed = new();
        }
    }
}
