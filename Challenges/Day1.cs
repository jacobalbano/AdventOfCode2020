using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(1, "Report Repair")]
    class Day1 : ChallengeBase
    {
        private const string testData = @"1721
979
366
299
675
1456";
        
        public override object Part1(string input)
        {
            var numbers = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .OrderByDescending(x => x)
                .ToArray();

            if (!FindSum2(numbers, 2020, out var a, out var b))
                throw new Exception("Panic! Unable to solve");

            return a * b;
        }

        public override void Part1Test()
        {
            Assert("Part1 should match example outcome", Part1(testData).Equals(514579));
        }

        public override void Part2Test()
        {
            Assert("Part2 should match example outcome", Part2(testData).Equals(241861950));
        }

        public override object Part2(string input)
        {
            var numbers = input.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .OrderByDescending(x => x)
                .ToArray();

            if (!FindSum3(numbers, 2020, out var a, out var b, out var c))
                throw new Exception("Panic! Unable to solve");

            return a * b * c;
        }

        /// <summary>
        ///  assume input is sorted descending
        ///  sum the two numbers at either end
        ///  if sum is larger than target, slice off head and recurse
        ///  if sum is smaller than target, slice off tail and recurse
        /// </summary>
        private bool FindSum2(Span<int> numbers, int target, out int a, out int b)
        {
            a = b = -1;
            if (numbers.IsEmpty)
                return false;

            a = numbers[0];
            b = numbers[^1];
            if (a + b > target)
                return FindSum2(numbers[1..], target, out a, out b);
            else if (a + b < target)
                return FindSum2(numbers[..^1], target, out a, out b);

            return true;
        }

        /// <summary>
        ///  assume input is sorted descending
        ///  find difference between head and target
        ///  solve for difference with FindSum2
        ///  if no solution, slice off head and recurse
        /// </summary>
        private bool FindSum3(Span<int> numbers, int target, out int a, out int b, out int c)
        {
            a = b = c = -1;
            if (numbers.IsEmpty)
                return false;

            c = numbers[0];
            if (FindSum2(numbers[1..], target - c, out a, out b))
                return true;
            
            return FindSum3(numbers[1..], target, out a, out b, out c);
        }
    }
}
