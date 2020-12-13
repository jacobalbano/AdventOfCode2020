using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CA1050 // Declare types in namespaces
public static class EnumerableExtensions
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static IEnumerable<IEnumerable<T>> PartitionBy<T>(this IEnumerable<T> self, Func<T, bool> delimit)
    {
        using var e = self.GetEnumerator();
        while (e.MoveNext())
            yield return Inner();

        IEnumerable<T> Inner()
        {
            do
            {
                if (delimit(e.Current))
                    yield break;

                yield return e.Current;
            } while (e.MoveNext());
        }
    }

    public static IEnumerable<T> As<T>(this IEnumerable untyped)
    {
        foreach (var x in untyped)
            yield return (T)x;
    }

    public delegate (bool, TOut) SelectWhereSelector<TIn, TOut>(TIn input);

    public static IEnumerable<TOut> SelectWhere<TIn, TOut>(this IEnumerable<TIn> self, SelectWhereSelector<TIn, TOut> selector)
    {
        foreach (var x in self)
        {
            var (success, result) = selector(x);
            if (success) yield return result;
        }
    }
}
