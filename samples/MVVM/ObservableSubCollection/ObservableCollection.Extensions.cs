using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CollectionTest
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                coll.Add(item);
            }
        }

        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable items)
        {
            foreach (T item in items)
            {
                coll.Add(item);
            }
        }

        public static void RemoveRange<T>(this ObservableCollection<T> coll, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                coll.Remove(item);
            }
        }

        public static void RemoveRange<T>(this ObservableCollection<T> coll, IEnumerable items)
        {
            foreach (T item in items)
            {
                coll.Remove(item);
            }
        }
    }
}
