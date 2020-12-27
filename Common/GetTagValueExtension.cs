namespace Common
{
    public static class TagValueExtension
    {
        public static string TagValue(this string text)
        {
            if (text == null)
                return null;

            int idx = text.LastIndexOf(":");
            if (idx == -1)
                return null;

            return text.Substring(idx + 1, text.Length - idx - 2);
        }
    }
}
