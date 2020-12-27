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
            Assert.AreEqual(2, Part1(testData1));
        }

        public override object Part1(string input)
        {
            ParseData(input, out var rawRules, out var messages);
            var rules = ParseRules(rawRules);

            return Solve(rules, messages);
        }

        public override void Part2Test()
        {
            Assert.AreEqual(3, Part1(testData2));
            Assert.AreEqual(12, Part2(testData2));
        }

        public override object Part2(string input)
        {
            ParseData(input, out var rawRules, out var messages);
            rawRules[8] = "42 | 42 8";
            rawRules[11] = "42 31 | 42 11 31";

            var rules = ParseRules(rawRules);
            return Solve(rules, messages);
        }

        private static int Solve(Dictionary<int, IRule> rules, IReadOnlyList<string> messages)
        {
            var ruleZero = rules[0];
            return messages.Count(x =>
            {
                int cursor = 0;
                var match = ruleZero.Matches(x, ref cursor);
                return match && cursor == x.Length;
            });
        }

        private static void ParseData(string input, out Dictionary<int, string> rawRules, out string[] messages)
        {
            var e = input.ToLines()
                .PartitionBy(string.IsNullOrWhiteSpace)
                .GetEnumerator();

            e.MoveNext();
            rawRules = e.Current
                .Select(x => (str: x, colon: x.IndexOf(':')))
                .Select(x => (id: int.Parse(x.str[..x.colon]), line: x.str[(x.colon + 1)..].Trim()))
                .ToDictionary(x => x.id, x => x.line);

            e.MoveNext();
            messages = e.Current.ToArray();
        }

        private static Dictionary<int, IRule> ParseRules(Dictionary<int, string> rawRules)
        {
            var rules = new Dictionary<int, IRule>();
            foreach (var (id, line) in rawRules)
                rules[id] = ParseRule(line);

            return rules;

            IRule ParseRule(string fragment)
            {
                int idx = 0;
                if (fragment[0] == '"')
                {
                    return new SingleLetterRule
                    {
                        Letter = fragment.Trim('"').Single()
                    };
                }
                else if ((idx = fragment.IndexOf('|')) >= 0)
                {
                    return new OptionRule
                    {
                        Left = ParseRule(fragment[0..idx]),
                        Right = ParseRule(fragment[(idx + 1)..])
                    };
                }
                else
                {
                    return new GroupRule
                    {
                        Rules = fragment.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => new ReferenceRule(int.Parse(x), rules))
                            .ToArray()
                    };
                }
            }
        }

        private interface IRule
        {
            bool Matches(string message, ref int cursor);
        }

        private class GroupRule : IRule
        {
            public IReadOnlyList<IRule> Rules { get; init; }

            public bool Matches(string message, ref int cursor)
            {
                var oldCursor = cursor;
                foreach (var rule in Rules)
                {
                    if (!rule.Matches(message, ref cursor))
                    {
                        cursor = oldCursor;
                        return false;
                    }
                }

                return true;
            }
        }

        private class OptionRule : IRule
        {
            public IRule Left { get; init; }
            public IRule Right { get; init; }

            public bool Matches(string message, ref int cursor)
            {
                var oldCursor = cursor;
                if (Left.Matches(message, ref cursor))
                    return true;

                cursor = oldCursor;
                if (Right.Matches(message, ref cursor))
                    return true;

                cursor = oldCursor;
                return false;
            }
        }

        private class ReferenceRule : IRule
        {
            public bool Matches(string message, ref int cursor)
            {
                return GetLazy().Matches(message, ref cursor);
            }

            public ReferenceRule(int reference, Dictionary<int, IRule> rules)
            {
                IRule storage = null;
                GetLazy = () => storage ??= rules[reference];
            }

            private readonly Func<IRule> GetLazy;
        }

        private class SingleLetterRule : IRule
        {
            public char Letter { get; init; }
            public bool Matches(string message, ref int cursor)
            {
                if (cursor >= message.Length)
                    return false;

                if (message[cursor] != Letter)
                    return false;

                cursor++;
                return true;
            }
        }

        private const string testData1 = @"
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

        private const string testData2 = @"
42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba";

    }
}
