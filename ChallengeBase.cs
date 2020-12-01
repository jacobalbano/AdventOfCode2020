using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020
{
    abstract class ChallengeBase
    {
        public int DayNumber { get; }

        public ChallengeBase(int number)
        {
            DayNumber = number;
        }

        public void Run(string input)
        {
            RunInner(1, () => Part1(input));
            RunInner(2, () => Part2(input));
        }

        private void RunInner(int part, Func<object> action)
        {
            try
            {
                var result = action();
                Console.WriteLine($"Part {part} solution result: {result?.ToString() ?? "null"}");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine($"Part {part} not yet implemented");
            }
            catch { throw; }
        }

        protected void Assert(string message, bool condition)
        {
            if (!condition) throw new Exception($"Test failed: {message}");
            Console.WriteLine($"Test passed: {message}");
        }

        protected void Assert(string message, Func<bool> conditionRunner)
        {
            Assert(message, conditionRunner());
        }

        protected abstract object Part1(string input);
        protected abstract object Part2(string input);
    }
}
