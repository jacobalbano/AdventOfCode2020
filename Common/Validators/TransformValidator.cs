using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common.Validators
{
    class TransformValidator<TIn, TOut> : IValidator<TIn>
    {
        public TransformValidator(Func<TIn, (TOut, bool)> tryTransform, IValidator<TOut> continueWith)
        {
            TryTransform = tryTransform;
            ContinueWith = continueWith;
        }

        public bool Validate(TIn input)
        {
            var (result, success) = TryTransform(input);
            if (!success)
                return false;

            return ContinueWith.Validate(result);
        }

        private readonly Func<TIn, (TOut, bool)> TryTransform;
        private readonly IValidator<TOut> ContinueWith;
    }
}
