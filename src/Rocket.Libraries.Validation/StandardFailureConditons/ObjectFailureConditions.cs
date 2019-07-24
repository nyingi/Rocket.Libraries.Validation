using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation.FailureRules
{
    public static class ObjectFailureConditions
    {
        public static bool IsNull(object obj)
        {
            return obj == null;
        }
    }
}