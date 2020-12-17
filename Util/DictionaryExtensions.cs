using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

public static class DictionaryExtensions
{
    public static TValue Establish<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, Func<TKey, TValue> factory)
    {
        if (!self.TryGetValue(key, out var value))
            self[key] = value = factory(key);

        return value;
    }

    public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> self)
    {
        var result = new Dictionary<TKey, TValue>(self.Count);
        foreach (var (key, val) in self)
            result[key] = val;
        return result;
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
            Expression create = null;

            var ctor = typeof(T).GetConstructor(Type.EmptyTypes);
            if (ctor != null)
                create = Expression.New(ctor);
            else if (typeof(T).IsValueType)
                create = Expression.Constant(default(T), typeof(T));
                
            return Expression.Lambda<Func<T>>(create).Compile();
        }
    }
}