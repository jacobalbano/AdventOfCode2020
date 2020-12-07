using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Challenges
{
    [Challenge(7, "Handy Haversacks")]
    class Day7 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(4, GetPossibleContainingBags(part1Test));
        }

        public override object Part1(string input)
        {
            return GetPossibleContainingBags(input);
        }


        public override void Part2Test()
        {
            Assert.AreEqual(126, GetTotalContainedBags(part2Test));
        }

        public override object Part2(string input)
        {
            return GetTotalContainedBags(input);
        }

        private static int GetPossibleContainingBags(string input)
        {
            var nodes = NodesByColor(input);
            if (!nodes.TryGetValue("shiny gold", out var myBag))
                throw new UnreachableCodeException();

            return FindNodesContaining(myBag)
                .Distinct()
                .Count();

            static IEnumerable<Node> FindNodesContaining(Node node)
            {
                foreach (var parent in node.ContainedBy)
                {
                    yield return parent;
                    foreach (var grandparent in FindNodesContaining(parent))
                        yield return grandparent;
                }
            }
        }

        private static int GetTotalContainedBags(string input)
        {
            var nodes = NodesByColor(input);
            if (!nodes.TryGetValue("shiny gold", out var myBag))
                throw new UnreachableCodeException();

            return ExploreBag(myBag);

            static int ExploreBag(Node node)
            {
                int result = node.Contains.Count;
                foreach (var child in node.Contains)
                    result += ExploreBag(child);

                return result;
            }
        }

        private static IReadOnlyDictionary<string, Node> NodesByColor(string input)
        {
            var result = new Dictionary<string, Node>();

            foreach (var line in input.ToLines())
            {
                var parser = new StringParser(line);
                var parentColor = parser.ReadUntil(" bags contain ", skip:true);
                var parent = result.Establish(parentColor, x => new Node { Color = x });

                while (parser.HasMaterial)
                {
                    if (!parser.TryReadInt(out var num))
                        break;

                    parser.Skip(1);
                    var childColor = parser.ReadUntil(" bag", skip: true);
                    parser.SkipAny("s,. ");

                    var child = result.Establish(childColor, x => new Node { Color = x });
                    while (num --> 0)
                        parent.Contains.Add(child);
                    child.ContainedBy.Add(parent);
                }
            }

            return result;
        }

        class Node
        {
            public string Color { get; set; }
            public List<Node> ContainedBy { get; } = new List<Node>();
            public List<Node> Contains { get; } = new List<Node>();

            public override string ToString() => $"{Color}{(ContainedBy.Any() ? "" : "@")}";
        }

        private const string part1Test = @"
light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.";

        private const string part2Test = @"
shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.";
    }
}
