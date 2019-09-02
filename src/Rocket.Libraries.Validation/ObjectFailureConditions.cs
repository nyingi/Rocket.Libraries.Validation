using Rocket.Libraries.Validation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation.FailureDescriptors
{
    public static class ObjectFailureConditions
    {
        public static bool IsDefault<TValue>(TValue value)
        {
            return value == default;
        }
    }
}