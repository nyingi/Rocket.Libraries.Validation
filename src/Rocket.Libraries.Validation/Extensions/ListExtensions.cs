namespace Rocket.Libraries.Validation.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class ListExtensions
    {
        public static void MakeListEmpty<TType>(this List<TType> list)
        {
            list.MakeListEmpty(item => { });
        }

        public static void MakeListEmpty<TType>(this List<TType> list, Action<TType> cleaner)
        {
            while (list.Any())
            {
                cleaner(list[0]);
                list.RemoveAt(0);
            }
        }
    }
}