using AdventOfCode2020.Util.Validators;
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
            Assert.AreEqual(71, Part1(testData));
        }

        public override object Part1(string input)
        {
            var data = Data.Parse(input);
            var results = data.NearbyTickets
                .SelectMany(x => x)
                .Where(x => !data.Fields.Values.Any(y => y.Validate(x)))
                .ToArray();

            return results.Sum();
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
        }

        private const string testData = @"
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
    }
}
