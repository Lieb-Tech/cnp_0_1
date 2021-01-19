
namespace Common
{
    /// <summary>
    /// Extension class to help prevent inccorect subtagging
    /// </summary>
    public static class InTagExtension
    {
        /// <summary>
        /// Determine if the position index provided is within a Tag
        /// </summary>
        /// <param name="text">String of text being examined</param>
        /// <param name="index">Position to check</param>
        /// <returns></returns>
        public static bool IsInTag(this string text, int index)
        {   
            // if not valid position
            if (index < 0)
                return false;

            // find { closest before index
            var openIdx = text.LastIndexOf("{", index);
            // find } closest before index
            var endIdx = text.LastIndexOf("}", index);

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
