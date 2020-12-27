using System.Text.RegularExpressions;

namespace Common.Processing
{
    public class RegexReplacer
    {
        public string tagPattern(Regex regex, string updatedText, string tag)
        {
            // since am replacing matches with modified + original, can't use .Replace
            // since updatedText is being modified each time, re-evaluate
            var hasMatch = regex.IsMatch(updatedText);
            int lastMatchPos = 0;
            string tagged;
            while (hasMatch)
            {               
                // get the info
                var match = regex.Match(updatedText, lastMatchPos);
                if (!updatedText.IsInTag(match.Index))
                {
                    // create replacement string
                    tagged = $" {{{tag}{match.Value.Trim()}}} ";

                    // update index to continue past this update
                    lastMatchPos = match.Index + tagged.Length;
                    // do the update
                    updatedText = replaceText(updatedText, match, tagged);
                }
                else
                    lastMatchPos = match.Index + match.Value.Length;

                if (lastMatchPos < updatedText.Length)
                    // check if more to do
                    hasMatch = regex.IsMatch(updatedText, lastMatchPos);
                else
                    hasMatch = false;
            }

            return updatedText;
        }
        public string replaceValue(string regexPattern, string textToSearch, string newValue)
        {
            return replaceValue(new Regex(regexPattern), textToSearch, newValue);
        }

        public string replaceValue(Regex regex, string textToSearch, string newValue)
        {
            // since am replacing matches with modified + original, can't use .Replace
            // since updatedText is being modified each time, re-evaluate
            var hasMatch = regex.IsMatch(textToSearch);
            int lastMatchPos = 0;
            while (hasMatch)
            {
                // get the info
                var match = regex.Match(textToSearch, lastMatchPos);
                
                // update index to continue past this update
                lastMatchPos = match.Index + newValue.Length;

                // do the update
                textToSearch = replaceText(textToSearch, match, newValue);

                if (lastMatchPos < textToSearch.Length)
                    // check if more to do
                    hasMatch = regex.IsMatch(textToSearch, lastMatchPos);
                else
                    hasMatch = false;
            }

            return textToSearch;
        }

        public string replaceText(string updatedText, Match match, string value)
        {
            string newText = updatedText;
            // skip if already tagged
            if (!updatedText.IsInTag(match.Index))
            {
                // good to go, so replace the text
                newText = updatedText.Substring(0, match.Index) +
                    value +
                    updatedText.Substring(match.Index + match.Length);
            }

            return newText;
        }
    }
}
