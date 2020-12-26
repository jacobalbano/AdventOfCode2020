using AdventOfCode2020.Common.Validators;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Challenges
{
    [Challenge(16, "Ticket Translation")]
    class Day16 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(71, Part1(testData1));
        }

        public override object Part1(string input)
        {
            var data = Data.Parse(input);
            var results = data.NearbyTickets
                .SelectMany(data.GetInvalidValuesForTicket)
                .ToArray();

            return results.Sum();
        }

        public override void Part2Test()
        {
            var results = Solve(testData2);
            Assert.AreEqual(12, results["class"]);
            Assert.AreEqual(11, results["row"]);
            Assert.AreEqual(13, results["seat"]);
        }

        public override object Part2(string input)
        {
            return Solve(input)
                .Where(x => x.Key.StartsWith("departure"))
                .Select(x => x.Value)
                .Aggregate(1L, (x, y) => x * y);
        }

        private static Dictionary<string, int> Solve(string input)
        {
            var data = Data.Parse(input);
            var validTickets = data.NearbyTickets
                .Where(x => !data.GetInvalidValuesForTicket(x).Any())
                .ToList();

            var cache = new Dictionary<string, List<(int, IValidator<int>)>>();
            for (int i = 0; i < data.MyTicket.Count; i++)
            {
                foreach (var (name, validator) in data.Fields)
                {
                    if (FieldsAt(i).All(x => validator.Validate(x)))
                        cache.Establish(name).Add((i, validator));
                }
            }

            var possible = cache
                .Select(x => x.Value)
                .OrderBy(x => x.Count)
                .ToList();

            for (int i = 0; i < possible.Count; i++)
            {
                var validators = possible[i];
                if (validators.Count == 1)
                {
                    var (single, _) = validators.Single();
                    for (int j = i + 1; j < possible.Count; j++)
                        possible[j].RemoveAll(x => x.Item1 == single);
                }
            }

            return cache
                .Select(x => (x.Key, x.Value.First().Item1))
                .ToDictionary(x => x.Key, x => data.MyTicket[x.Item2]);


            IEnumerable<int> FieldsAt(int index)
            {
                yield return data.MyTicket[index];
                foreach (var ticket in validTickets)
                    yield return ticket[index];
            }
        }

        private class Data
        {
            public IReadOnlyDictionary<string, IValidator<int>> Fields { get; private init; }
            public IReadOnlyList<int> MyTicket { get; private init; }
            public IReadOnlyList<IReadOnlyList<int>> NearbyTickets { get; private init; }

            public static Data Parse(string input)
            {
                var e = input.ToLines()
                    .PartitionBy(string.IsNullOrWhiteSpace)
                    .GetEnumerator();
                e.MoveNext();

                var fields = e.Current
                    .Select(x => x.Split(':'))
                    .ToDictionary(x => x[0], x => (IValidator<int>) new AnyValidator<int>(ParseValidators(x[1])));

                e.MoveNext();
                var myTicket = e.Current
                    .Skip(1)    //  "your ticket:"
                    .Single()
                    .CSV()
                    .Select(int.Parse)
                    .ToArray();

                e.MoveNext();
                var nearbyTickets = e.Current
                    .Skip(1)    //  "nearby tickets:"
                    .Select(x => x.CSV().Select(int.Parse).ToList())
                    .ToList();

                return new Data { Fields = fields, MyTicket = myTicket, NearbyTickets = nearbyTickets };
            }

            private static IEnumerable<IValidator<int>> ParseValidators(string declaration)
            {
                foreach (var range in declaration.Split("or"))
                {
                    var (min, max) = range.Split('-');
                    yield return new RangeValidator<int>(int.Parse(min), int.Parse(max))
                    {
                        UpperInclusive = true
                    };
                }
            }

            public IEnumerable<int> GetInvalidValuesForTicket(IReadOnlyList<int> values)
            {
                foreach (var v in values)
                {
                    if (Fields.Values.Any(x => x.Validate(v)))
                        continue;

                    yield return v;
                }
            }
        }

        private const string testData1 = @"
class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12";

        private const string testData2 = @"
class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9";
    }
}
