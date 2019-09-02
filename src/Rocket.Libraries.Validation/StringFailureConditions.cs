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
        /// <summary>
        /// Checks if a value is an empty string.
        /// </summary>
        /// <param name="userValue">Value to check</param>
        /// <returns>True if the value is an empty string or false if it is not</returns>
        public static bool IsEmptyString(string userValue)
        {
            return string.IsNullOrEmpty(userValue);
        }

        /// <summary>
        /// Checks if a value can be parsed to a double.
        /// </summary>
        /// <param name="userValue">Value to attempt to parse</param>
        /// <returns>True if the value cannot be parsed as a valid double or false if it is indeed a valid double</returns>
        public static bool DoesNotParseToDoubleNumber(string userValue)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return true;
            }
            else
            {
                return double.TryParse(userValue, out _) == false;
            }
        }

        /// <summary>
        /// Checks if a value can be parsed to an integer.
        /// </summary>
        /// <param name="userValue">Value to attempt to parse</param>
        /// <returns>True if the value cannot be parsed as a valid integer or false if it is indeed a valid integer</returns>
        public static bool DoesNotParseToIntNumber(string userValue)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return true;
            }
            else
            {
                return long.TryParse(userValue, out _) == false;
            }
        }

        /// <summary>
        /// Checks that a value contains only alpha-numeric characters
        /// </summary>
        /// <param name="userValue">Value to check</param>
        /// <returns>True if non-alpha-numeric characters are found, or false if the entire string is composed of alpha-numeric characters only</returns>
        public static bool ContainsNonAlphaNumericCharacters(string userValue)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return true;
            }
            else
            {
                return FailsRegExFilter(userValue, "^[a-zA-Z0-9]+$");
            }
        }

        /// <summary>
        /// Checks that a value contains only alphabetic characters
        /// </summary>
        /// <param name="userValue">Value to check</param>
        /// <returns>True if non-alphabetic characters are found, or false if the entire string is composed of alphabetic characters only</returns>
        public static bool ContainsNonAlphabeticCharacters(string userValue)
        {
            return FailsRegExFilter(userValue, "^[a-zA-Z]+$");
        }

        /// <summary>
        /// Tests a user string against a specified RegEx filter
        /// </summary>
        /// <param name="userValue">The user string to validate</param>
        /// <param name="regEx">The RegEx filter of allowed characters</param>
        /// <returns>True if the user value fails the RegEx test of false if it passes</returns>
        public static bool FailsRegExFilter(string userValue, string regEx)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return true;
            }
            else
            {
                var filter = new Regex(regEx);
                return filter.IsMatch(userValue) == false;
            }
        }

        /// <summary>
        /// Checks that a string does not exceed as specified length
        /// </summary>
        /// <param name="userValue">The string to check</param>
        /// <param name="length">The length to verify against</param>
        /// <returns>True if the string exceeds the length or false if it doesn't</returns>
        public static bool ExceedsLength(string userValue, uint length)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return false;
            }
            else
            {
                return userValue.Length > length;
            }
        }

        /// <summary>
        /// Checks that a string is not shorter than a specified length
        /// </summary>
        /// <param name="userValue">The string to check</param>
        /// <param name="length">The length to verify against</param>
        /// <returns>True if the string is shorter than the length or false it isn't</returns>
        public static bool ShorterThan(string userValue, uint length)
        {
            var nothingToValidate = string.IsNullOrEmpty(userValue);
            if (nothingToValidate)
            {
                return true;
            }
            else
            {
                var userValueLength = string.IsNullOrEmpty(userValue) ? 0 : userValue.Length;
                return userValueLength < length;
            }
        }

        /// <summary>
        /// Checks that a string is not shorter than a specified length
        /// </summary>
        /// <param name="userValue">The string to check</param>
        /// <param name="minLength">The minimum length to verify against</param>
        /// <param name="maxLength">The maximum length to verify against</param>
        /// <returns>True if the string is shorter than the <paramref name="minLength"/> or longer than <paramref name="maxLength"/>, otherwise false</returns>
        public static bool LengthOutsideOfBounds(string userValue, uint minLength, uint maxLength)
        {
            return ExceedsLength(userValue, maxLength) || ShorterThan(userValue, minLength);
        }
    }
}