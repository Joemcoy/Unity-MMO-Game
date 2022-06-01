using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace tFramework.Extensions
//{
public static class LinqExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> method)
    {
        foreach (var Object in enumerable)
            method(Object);
    }

    public static void For<T>(this IEnumerable<T> enumerable, Action<int, T> method)
    {
        var c = enumerable.Count();
        for (int i = 0; i < c; i++)
            method(i, enumerable.ElementAt(i));
    }

    public static void For(this int to, Action<int> method)
    {
        For(to, 0, method);
    }

    public static void For(this int to, int @from, Action<int> method)
    {
        for (int i = @from; i < to; i++)
            method(i);
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, T value)
    {
        int i = 0;
        foreach (T val in enumerable)
            if (val.Equals(value))
                return i;
            else
                i++;
        return -1;
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, Predicate<T> condition)
    {
        int i = 0;
        foreach (T val in enumerable)
            if (condition == null || condition(val))
                return i;
            else
                i++;
        return -1;
    }
}
//}