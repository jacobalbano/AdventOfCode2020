using AdventOfCode2020.Util;
using AdventOfCode2020.Util.Validators;
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
            var nodes = Part1Nodes(input);
            if (!nodes.TryGetValue("shiny gold", out var myBag))
                throw new UnreachableCodeException();

            return FindNodesContaining(myBag)
                .Distinct()
                .Count();

            static IEnumerable<Node1> FindNodesContaining(Node1 node)
            {
                foreach (var parent in node.Parents)
                {
                    yield return parent;
                    foreach (var grandparent in FindNodesContaining(parent))
                        yield return grandparent;
                }
            }
        }

        private static int GetTotalContainedBags(string input)
        {
            var nodes = Part2Nodes(input);
            if (!nodes.TryGetValue("shiny gold", out var myBag))
                throw new UnreachableCodeException();

            return ExploreBag(myBag) - 1;

            static int ExploreBag(Node2 node)
            {
                int result = 1;
                foreach (var (child, count) in node.Children)
                    result += ExploreBag(child) * count;

                return result;
            }
        }

        private static IReadOnlyDictionary<string, Node1> Part1Nodes(string input)
        {
            var result = new Dictionary<string, Node1>();

            foreach (var line in input.ToLines())
            {
                var parser = new StringParser(line);
                ReadBagColor(parser, out var parentColor);

                var parent = result.Establish(parentColor, x => new Node1 { Color = x });
                while (parser.HasMaterial)
                {
                    ReadChildNode(parser, out var color, out var count);
                    var child = result.Establish(color, x => new Node1 { Color = x });
                    child.Parents.Add(parent);
                }
            }

            return result;
        }

        private static IReadOnlyDictionary<string, Node2> Part2Nodes(string input)
        {
            var result = new Dictionary<string, Node2>();

            foreach (var line in input.ToLines())
            {
                var parser = new StringParser(line);

                ReadBagColor(parser, out var parentColor);

                var parent = result.Establish(parentColor, x => new Node2 { Color = x });
                while (parser.HasMaterial)
                {
                    ReadChildNode(parser, out var color, out var count);

                    var child = result.Establish(color, x => new Node2 { Color = x });
                    parent.Children[child] = count;
                }
            }

            return result;
        }

        private static void ReadChildNode(StringParser parser, out string color, out int count)
        {
            count = parser.ReadInt();
            parser.Skip(1);

            ReadBagColor(parser, out color);
        }

        private static void ReadBagColor(StringParser parser, out string color)
        {
            color = parser.ReadUntil("bag", skip: true).Trim();
            parser.SkipWhile(x => !char.IsDigit(x));
        }

        class Node1
        {
            public string Color { get; init; }
            public List<Node1> Parents { get; } = new List<Node1>();
        }

        class Node2
        {
            public string Color { get; init; }
            public Dictionary<Node2, int> Children { get; } = new Dictionary<Node2, int>();
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
