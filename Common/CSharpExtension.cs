using System.Collections;

namespace FinchUtils.Common
{
    public static class CollectionsExtension
    {
        public static bool IsNullOrEmpty<T>(this ICollection? collection)
        {
            return collection == null || collection.Count == 0;
        }
    }

    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}