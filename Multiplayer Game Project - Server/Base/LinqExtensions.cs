using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtensions
    {
        public static void For(this int n, Action<int> Method)
        {
            for (int i = 0; i < n; i++)
                Method(i);
        }

        public static void For<T>(this T[] Array, Action<int> Method)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Method(i);
            }
        }

        public static void For<T>(this T[] Array, Action<T, int> Method)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Method(Array[i], i);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> Collection, Action<T> Method)
        {
            foreach (T Value in Collection)
            {
                Method(Value);
            }
        }
    }
}