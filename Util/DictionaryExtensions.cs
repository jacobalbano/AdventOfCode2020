using System;
using System.Collections.Generic;
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
}