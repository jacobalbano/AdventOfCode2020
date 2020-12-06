using System;
using System.Collections.Generic;
using System.Text;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> PartitionBy<T>(this IEnumerable<T> self, Func<T, bool> delimit)
    {
        var e = self.GetEnumerator();
        while (e.MoveNext())
            yield return PartitionByInner(e, delimit);
    }

    private static IEnumerable<T> PartitionByInner<T>(IEnumerator<T> e, Func<T, bool> delimit)
    {
        do
        {
            if (delimit(e.Current))
                yield break;

            yield return e.Current;
        } while (e.MoveNext());
    }
}
