using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Common.Validators
{
    class TransformValidator<TIn, TOut> : IValidator<TIn>
    {
        public delegate (bool, TOut) TryTransformTuple(TIn input);
        public delegate bool TryTransform(TIn input, out TOut output);

        public TransformValidator(TryTransformTuple handler, IValidator<TOut> continueWith)
        {
            Handler = handler;
            ContinueWith = continueWith;
        }

        public TransformValidator(TryTransform handler, IValidator<TOut> continueWith)
        {
            Handler = x => (handler(x, out var result), result);
            ContinueWith = continueWith;
        }

        public bool Validate(TIn input)
        {
            var (success, result) = Handler(input);
            if (!success)
                return false;

            return ContinueWith.Validate(result);
        }

        private readonly TryTransformTuple Handler;
        private readonly IValidator<TOut> ContinueWith;
    }
}
