using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

#pragma warning disable CA1050 // Declare types in namespaces
public static class DictionaryExtensions
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static TValue Establish<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> factory)
    {
        if (!self.TryGetValue(key, out var value))
            self[key] = value = factory(key);

        return value;
    }

    public static TValue Establish<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        where TValue : new()
    {
        return Establish(self, key, x => DefaultFactory<TValue>.Create());
    }

    private static class DefaultFactory<T>
    {
        public static readonly Func<T> Create = MakeFactory();
        private static Func<T> MakeFactory()
        {
            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            return Expression.Lambda<Func<T>>(Expression.New(ctor)).Compile();
        }
    }
}