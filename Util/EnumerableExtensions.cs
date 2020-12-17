using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class EnumerableExtensions
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

    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> self, int chunkSize)
    {
        using var e = self.GetEnumerator();
        while (e.MoveNext())
            yield return Inner();

        IEnumerable<T> Inner()
        {
            for (int i = 0; i < chunkSize; i++)
            {
                yield return e.Current;
                e.MoveNext();
            }
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

    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
        foreach (var sequence in sequences)
        {
            var localSequence = sequence;
            result = result.SelectMany(
              _ => localSequence,
              (seq, item) => seq.Concat(new[] { item })
            );
        }
        return result;
    }
}
