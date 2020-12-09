using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(9, "Encoding Error")]
    class Day9 : ChallengeBase
    {
        public override void Part1Test()
        {
            var numbers = testData.ToLines()
                .Select(long.Parse)
                .ToArray();

            Assert.AreEqual(127u, FindInvalidNumber(numbers, 0..5));
        }

        public override object Part1(string input)
        {
            var numbers = input.ToLines()
                .Select(long.Parse)
                .ToArray();

            return FindInvalidNumber(numbers, 0..25);
        }

        public override void Part2Test()
        {
            var numbers = testData.ToLines()
                .Select(long.Parse)
                .ToArray();

            Assert.AreEqual(127u, FindInvalidNumber(numbers, 0..5));
            Assert.IsTrue(FindEncryptionWeakness(numbers, 127u, out var min, out var max), "Failed to find encryption weakness");
            Assert.AreEqual(62u, min + max);
        }

        public override object Part2(string input)
        {
            var numbers = input.ToLines()
                .Select(long.Parse)
                .ToArray();

            var entryPoint = FindInvalidNumber(numbers, 0..25);
            FindEncryptionWeakness(numbers, entryPoint, out var min, out var max);
            return min + max;
        }

        private static long FindInvalidNumber(long[] numbers, Range search)
        {
            while (true)
            {
                var preamble = numbers[search]
                    .OrderByDescending(x => x)
                    .ToArray();

                if (!FindSum2(preamble, numbers[search.End], out _, out _))
                    return numbers[search.End];

                search = (search.Start.Value + 1)..(search.End.Value + 1);
            }
        }

        private bool FindEncryptionWeakness(Span<long> numbers, long entryPoint, out long min, out long max)
        {
            long total = min = max = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                var current = numbers[i];
                min = Math.Min(min, current);
                max = Math.Max(max, current);
                total += current;

                if (total == entryPoint)
                    return true;
                else if (total > entryPoint)
                    break;
            }

            return FindEncryptionWeakness(numbers[1..], entryPoint, out min, out max);
        }

        private static bool FindSum2(Span<long> numbers, long target, out long a, out long b)
        {
            a = b = default;
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

        private const string testData = @"
35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576";
    }
}
