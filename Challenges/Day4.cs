using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges
{
    [Challenge(4, "Passport Processing")]
    class Day4 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(2, Part1(testInput1));
        }

        public override object Part1(string input)
        {
            return ParsePassports(input)
                .Where(PassportHasAllFields)
                .Count();
        }

        public override void Part2Test()
        {
            Assert.AreEqual(0, Part2(testInputInvalid));
            Assert.AreEqual(4, Part2(testInputValid));
        }

        public override object Part2(string input)
        {
            return ParsePassports(input)
                .Where(PassportIsFullyValid)
                .Count();
        }

        private bool PassportHasAllFields(IReadOnlyDictionary<string, string> passport)
        {
            return requiredKeys.All(passport.ContainsKey);
        }

        private bool PassportIsFullyValid(IReadOnlyDictionary<string, string> passport)
        {
            bool result = PassportHasAllFields(passport);
            foreach (var (key, value) in passport)
            {
                switch (key)
                {
                    case "byr": result &= int.TryParse(value, out var byr) && byr.IsBetween(1920, 2002); break;
                    case "iyr": result &= int.TryParse(value, out var iyr) && iyr.IsBetween(2010, 2020); break;
                    case "eyr": result &= int.TryParse(value, out var eyr) && eyr.IsBetween(2020, 2030); break;
                    case "hgt": result &= (value[^2..] == "cm" && int.Parse(value[..^2]).IsBetween(150, 193)) || (value[^2..] == "in" && int.Parse(value[..^2]).IsBetween(59, 76)); break;
                    case "hcl": result &= value[0] == '#' && value[1..].All(x => x.IsBetween('0', '9') || x.IsBetween('a', 'f')); break;
                    case "ecl": result &= validEyeColors.Contains(value); break;
                    case "pid": result &= value.Length == 9 && int.TryParse(value, out _); break;
                    case "cid": break;
                    default: Assert.Unreachable(); break;
                }
            }

            return result;
        }

        private static readonly HashSet<string> validEyeColors = new HashSet<string> { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
        private static readonly IReadOnlyList<string> requiredKeys = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        public enum TokenKind { New, Key, Value }
        private struct Token
        {
            public TokenKind Kind { get; set; }
            public string Value { get; set; }
        }

        private IEnumerable<IReadOnlyDictionary<string, string>> ParsePassports(string input)
        {
            var e = Tokenize(input).GetEnumerator();
            bool hasNext = e.MoveNext();
            while (hasNext)
            {
                var c = e.Current;
                Assert.AreEqual(TokenKind.New, c.Kind);
                var passport = new Dictionary<string, string>();
                
                Assert.IsTrue(hasNext = e.MoveNext(), "Enumeration continues");
                Assert.AreEqual(e.Current.Kind, TokenKind.Key);

                while (hasNext && e.Current.Kind == TokenKind.Key)
                {
                    var key = e.Current.Value;

                    Assert.IsTrue(hasNext = e.MoveNext(), "Enumeration continues");
                    Assert.AreEqual(e.Current.Kind, TokenKind.Value);

                    passport[key] = e.Current.Value;
                    hasNext = e.MoveNext();
                }

                yield return passport;
            }
        }

        private IEnumerable<Token> Tokenize(string input)
        {
            yield return new Token { Kind = TokenKind.New };
            foreach (var line in input.ToLines())
            {
                if (string.IsNullOrEmpty(line))
                    yield return new Token { Kind = TokenKind.New };
                else
                {
                    foreach (var pair in line.Split(' '))
                    {
                        yield return new Token { Kind = TokenKind.Key, Value = pair[..3] };
                        yield return new Token { Kind = TokenKind.Value, Value = pair[4..] };
                    }
                }
            }
        }

        private const string testInput1 = @"
ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";

        private const string testInputInvalid = @"
eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";

        private const string testInputValid = @"
pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";

    }
}
