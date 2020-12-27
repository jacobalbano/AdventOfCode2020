using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common.Validators
{
    class EachValidator<T> : IValidator<IEnumerable<T>>
    {
        public EachValidator(IValidator<T> continueWith)
        {
            ContinueWith = continueWith;
        }

        public bool Validate(IEnumerable<T> input)
        {
            foreach (var x in input)
                if (!ContinueWith.Validate(x))
                    return false;

            return true;
        }

        private readonly IValidator<T> ContinueWith;
    }
}
