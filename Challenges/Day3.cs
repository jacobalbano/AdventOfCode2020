using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(3, "Toboggan Trajectory")]
    class Day3 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(7, Part1(testInput));
        }

        public override object Part1(string input)
        {
            var grid = new RepeatingGrid(input);
            return CountTreesForSlope(grid, 3, 1);
        }

        public override void Part2Test()
        {
            Assert.AreEqual(336, Part2(testInput));
        }

        public override object Part2(string input)
        {
            var grid = new RepeatingGrid(input);
            int total = 1;
            foreach (var (x, y) in new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) })
                total *= CountTreesForSlope(grid, x, y);

            return total;
        }

        private int CountTreesForSlope(RepeatingGrid grid, int byX, int byY)
        {
            int x = 0, y = 0, trees = 0;
            while (grid.IsValid(x += byX, y += byY))
                if (grid[x, y]) trees++;

            return trees;
        }

        private class RepeatingGrid
        {
            public int Height { get; }
            public int Stride { get; }

            public bool this[int col, int row]
            {
                get => storage[col % Stride, row];
                set => storage[col % Stride, row] = value;
            }

            public bool IsValid(int col, int row)
            {
                return col >= 0 && row >= 0 && row < Height;
            }

            public RepeatingGrid(string input)
            {
                var lines = input.ToLines();
                Stride = lines[0].Length;
                Height = lines.Length;
                storage = new bool[Stride, Height];

                for (int r = 0; r < Height; r++)
                {
                    var line = lines[r];
                    for (int c = 0; c < Stride; c++)
                        storage[c, r] = line[c] == '#';
                }
            }

            private bool[,] storage;
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
