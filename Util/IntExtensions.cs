using System;
using System.Collections.Generic;
using System.Text;

public static class IntExtensions
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
