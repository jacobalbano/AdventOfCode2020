using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            Assert.AreEqual(26L, Part1("2 * 3 + (4 * 5)"));
            Assert.AreEqual(437L, Part1("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(12240L, Part1("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(13632L, Part1("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        public override void Part2Test()
        {
            Assert.AreEqual(51L, Part2("1 + (2 * 3) + (4 * (5 + 6))"));
            Assert.AreEqual(46L, Part2("2 * 3 + (4 * 5)"));
            Assert.AreEqual(1445L, Part2("5 + (8 * 3 + 9 + 3 * 4 * 3)"));
            Assert.AreEqual(669060L, Part2("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"));
            Assert.AreEqual(23340L, Part2("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"));
        }

        public override object Part1(string input)
        {
            return Evaluate(input, advancedMode: false);
        }

        public override object Part2(string input)
        {
            return Evaluate(input, advancedMode: true);
        }

        private static long Evaluate(string input, bool advancedMode)
        {
            var interpreter = new Interpreter();
            var parser = new Parser(advancedMode);

            return input.ToLines()
                .Select(parser.Parse)
                .Select(interpreter.Visit)
                .Sum();
        }

        private class Parser
        {
            public Parser(bool advancedMode)
            {
                prefix[TokenType.GroupOpen] = new GroupParselet();
                prefix[TokenType.Value] = new ValueParselet();
                infix[TokenType.Add] = new AddParselet(advancedMode);
                infix[TokenType.Multiply] = new MultiplyParselet();
            }

            public IExpr Parse(string expression)
            {
                using var ctx = new ParserContext(prefix, infix, Tokenize(expression));
                return ctx.ParseExpression(0);
            }

            private sealed class ParserContext : IDisposable
            {
                public ParserContext(Dictionary<TokenType, IPrefixParselet> prefix, Dictionary<TokenType, IInfixParselet> infix, IEnumerable<Token> enumerable)
                {
                    this.prefix = prefix;
                    this.infix = infix;
                    tokens = enumerable.ToList();
                }

                public IExpr ParseExpression(int precedence)
                {
                    if (disposed)
                        throw new ObjectDisposedException(nameof(ParserContext));

                    var token = Consume();
                    if (!prefix.TryGetValue(token.Type, out var pfixParser))
                        throw new Exception($"No parser found for token {token}");

                    var left = pfixParser.Parse(this, token);
                    while (precedence < GetPrecedence())
                    {
                        token = Consume();
                        left = infix[token.Type].Parse(this, left, token);
                    }

                    return left;
                }

                public Token Consume()
                {
                    if (disposed)
                        throw new ObjectDisposedException(nameof(ParserContext));

                    if (tokens.Count > 0)
                    {
                        var result = tokens[0];
                        tokens.RemoveAt(0);
                        return result;
                    }

                    return null;
                }

                public Token LookAhead(int count)
                {
                    if (disposed)
                        throw new ObjectDisposedException(nameof(ParserContext));

                    if (tokens.Count <= count)
                        return null;

                    return tokens[count];
                }

                public int GetPrecedence()
                {
                    if (disposed)
                        throw new ObjectDisposedException(nameof(ParserContext));

                    if (tokens.Count == 0 || !infix.TryGetValue(tokens[0].Type, out var parselet))
                        return 0;

                    return parselet.Precedence;
                }

                public void Dispose()
                {
                    disposed = true;
                }

                private readonly List<Token> tokens;
                private bool disposed;
                private readonly Dictionary<TokenType, IPrefixParselet> prefix;
                private readonly Dictionary<TokenType, IInfixParselet> infix;
            }

            private interface IPrefixParselet
            {
                IExpr Parse(ParserContext ctx, Token token);
            }

            private interface IInfixParselet
            {
                IExpr Parse(ParserContext ctx, IExpr left, Token token);
                int Precedence { get; }
            }

            private static IEnumerable<Token> Tokenize(string line)
            {
                return tokenizer.Split(line)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(t => t switch
                    {
                        "+" => new Token { Type = TokenType.Add },
                        "*" => new Token { Type = TokenType.Multiply },
                        "(" => new Token { Type = TokenType.GroupOpen },
                        ")" => new Token { Type = TokenType.GroupClose },
                        _ when int.TryParse(t, out var val) => new Token { Type = TokenType.Value, Value = val },
                        _ => throw new UnreachableCodeException()
                    });
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

            private class GroupParselet : IPrefixParselet
            {
                public IExpr Parse(ParserContext ctx, Token token)
                {
                    var result = ctx.ParseExpression(0);
                    Assert.AreEqual(TokenType.GroupClose, ctx.Consume()?.Type, "Mismatched parentheses");
                    return result;
                }
            }

            private class ValueParselet : IPrefixParselet
            {
                public IExpr Parse(ParserContext ctx, Token token)
                {
                    return new ValueExpr { Value = (int) token.Value };
                }
            }

            private class AddParselet : IInfixParselet
            {
                public AddParselet(bool advancedMode)
                {
                    Precedence = advancedMode ? 2 : 1;
                }

                public int Precedence { get; }

                public IExpr Parse(ParserContext ctx, IExpr left, Token token)
                {
                    var right = ctx.ParseExpression(Precedence);
                    return new AddExpr { Left = left, Right = right };
                }
            }

            private class MultiplyParselet : IInfixParselet
            {
                public int Precedence => 1;

                public IExpr Parse(ParserContext ctx, IExpr left, Token token)
                {
                    var right = ctx.ParseExpression(Precedence);
                    return new MultExpr { Left = left, Right = right };
                }
            }

            private static readonly Regex tokenizer = new Regex(@"([(=*)]|(?:\d*))", RegexOptions.Compiled);
            private readonly Dictionary<TokenType, IPrefixParselet> prefix = new Dictionary<TokenType, IPrefixParselet>();
            private readonly Dictionary<TokenType, IInfixParselet> infix = new Dictionary<TokenType, IInfixParselet>();
        }

        private interface IExpr
        {
            T Accept<T>(IVisitor<T> visitor);
        }

        private interface IVisitor<out T>
        {
            T Visit(IExpr expr);
            T Visit(ValueExpr expr);
            T Visit(AddExpr expr);
            T Visit(MultExpr expr);
        }

        private class Interpreter : IVisitor<long>
        {
            public long Visit(IExpr expr)
            {
                return expr.Accept(this);
            }

            public long Visit(ValueExpr expr)
            {
                return expr.Value;
            }

            public long Visit(AddExpr expr)
            {
                var lValue = Visit(expr.Left);
                var rValue = Visit(expr.Right);
                return lValue + rValue;
            }

            public long Visit(MultExpr expr)
            {
                var lValue = Visit(expr.Left);
                var rValue = Visit(expr.Right);
                return lValue * rValue;
            }
        }

        private class AddExpr : IExpr
        {
            public IExpr Left { get; init; }
            public IExpr Right { get; init; }
            public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
        }

        private class MultExpr : IExpr
        {
            public IExpr Left { get; init; }
            public IExpr Right { get; init; }
            public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
        }

        private class ValueExpr : IExpr
        {
            public int Value { get; init; }
            public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
        }
    }
}
