using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(6, "Custom Customs")]
    class Day6 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(11, SumDistinctAnswers(testInput));
        }

        public override object Part1(string input)
        {
            return SumDistinctAnswers(input);
        }

        public override void Part2Test()
        {
            Assert.AreEqual(6, SumUnanimousAnswers(testInput));
        }

        public override object Part2(string input)
        {
            return SumUnanimousAnswers(input);
        }

        private static int SumDistinctAnswers(string input)
        {
            return input.ToLines()
                .PartitionBy(string.IsNullOrWhiteSpace)
                .Select(x => x.SelectMany(y => y).Distinct().Count())
                .Sum();
        }

        private static int SumUnanimousAnswers(string input)
        {
            return input.ToLines()
                .PartitionBy(string.IsNullOrWhiteSpace)
                .Select(x => x.ToList())
                .Sum(group => group
                    .SelectMany(x => x)
                    .GroupBy(x => x)
                    .Where(x => x.Count() == group.Count)
                    .Count());
        }

        private const string testInput = @"
abc

a
b
c

ab
ac

a
a
a
a

b";
    }
}
