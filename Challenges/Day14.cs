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
            Assert.AreEqual(165L, Part1(testData));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(208L, Part2(testData));
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

        //public override object Part2(string input)
        //{
        //    var memory = new Dictionary<long, long>();

        //    foreach (var (mask, instructions) in ReadProgram(input))
        //    {
        //        foreach (var (addr, value) in instructions)
        //            foreach (var p in Permute(mask, addr))
        //                memory[p] = value;
        //    }

        //    return memory.Values
        //        .Sum();

        //    IEnumerable<int> Permute(string mask, long address)
        //    {

        //    }
        //}


        private static long ApplyMask(string mask, long input)
        {
            for (int i = mask.Length, j = 0; i-- > 0; j++)
            {
                var c = mask[i];
                switch (c)
                {
                    case 'X': break;
                    case '1': input |= 1L << j; break;
                    case '0': input &= ~(1L << j); break;
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

        private const string testData = @"
mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0";
    }
}
