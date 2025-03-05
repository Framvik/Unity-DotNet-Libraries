namespace Framvik.DotNetLib.Extensions
{
    /// <summary>
    /// A collection of string extensions relating to the string.Replace() method.
    /// </summary>
    public static class StringReplaceExtensions
    {
        /// <summary>
        /// Works like string.Replace() except it only replaces the last found instance.
        /// </summary>
        public static string ReplaceLast(this string source, string oldValue, string newValue)
        {
            int place = source.LastIndexOf(oldValue);

            if (place == -1)
                return source;

            return source.Remove(place, oldValue.Length).Insert(place, newValue);
        }
    }
}