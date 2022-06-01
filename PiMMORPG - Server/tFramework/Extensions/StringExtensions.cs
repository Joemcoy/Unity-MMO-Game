using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Extensions
{
    public static class StringExtensions
    {
        public static string Format(this string pattern, params object[] arguments)
        {
            return string.Format(pattern, arguments);
        }

        public static string Join(this string[] strings, string separator = "")
        {
            return string.Join(separator, strings);
        }

        public static string Repeat(this string target, uint n)
        {
            for (int i = 1; i < n; i++)
                target += target;
            return target;
        }
    }
}
