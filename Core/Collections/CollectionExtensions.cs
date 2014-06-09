using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Collections
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> FilterMonotonicallyIncreasing<T>(this IEnumerable<T> items)
            where T : IComparable<T>
        {
            return FilterMonotonicallyIncreasingBy<T, T>(items, x => x);
        }

        public static IEnumerable<T> FilterMonotonicallyIncreasingBy<T, TKey>(this IEnumerable<T> items, Func<T,TKey> keyExtractor)
            where TKey : IComparable<TKey>
        {
            return FilterMonotonicallyIncreasingBy<T, TKey>(items, keyExtractor, Comparer<TKey>.Default);
        }

        public static IEnumerable<T> FilterMonotonicallyIncreasingBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keyExtractor, IComparer<TKey> comparer)
        {
            bool first = true;
            TKey incumbentKey = default(TKey);

            foreach (var item in items)
            {
                var key = keyExtractor(item);

                if (first)
                {
                    first = false;
                    incumbentKey = key;
                    yield return item;
                }
                else
                {
                    if (comparer.Compare(incumbentKey, key) < 0)
                    {
                        incumbentKey = key;
                        yield return item;
                    }
                }
            }
        }
    }
}
