namespace Rocket.Libraries.Validation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ListExtensions
    {
        public static bool TrueThat<TType>(this List<TType> list, Func<TType, bool> condition)
        {
            var occurenceCount = list.Count(condition);
            return occurenceCount > 0;
        }

        public static void ForEvery<TType>(this List<TType> list, Action<TType, int> iterator)
        {
            var counter = 0;
            list.ForEach(a =>
            {
                iterator(a, counter);
                counter++;
            });
        }
    }
}