using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CA1050 // Declare types in namespaces
public static class IntExtensions
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static bool IsBetween(this int val, int min, int max)
    {
        return val >= min && val <= max;
    }

    public static bool IsBetween(this char val, char min, char max)
    {
        return val >= min && val <= max;
    }
}
