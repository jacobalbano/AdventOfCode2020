using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(19, "Monster Messages")]
    class Day19 : ChallengeBase
    {

        public override void Part1Test()
        {
            base.Part1Test();
        }

        public override object Part1(string input)
        {
            var e = input.ToLines()
                .PartitionBy(string.IsNullOrWhiteSpace)
                .GetEnumerator();

            e.MoveNext();
            var rules = ParseRules(e.Current);

            e.MoveNext();
            var messages = e.Current.ToList();

            throw new NotImplementedException();
        }

        private static IReadOnlyList<IRule> ParseRules(IEnumerable<string> lines)
        {
            var rules = new Dictionary<int, IRule>();

            return rules
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToList();
        }

        private interface IRule
        {
            bool Matches(string message);
        }

        private class SingleLetterRule : IRule
        {
            public char Letter { get; init; }
            public bool Matches(string message) => message.Length == 1 && message[0] == Letter;
        }

        private class OptionRule : IRule
        {
            public IRule Left { get; init; }
            public IRule Right { get; init; }
            public bool Matches(string message) => Left.Matches(message) || Right.Matches(message);
        }

        private class PassthroughRule : IRule
        {
            public IRule Next { get; init; }
            public bool Matches(string message) => Next.Matches(message);
        }

        private const string testData = @"
0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb";

    }
}
