using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using Rocket.Libraries.Validation.Services;

namespace Rocket.Libraries.Validation
{
    public static class StringFailureConditions
    {
        public static bool ContainsNonAlphaNumericCharacters(string userValue)
        {
            FailIfTestValueEmpty(userValue);
            var alphaNumericRegEx = new Regex("^[a-zA-Z0-9]+$");
            return alphaNumericRegEx.IsMatch(userValue) == false;
        }

        public static bool ExceedsLength(string userValue, uint length)
        {
            FailIfTestValueEmpty(userValue);
            return userValue.Length > length;
        }

        public static bool ShorterThan(string userValue, uint length)
        {
            var userValueLength = string.IsNullOrEmpty(userValue) ? 0 : userValue.Length;
            return userValueLength < length;
        }

        public static bool LengthOutsideOfBounds(string userValue, uint minLength, uint maxLength)
        {
            return ExceedsLength(userValue, maxLength) || ShorterThan(userValue, minLength);
        }

        private static void FailIfTestValueEmpty(string value)
        {
            new DataValidator().EvaluateImmediate(string.IsNullOrEmpty(value), "Test value is empty, nothing to validate");
        }
    }
}