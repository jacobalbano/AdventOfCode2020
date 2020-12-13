using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(13, "Shuttle Search")]
    class Day13 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(295, Part1(testInput));
        }

        public override object Part1(string input)
        {
            var lines = input.ToLines().ToArray();

            var currentTs = int.Parse(lines[0]);
            var (bus, wait) = lines[1].Split(',')
                .SelectWhere(x => (int.TryParse(x, out var y), y))
                .Select(x => (bus: x, wait : (currentTs % x) - x))
                .OrderBy(x => currentTs - x.wait)
                .First();

            return Math.Abs(wait) * bus;
        }

        private const string testInput = @"
939
7,13,x,x,59,x,31,19";
    }
}
