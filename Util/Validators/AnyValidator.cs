using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2020.Util.Validators
{
    class AnyValidator<T> : IValidator<T>
    {
        public AnyValidator(params IValidator<T>[] validators)
        {
            this.validators = validators;
        }

        public bool Validate(T input)
        {
            return validators.Any(x => x.Validate(input));
        }

        private readonly IValidator<T>[] validators;
    }
}
