namespace Framvik.DotNetLib.Extensions
{
    /// <summary>
    /// A collection of string extensions relating to the string.EndsWith() method.
    /// </summary>
    public static class StringEndsWithExtensions
    {
        /// <summary>
        /// Works like string.EndsWith() method except it checks multiple values at once. 
        /// Returns true if string ends with any of given values.
        /// </summary>
        public static bool EndsWithAny(this string source, params string[] values)
        {
            bool found = false;
            foreach (var s in values)
            {
                if (source.EndsWith(s))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// Works like string.EndsWith() method except it checks multiple values at once.
        /// Returns true if string ends with any of given values. 
        /// Also checks for each values ToLower() and ToUpper() variants.
        /// </summary>
        public static bool EndsWithAnyCaseVariants(this string source, params string[] values)
        {
            bool found = false;
            foreach (var s in values)
            {
                if (source.EndsWith(s) || source.EndsWith(s.ToLower()) || source.EndsWith(s.ToUpper()))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
