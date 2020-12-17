using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TupleExtensions
{
    public static void Deconstruct<T>(this T[] self, out T a, out T b)
    {
        a = self[0];
        b = self[1];
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c)
    {
        self.Deconstruct(out a, out b);
        c = self[2];
    }

    public static void Deconstruct<T>(this T[] self, out T a, out T b, out T c, out T d)
    {
        self.Deconstruct(out a, out b, out c);
        d = self[3];
    }
}