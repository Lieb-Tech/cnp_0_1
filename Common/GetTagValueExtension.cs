namespace Common
{
    /// <summary>
    /// Extension class for extracting the value of a tag
    /// </summary>
    public static class TagValueExtension
    {
        /// <summary>
        /// Get the original text that was tagged
        /// Ex: {med:num:5} -- return 5
        /// </summary>
        /// <param name="text">The tag</param>
        /// <returns></returns>
        public static string TagValue(this string text)
        {
            if (text == null)
                return null;

            if (!text.Contains("{"))
                return null;

            int idx = text.LastIndexOf(":");
            if (idx == -1)
                return null;

            return text.Substring(idx + 1, text.Length - idx - 2);
        }
    }
}
