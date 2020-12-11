using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges
{
    [Challenge(2, "Password Philosophy")]
    class Day2 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.IsTrue(IsSledPasswordValid("1-3 a: abcde"), "First password should match");
            Assert.IsFalse(IsSledPasswordValid("1-3 b: cdefg"), "Second password should not match");
            Assert.IsTrue(IsSledPasswordValid("2-9 c: ccccccccc"), "Third password should match");
        }

        public override object Part1(string input)
        {
            return input.ToLines()
                .Where(IsSledPasswordValid)
                .Count();
        }

        public override void Part2Test()
        {
            Assert.IsTrue(IsTobogganPasswordValid("1-3 a: abcde"), "First password should match");
            Assert.IsFalse(IsTobogganPasswordValid("1-3 b: cdefg"), "Second password should not match");
            Assert.IsFalse(IsTobogganPasswordValid("2-9 c: ccccccccc"), "Third password should not match");
        }

        public override object Part2(string input)
        {
            return input.ToLines()
                .Where(IsTobogganPasswordValid)
                .Count();
        }

        private bool IsSledPasswordValid(string inputLine)
        {
            Parse(inputLine, out var min, out var max, out var target, out var password);

            var matchingCount = password.Count(x => x == target);
            return matchingCount >= min && matchingCount <= max;
        }

        private bool IsTobogganPasswordValid(string inputLine)
        {
            Parse(inputLine, out var i, out var j, out var target, out var password);
            return (password[i - 1] == target) ^ (password[j - 1] == target);
        }

        private static void Parse(string input, out int a, out int b, out char target, out string password)
        {
            var e = new StringParser(input);
            a = e.ReadInt();
            e.Skip("-".Length);
            b = e.ReadInt();
            e.Skip(" ".Length);
            target = e.ReadChar();
            e.Skip(": ".Length);
            password = e.ReadRemainder();
        }
    }
}
