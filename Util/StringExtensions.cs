using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class StringExtensions
{
    public static string[] ToLines(this string input)
    {
        return input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    }

    public static string[] CSV(this string input)
    {
        return input.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .ToArray();
    }
}
