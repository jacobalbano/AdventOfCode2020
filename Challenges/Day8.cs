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
                switch (inst.Op[0])
                {
                    case 'n':
                        state = RunInner(state, state.InstrPointer, new Instruction("jmp", inst.Value));
                        break;
                    case 'j':
                        state = RunInner(state, state.InstrPointer, new Instruction("nop", inst.Value));
                        break;
                }

                if (state.IsHalted)
                    return state;
            }

            throw new UnreachableCodeException();

            VmState RunInner(VmState initialState, int patchAt, Instruction patch)
            {
                var old = bytecode[patchAt];
                bytecode[patchAt] = patch;
                var result = RunOnce(bytecode, initialState);
                bytecode[patchAt] = old;
                return result;
            }
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

            public static Instruction Parse(string line) => new Instruction(line[..3], int.Parse(line[3..]));
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
