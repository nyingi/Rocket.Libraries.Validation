using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation
{
    public static class NumberFailureConditions
    {
        public static bool IsGreaterThan(double userValue, double compareValue)
        {
            return userValue > compareValue;
        }

        public static bool IsLessThan(double userValue, double compareValue)
        {
            return userValue < compareValue;
        }

        public static bool IsEqualTo(double userValue, double compareValue)
        {
            return userValue == compareValue;
        }
    }
}