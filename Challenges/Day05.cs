using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(5, "Binary Boarding")]
    class Day5: ChallengeBase
    {
        public override void Part1Test()
        {
            {
                var id = GetSeatPosition("FBFBBFFRLR", out var col, out var row);
                Assert.AreEqual(44, row);
                Assert.AreEqual(5, col);
                Assert.AreEqual(357, id);
            }

            {
                var id = GetSeatPosition("BFFFBBFRRR", out var col, out var row);
                Assert.AreEqual(70, row);
                Assert.AreEqual(7, col);
                Assert.AreEqual(567, id);
            }

            {
                var id = GetSeatPosition("FFFBBBFRRR", out var col, out var row);
                Assert.AreEqual(14, row);
                Assert.AreEqual(7, col);
                Assert.AreEqual(119, id);
            }

            {
                var id = GetSeatPosition("BBFFBBFRLL", out var col, out var row);
                Assert.AreEqual(102, row);
                Assert.AreEqual(4, col);
                Assert.AreEqual(820, id);
            }
        }

        public override object Part1(string input)
        {
            return input.ToLines()
                .Select(x => GetSeatPosition(x, out _, out _))
                .Max();
        }

        public override void Part2Test()
        {
            // no test cases!
        }

        public override object Part2(string input)
        {
            var ids = input.ToLines()
                .Select(x => GetSeatPosition(x, out _, out _))
                .OrderBy(x => x)
                .ToArray();

            for (int i = 1; i < ids.Length - 1; i++)
            {
                if (ids[i + 1] != ids[i] + 1)
                    return ids[i] + 1;
            }

            throw new UnreachableCodeException();
        }

        private static int GetSeatPosition(string pass, out int col, out int row)
        {
            row = BSP(pass[..7], 0..128);
            col = BSP(pass[7..], 0..8);
            return row * 8 + col;
        }

        private static int BSP(string span, Range range)
        {
            int halfRange = range.End.Value - range.Start.Value;
            foreach (var c in span)
            {
                halfRange /= 2;
                switch (c)
                {
                    case 'F': case 'L': range = range.Start..(range.End.Value - halfRange); break;
                    case 'B': case 'R': range = (range.Start.Value + halfRange)..range.End; break;
                    default: Assert.Unreachable(); break;
                }
            }

            return range.Start.Value;
        }
    }
}
