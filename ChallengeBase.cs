using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode2020
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ChallengeAttribute : Attribute
    {
        public int DayNum { get; }
        public string Title { get; }

        public ChallengeAttribute(int dayNumber, string title)
        {
            DayNum = dayNumber;
            Title = title;
        }
    }

    public abstract class ChallengeBase
    {
        protected void Assert(string message, bool condition)
        {
            if (!condition) throw new Exception($"Test failed: {message}");
            Console.WriteLine($"Test passed: {message}");
        }

        protected void Assert(string message, Func<bool> conditionRunner)
        {
            Assert(message, conditionRunner());
        }

        public virtual object Part1(string input) => throw new NotImplementedException();
        public virtual void Part1Test() => throw new NotImplementedException();

        public virtual object Part2(string input) => throw new NotImplementedException();
        public virtual void Part2Test() => throw new NotImplementedException();
    }
}
