using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(12, "Rain Risk")]
    class Day12 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(25, Part1(testData));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(286, Part2(testData));
        }

        public override object Part1(string input)
        {
            int x = 0, y = 0, rot = 0;
            var dirs = new[] { (1, 0), (0, 1), (-1, 0), (0, -1) };
            foreach (var line in input.ToLines())
            {
                var param = int.Parse(line[1..]);
                switch (line[0])
                {
                    case 'N': y -= param; break;
                    case 'S': y += param; break;
                    case 'E': x += param; break;
                    case 'W': x -= param; break;
                    case 'L': rot = (rot - param) % 360; break;
                    case 'R': rot = (rot + param) % 360; break;
                    case 'F':
                        if (rot < 0) rot += 360;
                        var (scaleX, scaleY) = dirs[rot / 90];
                        x += scaleX * param;
                        y += scaleY * param;
                        break;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }

        public override object Part2(string input)
        {
            int shipX = 0, shipY = 0, wpX = 10, wpY = -1;
            foreach (var line in input.ToLines())
            {
                var param = int.Parse(line[1..]);
                switch (line[0])
                {
                    case 'N': wpY -= param; break;
                    case 'S': wpY += param; break;
                    case 'E': wpX += param; break;
                    case 'W': wpX -= param; break;
                    case 'L': Repeat(param / 90, () => (wpX, wpY) = (wpY, -wpX)); break;
                    case 'R': Repeat(param / 90, () => (wpX, wpY) = (-wpY, wpX)); break;
                    case 'F':
                        shipX += wpX * param;
                        shipY += wpY * param;
                        break;
                }
            }

            return Math.Abs(shipX) + Math.Abs(shipY);
        }

        private static void Repeat(int times, Action action)
        {
            for (int i = 0; i < times; i++)
                action();
        }

        private const string testData = @"
F10
N3
F7
R90
F11";
    }
}
