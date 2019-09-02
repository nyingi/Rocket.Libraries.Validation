using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.Validation
{
    public static class NumberFailureConditions
    {
        /// <summary>
        /// Verifies whether or not user value is greater than a specified number
        /// </summary>
        /// <param name="userValue">The user's value</param>
        /// <param name="compareValue">The number to check against</param>
        /// <returns>True if <paramref name="userValue"/> is greater than <paramref name="compareValue"/> or false otherwise</returns>
        public static bool IsGreaterThan(double userValue, double compareValue)
        {
            return userValue > compareValue;
        }

        /// <summary>
        /// Verifies whether or not user value is less than a specified number
        /// </summary>
        /// <param name="userValue">The user's value</param>
        /// <param name="compareValue">The number to check against</param>
        /// <returns>True if <paramref name="userValue"/> is less than <paramref name="compareValue"/> or false otherwise</returns>
        public static bool IsLessThan(double userValue, double compareValue)
        {
            return userValue < compareValue;
        }

        /// <summary>
        /// Verifies whether or not user value is equal to a specified number
        /// </summary>
        /// <param name="userValue">The user's value</param>
        /// <param name="compareValue">The number to check against</param>
        /// <returns>True if <paramref name="userValue"/> is equal to <paramref name="compareValue"/> or false otherwise</returns>
        public static bool IsEqualTo(double userValue, double compareValue)
        {
            return userValue == compareValue;
        }

        /// <summary>
        /// Verifies whether or not user value is not equal to a specified number
        /// </summary>
        /// <param name="userValue">The user's value</param>
        /// <param name="compareValue">The number to check against</param>
        /// <returns>True if <paramref name="userValue"/> is not equal to <paramref name="compareValue"/> or false otherwise</returns>
        public static bool IsNotEqualTo(double userValue, double compareValue)
        {
            return !IsEqualTo(userValue, compareValue);
        }
    }
}