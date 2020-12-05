﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode2020.Util.Validators
{
    public interface IValidator<T>
    {
        bool Validate(T input);
    }
}
