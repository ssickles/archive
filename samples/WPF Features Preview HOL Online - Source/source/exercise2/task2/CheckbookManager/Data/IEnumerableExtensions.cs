namespace CheckbookManager.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// LINQ extensions for IEnumerable
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// ForEach extension
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var e in ie)
                action(e);
        }
    }
}
