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
            Assert.AreEqual(5, RunOnce(testInput.ToLines()
                .Select(Instruction.Parse)
                .ToArray()).Accumulator);
        }

        public override object Part1(string input)
        {
            return RunOnce(input.ToLines()
                .Select(Instruction.Parse)
                .ToArray()).Accumulator;
        }

        public override void Part2Test()
        {
            Assert.AreEqual(8, RunUntilHalt(testInput.ToLines()
                .Select(Instruction.Parse)
                .ToArray()).Accumulator);
        }

        public override object Part2(string input)
        {
            return RunUntilHalt(input.ToLines()
                .Select(Instruction.Parse)
                .ToArray()).Accumulator;
        }

        private static VmState RunOnce(Instruction[] bytecode, VmState initialState = default)
        {
            var visited = new HashSet<int>();
            return Run(bytecode, initialState)
                .TakeWhile(x => visited.Add(x.InstrPointer))
                .Last();
        }

        private static VmState RunUntilHalt(Instruction[] bytecode)
        {
            var step = Run(bytecode).GetEnumerator();
            while (step.MoveNext())
            {
                var state = step.Current;
                var inst = bytecode[state.InstrPointer];
                switch (inst.Op)
                {
                    case 'n':
                        state = RunInner(state, inst with { Op = 'j' });
                        break;
                    case 'j':
                        state = RunInner(state, inst with { Op = 'n' });
                        break;
                }

                if (state.IsHalted)
                    return state;
            }

            throw new UnreachableCodeException();

            VmState RunInner(VmState initialState, Instruction patch)
            {
                var old = bytecode[initialState.InstrPointer];
                bytecode[initialState.InstrPointer] = patch;
                var result = RunOnce(bytecode, initialState);
                bytecode[initialState.InstrPointer] = old;
                return result;
            }
        }

        private static IEnumerable<VmState> Run(Instruction[] instructions, VmState initialState = default  )
        {
            int i = initialState?.InstrPointer ?? 0,
                acc = initialState?.Accumulator ?? 0;

            while (i < instructions.Length)
            {
                var (op, val) = (instructions[i].Op, instructions[i].Value);
                yield return new VmState { InstrPointer = i, Accumulator = acc, IsHalted = false };

                switch (op)
                {
                    case 'n': i++; break;
                    case 'j': i += val; break;
                    case 'a': acc += val; i++; break;
                }
            }

            yield return new VmState { InstrPointer = i, Accumulator = acc, IsHalted = true };
        }

        private record Instruction
        {
            public char Op { get; init; }
            public int Value { get; init; }

            public static Instruction Parse(string line) => new Instruction { Op = line[0], Value = int.Parse(line[3..]) };
        }

        private record VmState
        {
            public int InstrPointer { get; init; }
            public int Accumulator { get; init; }
            public bool IsHalted { get; init; }
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
