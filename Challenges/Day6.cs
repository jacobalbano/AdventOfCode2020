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
                .Select(x => new { People = x.Count, Answers = x.SelectMany(y => y).GroupBy(y => y)})
                .Select(x => new { x.People, Answers = x.Answers.Where(y => y.Count() == x.People) })
                .Select(x => x.Answers.Count())
                .Sum();
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
