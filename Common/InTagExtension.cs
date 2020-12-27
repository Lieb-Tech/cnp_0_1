
namespace Common
{
    public static class InTagExtension
    {
        public static bool IsInTag(this string text, int pos)
        {   
            // if not valid position
            if (pos < 0)
                return false;

            // find { closest before index
            var openIdx = text.LastIndexOf("{", pos);
            // find } closest before index
            var endIdx = text.LastIndexOf("}", pos);

            // if { not found, then not in tag
            if (openIdx == -1)
                return false;

            // close tag is after the open, then not in the middle of a tag
            if (endIdx > openIdx)
                return false;

            return true;
        }
    }
}
