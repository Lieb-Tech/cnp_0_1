using System.Collections.Generic;

namespace Common
{
    public class TagStringSplit 
    {
        public List<string> SplitTagged(string taggedString)
        {
            List<string> values = new List<string>();
            string cache = "";
            bool inTag = false;
            for (int p = 0; p < taggedString.Length; p++)
            {
                if (taggedString[p] == '{')
                {
                    inTag = true;
                    if (!string.IsNullOrEmpty(cache))
                        addItem(values, cache);

                    cache = taggedString[p].ToString();
                }
                else if (taggedString[p] == '}')
                {
                    inTag = false;
                    cache += taggedString[p];
                    addItem(values, cache);
                    cache = "";
                }
                else if (inTag)
                {
                    cache += taggedString[p];
                }
            }
            addItem(values, cache);
            return values;
        }

        public List<string> Split(string taggedString)
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
