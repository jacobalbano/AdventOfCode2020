using AdventOfCode2020.Common;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(3, "Toboggan Trajectory")]
    class Day03 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(7, Part1(testInput));
        }

        public override object Part1(string input)
        {
            var grid = new Grid<bool>(input, c => c == '#');
            return CountTreesForSlope(grid, 3, 1);
        }

        public override void Part2Test()
        {
            Assert.AreEqual(336, Part2(testInput));
        }

        public override object Part2(string input)
        {
            var grid = new Grid<bool>(input, c => c == '#');
            int total = 1;
            foreach (var (x, y) in new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) })
                total *= CountTreesForSlope(grid, x, y);

            return total;
        }

        private static int CountTreesForSlope(Grid<bool> grid, int byX, int byY)
        {
            int col = 0, row = 0, trees = 0;
            while (grid.IsValidPosition(col = (col + byX) % grid.Columns, row += byY))
                if (grid[row, col]) trees++;

            return trees;
        }

        private const string testInput = @"
..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
    }
}
