using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1050 // Declare types in namespaces
public static class StringExtensions
#pragma warning restore CA1050 // Declare types in namespaces
{
    public static IEnumerable<string> ToLines(this string input)
    {
        using StringReader sr = new StringReader(input);
        string line = sr.ReadLine(), nextLine = null;
        if (string.IsNullOrEmpty(line))
            nextLine = sr.ReadLine();
        else nextLine = line;

        while ((line = sr.ReadLine()) != null)
        {
            yield return nextLine;
            nextLine = line;
        }

        if (!string.IsNullOrEmpty(nextLine))
            yield return nextLine;
    }

    public static string[] CSV(this string input)
    {
        return input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();
    }

    public static void Deconstruct(this string[] input, out string a, out string b)
    {
        a = input[0];
        b = input[1];
    }
}
