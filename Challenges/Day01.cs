using AdventOfCode2020.Common;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(1, "Report Repair")]
    class Day01 : ChallengeBase
    {
        private const string testData = @"1721
979
366
299
675
1456";
        
        public override object Part1(string input)
        {
            var numbers = input.ToLines()
                .Select(long.Parse)
                .OrderByDescending(x => x)
                .ToArray();

            var parts = new long[2];
            if (!SumN.Find(numbers, 2020, ref parts))
                throw new Exception("Panic! Unable to solve");

            return parts.Aggregate((x, y) => x * y);
        }

        public override void Part1Test()
        {
            Assert.AreEqual(Part1(testData), 514579L);
        }

        public override object Part2(string input)
        {
            var numbers = input.ToLines()
                .Select(long.Parse)
                .OrderByDescending(x => x)
                .ToArray();

            var parts = new long[3];
            if (!SumN.Find(numbers, 2020, ref parts))
                throw new Exception("Panic! Unable to solve");

            return parts.Aggregate((x, y) => x * y);
        }

        public override void Part2Test()
        {
            Assert.AreEqual(Part2(testData), 241861950L);
        }
    }
}
