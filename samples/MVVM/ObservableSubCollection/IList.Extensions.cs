using System;
using System.Collections;
using System.Collections.Generic;

namespace CollectionTest
{
    public static class IListExtensions
    {
        public static IEnumerable<T> Where<T>(this IList list, Func<T, bool> filter)
        {
            foreach (T item in list)
            {
                if (filter.Invoke(item))
                    yield return item;
            }
        }
    }
}
