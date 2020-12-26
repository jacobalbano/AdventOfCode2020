using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common
{
    public static class SumN
    {
        /// <summary>
        ///  assume input is sorted descending
        ///  find difference between head and target
        ///  solve for difference with FindSum2
        ///  if no solution, slice off head and recurse
        /// </summary>
        public static bool Find(Span<long> numbers, long target, ref long[] constituents)
        {
            return FindInner(numbers, target, ref constituents, constituents.Length);
        }

        private static bool FindInner(Span<long> numbers, long target, ref long[] constituents, int n)
        {
            if (numbers.IsEmpty)
                return false;
            else if (n == 2)
            {
                var a = constituents[0] = numbers[0];
                var b = constituents[1] = numbers[^1];
                if (a + b > target)
                    return FindInner(numbers[1..], target, ref constituents, n);
                else if (a + b < target)
                    return FindInner(numbers[..^1], target, ref constituents, n);
                else return true;
            }
            else
            {
                var first = constituents[n - 1] = numbers[0];
                if (FindInner(numbers[1..], target - first, ref constituents, n - 1))
                    return true;

                return FindInner(numbers[1..], target, ref constituents, n);
            }
        }
    }
}
