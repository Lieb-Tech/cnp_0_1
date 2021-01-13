using System.Collections.Generic;

namespace Common
{
    public class TagStringSplit 
    {
        public List<string> Execute(string taggedString)
        {
            List<string> values = new List<string>();           
            string cache = "";
            for (int p = 0; p < taggedString.Length; p++)
            {                
                if (taggedString[p] == '{')
                {
                    if (!string.IsNullOrEmpty(cache ))
                        addItem(values, cache);

                    cache = taggedString[p].ToString();
                }
                else if (taggedString[p] == '}')
                {
                    cache += taggedString[p];
                    addItem(values, cache);
                    cache = "";
                }
                else
                {
                    cache += taggedString[p];
                }
            }
            addItem(values, cache);
            return values;
        }

        void addItem(List<string> values, string value)
        {
            if (!string.IsNullOrEmpty(value.Trim()) )
            {
                values.Add(value.Trim());
            }
        }
    }
}
