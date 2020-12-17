using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(14, "Docking Data")]
    class Day14 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(165L, Part1(testData1));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(208L, Part2(testData2));
        }

        public override object Part1(string input)
        {
            var memory = new Dictionary<long, long>();

            foreach (var (mask, instructions) in ReadProgram(input))
            {
                foreach (var (addr, value) in instructions)
                    memory[addr] = ApplyMask(mask, value);
            }

            return memory.Values
                .Sum();
        }

        public override object Part2(string input)
        {
            var memory = new Dictionary<long, long>();

            foreach (var (mask, instructions) in ReadProgram(input))
            {
                foreach (var (addr, value) in instructions)
                    foreach (var p in Permute(mask, addr))
                        memory[p] = value;
            }

            return memory.Values
                .Sum();
        }

        static IReadOnlyList<long> Permute(string mask, long address)
        {
            var floatingBits = new Stack<int>();
            for (int i = mask.Length, j = 0; i-- > 0; ++j)
            {
                switch (mask[i])
                {
                    case '0': break;
                    case '1': address |= 1L << j; break;
                    case 'X': address &= ~(1L << j); floatingBits.Push(j); break;
                    default: throw new UnreachableCodeException();
                }
            }

            var possible = new List<long> { address };
            while (floatingBits.TryPop(out var bit))
            {
                for (int i = possible.Count; i --> 0;)
                    possible.Add(possible[i] | 1L << bit);
            }

            return possible;
        }

        private static long ApplyMask(string mask, long input)
        {
            for (int i = mask.Length, j = 0; i-- > 0; j++)
            {
                var c = mask[i];
                switch (c)
                {
                    case '0': input &= ~(1L << j); break;
                    case '1': input |= 1L << j; break;
                    case 'X': break;
                    default:
                        throw new UnreachableCodeException();
                }
            }

            return input;
        }

        private static IEnumerable<(string, (int, int)[])> ReadProgram(string input)
        {
            using var line = input.ToLines().GetEnumerator();
            bool canMove = line.MoveNext();

            while (canMove)
            {
                yield return (
                    line.Current["mask = ".Length..],
                    Instructions().ToArray()
                );
            }

            IEnumerable<(int, int)> Instructions()
            {
                Assert.IsTrue(canMove = line.MoveNext(), "Enumeration continues");
                while (canMove && !line.Current.StartsWith("ma"))
                {
                    var (addrRaw, valueRaw) = line.Current.Split(']');
                    yield return (int.Parse(addrRaw["mem[".Length..]), int.Parse(valueRaw[" =".Length..]));
                    canMove = line.MoveNext();
                }
            }
        }

        private const string testData1 = @"
mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";

        private const string testData2 = @"
mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1";
    }
}
