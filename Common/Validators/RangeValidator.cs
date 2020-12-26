using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common.Validators
{
    class RangeValidator<T> : IValidator<T>
        where T : IComparable
    {
        public T Min { get; }
        public T Max { get; }
        public bool UpperInclusive { get; init; }

        public RangeValidator(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool Validate(T input)
        {
            return input.CompareTo(Min) >= 0 && (
                (UpperInclusive && input.CompareTo(Max) <= 0) ||
                (input.CompareTo(Max) < 0));
        }
    }
}
