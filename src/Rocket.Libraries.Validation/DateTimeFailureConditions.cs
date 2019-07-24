using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation
{
    public class DateTimeFailureConditions
    {
        public static bool IsFutureDate(DateTime userValue)
        {
            return userValue > DateTime.Now;
        }

        public static bool IsBefore(DateTime userValue, DateTime compareValue)
        {
            return userValue < compareValue;
        }

        public static bool IsAfter(DateTime userValue, DateTime compareValue)
        {
            return userValue > compareValue;
        }
    }
}