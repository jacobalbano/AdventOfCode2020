using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(18, "Operation Order")]
    class Day18 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(26, Evaluate("2 * 3 + (4 * 5)"));
            Assert.AreEqual(437, Evaluate("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(12240, Evaluate("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(13632, Evaluate("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        public override object Part1(string input)
        {
            return input.ToLines()
                .Select(Evaluate)
                .Sum();
        }

        private static long Evaluate(string line)
        {
            var tokens = Tokenize(line);
            var lexed = Lex(tokens);
            return RunProgram(lexed);
        }

        private static IEnumerable<string> Tokenize(string line)
        {
            return re.Split(line)
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        private static readonly Regex re = new Regex(@"([(=*)]|(?:\d*))", RegexOptions.Compiled);

        private static IEnumerable<Token> Lex(IEnumerable<string> tokens) => tokens.Select(t => t switch
        {
            "+" => new Token { Type = TokenType.Add },
            "*" => new Token { Type = TokenType.Multiply },
            "(" => new Token { Type = TokenType.GroupOpen },
            ")" => new Token { Type = TokenType.GroupClose },
            _ when int.TryParse(t, out var val) => new Token { Type = TokenType.Value, Value = val },
            _ => throw new UnreachableCodeException()
        });


        private static long RunProgram(IEnumerable<Token> tokens)
        {
            return Inner(tokens.GetEnumerator(), false);

            static long Inner(IEnumerator<Token> e, bool isInGroup)
            {
                e.MoveNext();
                long left = e.Current.Type switch
                {
                    TokenType.Value => (int)e.Current.Value,
                    TokenType.GroupOpen => Inner(e, true),
                    _ => throw new Exception("Corrupt program")
                };

                while (e.MoveNext())
                {
                    if (e.Current.Type == TokenType.GroupClose)
                    {
                        if (isInGroup) break;
                        else throw new Exception("Corrupt program");
                    }

                    var op = e.Current.Type switch
                    {
                        TokenType.Add => e.Current.Type,
                        TokenType.Multiply => e.Current.Type,
                        _ => throw new Exception("Corrupt program")
                    };

                    Assert.IsTrue(e.MoveNext(), "Corrupt program");
                    long right = e.Current.Type switch
                    {
                        TokenType.Value => (int)e.Current.Value,
                        TokenType.GroupOpen => Inner(e, true),
                        _ => throw new Exception("Corrupt program")
                    };

                    left = op switch
                    {
                        TokenType.Add => left + right,
                        TokenType.Multiply => left * right,
                        _ => throw new Exception("Corrupt program")
                    };
                }

                return left;
            }
        }

        enum TokenType
        {
            None,
            Value,
            GroupOpen,
            GroupClose,
            Add,
            Multiply
        }

        private record Token
        {
            public TokenType Type { get; init; }
            public int? Value { get; init; }
        }
    }
}
