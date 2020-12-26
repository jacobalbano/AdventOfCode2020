using AdventOfCode2020.Util;
using AdventOfCode2020.Common.Validators;
using AdventOfCodeScaffolding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges
{
    [Challenge(4, "Passport Processing")]
    class Day04 : ChallengeBase
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
            return PassportHasAllFields(passport) && passport.All(x => x.Key switch
            {
                "byr" => birthYear.Validate(x.Value),
                "iyr" => issueYear.Validate(x.Value),
                "eyr" => expireYear.Validate(x.Value),
                "hgt" => height.Validate(x.Value),
                "hcl" => hairColor.Validate(x.Value),
                "ecl" => eyeColor.Validate(x.Value),
                "pid" => passportId.Validate(x.Value),
                "cid" => true,
                _ => throw new UnreachableCodeException()
            });
        }

        private static readonly IValidator<string> birthYear = new PredicateValidator<string>(x => int.TryParse(x, out var byr) && byr.IsBetween(1920, 2002));
        private static readonly IValidator<string> issueYear = new PredicateValidator<string>(x => int.TryParse(x, out var byr) && byr.IsBetween(2010, 2020));
        private static readonly IValidator<string> expireYear = new PredicateValidator<string>(x => int.TryParse(x, out var byr) && byr.IsBetween(2020, 2030));
        private static readonly IValidator<string> heightCm = new PredicateValidator<string>(x => x[^2..] == "cm" && int.Parse(x[..^2]).IsBetween(150, 193));
        private static readonly IValidator<string> heightIn = new PredicateValidator<string>(x => x[^2..] == "in" && int.Parse(x[..^2]).IsBetween(59, 76));
        private static readonly IValidator<string> height = new AnyValidator<string>(heightIn, heightCm);
        private static readonly IValidator<string> eyeColor = new OptionValidator<string>("amb", "blu", "brn", "gry", "grn", "hzl", "oth");
        private static readonly IValidator<string> passportId = new PredicateValidator<string>(x => x.Length == 9 && int.TryParse(x, out _));
        private static readonly IValidator<string> hairColor = new PredicateValidator<string>(x => x[0] == '#' && x[1..].All(x => x.IsBetween('0', '9') || x.IsBetween('a', 'f')));
        private static readonly IReadOnlyList<string> requiredKeys = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        public enum TokenKind { New, Key, Value }
        private struct Token
        {
            public TokenKind Kind { get; init; }
            public string Value { get; init; }
        }

        private static IEnumerable<IReadOnlyDictionary<string, string>> ParsePassports(string input)
        {
            var e = Tokenize(input).GetEnumerator();
            bool hasNext = e.MoveNext();
            while (hasNext)
            {
                var passport = new Dictionary<string, string>();
                while (hasNext && e.Current.Kind == TokenKind.Key)
                {
                    var key = e.Current.Value;
                    Assert.IsTrue(e.MoveNext(), "Enumeration continues");
                    Assert.AreEqual(TokenKind.Value, e.Current.Kind);

                    passport[key] = e.Current.Value;
                    if ((hasNext = e.MoveNext()) && e.Current.Kind == TokenKind.New)
                        break;
                }

                yield return passport;
                hasNext = e.MoveNext();
            }
        }

        private static IEnumerable<Token> Tokenize(string input)
        {
            foreach (var passport in input.ToLines().PartitionBy(string.IsNullOrWhiteSpace))
            {
                yield return new Token { Kind = TokenKind.New };
                foreach (var line in passport)
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
