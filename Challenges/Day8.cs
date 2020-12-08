using AdventOfCode2020.Util;
using AdventOfCodeScaffolding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Challenges
{
    [Challenge(8, "Handheld Halting")]
    class Day8 : ChallengeBase
    {
        public override void Part1Test()
        {
            Assert.AreEqual(5, RunOnce(testInput));
        }

        public override object Part1(string input)
        {
            return RunOnce(input);
        }

        private static int RunOnce(string input)
        {
            var bytecode = input
                .ToLines()
                .Select(line => new Instruction(line[..3],int.Parse(line[3..])))
                .ToArray();

            var visited = new HashSet<int>();
            foreach (var state in Run(bytecode))
            {
                if (!visited.Add(state.InstrPointer))
                    return state.Accumulator;
            }

            throw new UnreachableCodeException();
        }

        private static IEnumerable<VmState> Run(Instruction[] instructions, VmState initialState = default)
        {
            int i = initialState.InstrPointer, acc = initialState.Accumulator;
            while (i < instructions.Length)
            {
                var (op, val) = (instructions[i].Op, instructions[i].Value);
                yield return new VmState(i, acc, false);

                switch (op[0])
                {
                    case 'n': i++; break;
                    case 'j': i += val; break;
                    case 'a': acc += val; i++; break;
                }
            }

            yield return new VmState(i, acc, true);
        }

        private struct Instruction
        {
            public Instruction(string op, int val)
            {
                Op = op;
                Value = val;
            }

            public string Op { get; }
            public int Value { get; }
        }

        private struct VmState
        {
            public VmState(int instrPointer, int acc, bool isHalted)
            {
                InstrPointer = instrPointer;
                Accumulator = acc;
                IsHalted = isHalted;
            }

            public int InstrPointer { get; }
            public int Accumulator { get; }
            public bool IsHalted { get; }
        }

        private const string testInput = @"
nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6
";
    }
}
