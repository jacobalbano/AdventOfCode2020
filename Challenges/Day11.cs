using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(11, "Seating System")]
    class Day11 : ChallengeBase
    {
        public enum State
        {
            Floor,
            Empty,
            Occupied
        }

        public override void Part1Test()
        {
            Assert.AreEqual(37, Part1(testData));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(26, Part2(testData));
        }

        public override object Part1(string input)
        {
            return Run(input, x => Simulate(x, x.GetSurroundingCount, 4));
        }

        public override object Part2(string input)
        {
            return Run(input, x => Simulate(x, x.GetLineOfSightCount, 5));
        }

        private static int Run(string input, Func<Grid<State>, Grid<State>> stepSimulation)
        {
            var grid = new Grid<State>(input, c => c switch
            {
                '.' => State.Floor,
                'L' => State.Empty,
                '#' => State.Occupied,
                _ => throw new UnreachableCodeException()
            });

            while (true)
            {
                var diffs = grid.FindDifferences(grid = stepSimulation(grid));
                if (!diffs.Any())
                    break;
            }

            return grid.Cells()
                .Select(x => grid[x.row, x.col])
                .Where(x => x == State.Occupied)
                .Count();
        }

        private static Grid<State> Simulate(Grid<State> original, Func<int, int, State, int> countSeats, int occupiedThreshold)
        {
            var clone = original.Clone();
            foreach (var (row, col) in original.Cells())
            {
                switch (original[row, col])
                {
                    case State.Empty:
                        if (original.GetSurroundingCount(row, col, State.Occupied) == 0)
                            clone[row, col] = State.Occupied;
                        break;

                    case State.Occupied:
                        if (original.GetSurroundingCount(row, col, State.Occupied) >= occupiedThreshold)
                            clone[row, col] = State.Empty;
                        break;
                    case State.Floor:
                        break;
                    default:
                        throw new UnreachableCodeException();
                }
            }

            return clone;
        }

        private const string testData = @"
L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL";
    }

    static class SeatingGridExtensions
    {
        public static int GetSurroundingCount(this Grid<Day11.State> self, int row, int col, Day11.State seek)
        {
            int total = 0;
            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                {
                    if (c == 0 && r == 0) continue;
                    var atCol = col + c;
                    var atRow = row + r;
                    if (!self.IsValidPosition(atRow, atCol)) continue;
                    if (self[atRow, atCol] == seek)
                        total++;
                }
            }
            return total;
        }

        public static int GetLineOfSightCount(this Grid<Day11.State> self, int row, int col, Day11.State seek)
        {
            int total = 0;
            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2; c++)
                {
                    if (c == 0 && r == 0) continue;
                    if (!self.CastRay(row, col, r, c, out var seat))
                        continue;

                    if (seat.Equals(seek))
                        total++;
                }
            }
            return total;
        }

        public static bool CastRay(this Grid<Day11.State> self, int rowStart, int colStart, int rowStep, int colStep, out Day11.State seat)
        {
            seat = default;
            while (self.IsValidPosition(rowStart += rowStep, colStart += colStep))
            {
                if (self[rowStart, colStart] != Day11.State.Floor)
                {
                    seat = self[rowStart, colStart];
                    return true;
                }
            }

            return false;
        }
    }
}
