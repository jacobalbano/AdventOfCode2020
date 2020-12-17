using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(17, "Conway Cubes")]
    class Day17 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(112, Part1(testData));
        }

        public override object Part1(string input)
        {
            return new Conway<Coord3>(input, (x, y) => new Coord3 { X = x, Y = y })
                .Run(6)
                .Where(x => x.Value)
                .Count();
        }

        public override void Part2Test()
        {
            Assert.AreEqual(848, Part2(testData));
        }

        public override object Part2(string input)
        {
            return new Conway<Coord4>(input, (x, y) => new Coord4 { X = x, Y = y })
                .Run(6)
                .Where(x => x.Value)
                .Count();
        }

        private class Conway<TCoord>
            where TCoord : struct, Conway<TCoord>.ICoord
        {
            public interface ICoord
            {
                IEnumerable<TCoord> GetNeighbors();
            }

            public Conway(string input, Func<int, int, TCoord> parser)
            {
                var lines = input
                    .ToLines()
                    .ToArray();

                for (int row = 0; row < lines.Length; row++)
                    for (int col = 0; col < lines[row].Length; col++)
                        storage[parser(col, row)] = lines[row][col] == '#';
            }

            public IReadOnlyDictionary<TCoord, bool> Run(int times)
            {
                var grid = storage;
                for (int i = 0; i < times; i++)
                {
                    var clone = grid.Clone();
                    var active = grid
                        .Where(x => x.Value)
                        .Select(x => x.Key)
                        .ToList();

                    var inactive = active
                        .SelectMany(x => x.GetNeighbors())
                        .Where(x => !clone.Establish(x))
                        .Distinct()
                        .ToList();

                    foreach (var origin in active)
                    {
                        var count = origin.GetNeighbors()
                            .Where(x => grid.TryGetValue(x, out var b) && b)
                            .Count();

                        clone[origin] = count == 2 || count == 3;
                    }

                    foreach (var origin in inactive)
                    {
                        var count = origin.GetNeighbors()
                            .Where(x => grid.TryGetValue(x, out var b) && b)
                            .Count();

                        clone[origin] = count == 3;
                    }

                    grid = clone;
                }

                return grid;

            }

            private readonly Dictionary<TCoord, bool> storage = new Dictionary<TCoord, bool>();
        }

        private struct Coord3 : Conway<Coord3>.ICoord, IEquatable<Coord3>
        {
            public int X { get; init; }
            public int Y { get; init; }
            public int Z { get; init; }

            public IEnumerable<Coord3> GetNeighbors()
            {
                foreach (var (a, b, c) in offsets)
                    yield return new Coord3 { X = X + a, Y = Y + b, Z = Z + c };
            }

            public override bool Equals(object obj) => obj is Coord3 coord && Equals(coord);
            public bool Equals(Coord3 other) => X == other.X && Y == other.Y && Z == other.Z;
            public override int GetHashCode() => HashCode.Combine(X, Y, Z);

            private static readonly int[][] offsets = Enumerable.Repeat(new[] { -1, 0, 1 }, 3)
                .CartesianProduct()
                .Select(x => x.ToArray())
                .Where(x => !x.All(y => y == 0))
                .ToArray();
        }

        private struct Coord4 : Conway<Coord4>.ICoord, IEquatable<Coord4>
        {
            public int X { get; init; }
            public int Y { get; init; }
            public int Z { get; init; }
            public int W { get; init; }

            public IEnumerable<Coord4> GetNeighbors()
            {
                foreach (var (a, b, c, d) in offsets)
                    yield return new Coord4 { X = X + a, Y = Y + b, Z = Z + c, W = W + d };
            }

            public override bool Equals(object obj) => obj is Coord4 coord && Equals(coord);
            public bool Equals(Coord4 other) => X == other.X && Y == other.Y && Z == other.Z && W == other.W;
            public override int GetHashCode() => HashCode.Combine(X, Y, Z, W);

            private static readonly int[][] offsets = Enumerable.Repeat(new[] { -1, 0, 1 }, 4)
                .CartesianProduct()
                .Select(x => x.ToArray())
                .Where(x => !x.All(y => y == 0))
                .ToArray();
        }

        private const string testData = @"
.#.
..#
###";
    }
}
