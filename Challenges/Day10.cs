using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(10, "Adapter Array")]
    class Day10 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(7 * 5, Part1(test1));
            Assert.AreEqual(22 * 10, Part1(test2));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(8L, Part2(test1));
            Assert.AreEqual(19208L, Part2(test2));
        }

        public override object Part1(string input)
        {
            int diff1 = 0, diff3 = -1;
            foreach (var seqLength in OneDiffSequences(input))
            {
                diff1 += seqLength - 1;
                diff3++;
            }

            return diff1 * diff3;
        }

        public override object Part2(string input)
        {
            return OneDiffSequences(input)
                .Aggregate(1L, (x, y) => x * permutationsFor[y]);
        }

        private IEnumerable<int> OneDiffSequences(string input)
        {
            int last = 0, group = 0;
            foreach (var i in FullRange(input))
            {
                if (i - last == 3)
                {
                    yield return group;
                    group = 0;
                }

                group++;
                last = i;
            }

            if (group > 0)
                yield return group;

            static IEnumerable<int> FullRange(string input)
            {
                var it = input.ToLines()
                    .Select(int.Parse)
                    .OrderBy(x => x);

                int max = 0;
                yield return max;

                foreach (var num in it)
                    yield return max = num;

                yield return max + 3;
            }
        }

        private readonly int[] permutationsFor = new[]
        {
            //  seq of 0 will never happen
            0,
            
            //  seq of 1 cannot be permuted
            1,
            
            //  seq of 2 could be reduced by 1 IF we had any gaps of 2 but we don't
            1,
            
            //  seq of 3:
            //      [a, b, c]
            //      [a, c]
            2,
        
            //  seq of 4:
            //      [a, b, c, d]
            //      [a, c, d]
            //      [a, b, d]
            //      [a, d]
            4,

            //  seq of 5
            //      [a, b, c, d, e]
            //      [a, b, c, e]
            //      [a, b, d, e]
            //      [a, c, d, e]
            //      [a, b, e]
            //      [a, c, e]
            //      [a, d, e]
            7
        };

        private const string test1 = @"
16
10
15
5
1
11
7
19
6
12
4";

        private const string test2 = @"
28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3";
    }
}
