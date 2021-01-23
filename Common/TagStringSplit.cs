using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Helper class for splitting Tagged lines into arrays of tagged/non-tagged elements
    /// </summary>
    public class TagStringSplit 
    {
        /// <summary>
        /// only extract elements which have been tagged
        /// </summary>
        /// <param name="taggedString"></param>
        /// <returns></returns>
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

        /// extract both elements which have and have not been tagged
        public List<string> Split(string taggedString)
        {
            if (string.IsNullOrWhiteSpace(taggedString))
                return new List<string>();

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

        // ensure not a blank before adding to return list
        void addItem(List<string> values, string value)
        {
            if (!string.IsNullOrEmpty(value.Trim()) )
            {
                values.Add(value.Trim());
            }
        }
    }
}
