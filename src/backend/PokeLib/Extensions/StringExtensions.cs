using System.Collections.Generic;
using System.Linq;

namespace PokeLib
{
    public static class StringExtensions
    {
        public static bool ContainsAny(this string str, params string[] values)
        {
            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    if (str.Contains(value))
                        return true;
                }
            }

            return false;
        }

        public static int IndexOfMany(this string str, params string[] values)
        {
            List<int> indexes = new List<int>();

            if (!string.IsNullOrEmpty(str) || values.Length > 0)
            {
                foreach (string value in values)
                {
                    indexes.Add(str.IndexOf(value));
                }
            }

            if (indexes.Count <= 0)
                indexes.Add(-1);

            return indexes.OrderBy(x => x).FirstOrDefault();
        }

    }
}
