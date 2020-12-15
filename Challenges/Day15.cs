using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(15, "Rambunctious Recitation")]
    class Day15 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(436, Part1("0,3,6"));
            Assert.AreEqual(1, Part1("1,3,2"));
            Assert.AreEqual(10, Part1("2,1,3"));
            Assert.AreEqual(27, Part1("1,2,3"));
            Assert.AreEqual(78, Part1("2,3,1"));
            Assert.AreEqual(438, Part1("3,2,1"));
            Assert.AreEqual(1836, Part1("3,1,2"));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(175594, Part2("0,3,6"));
            Assert.AreEqual(2578, Part2("1,3,2"));
            Assert.AreEqual(3544142, Part2("2,1,3"));
            Assert.AreEqual(261214, Part2("1,2,3"));
            Assert.AreEqual(6895259, Part2("2,3,1"));
            Assert.AreEqual(18, Part2("3,2,1"));
            Assert.AreEqual(362, Part2("3,1,2"));
        }

        public override object Part1(string input)
        {
            return MemoryGame(input)
                .Take(2020)
                .Last();
        }

        public override object Part2(string input)
        {
            return MemoryGame(input)
                .Take(30000000)
                .Last();
        }

        private static IEnumerable<int> MemoryGame(string input)
        {
            var startingDigits = input.CSV()
                .Select(int.Parse)
                .ToArray();

            int turn = 1, lastSpoken = 0;
            var memory = new Dictionary<int, (int, int)>();
            foreach (var num in startingDigits)
            {
                memory.Establish(lastSpoken = num, x => (turn, turn));
                yield return lastSpoken;
                turn++;
            }

            while (true)
            {
                var (prev, last) = memory[lastSpoken];
                lastSpoken = last - prev;
                if (!memory.TryGetValue(lastSpoken, out var turns))
                    memory[lastSpoken] = (turn, turn);
                else
                    memory[lastSpoken] = (turns.Item2, turn);

                yield return lastSpoken;
                turn++;
            }
        }
    }
}
