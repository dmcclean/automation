using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Mathematics;
using AutomationLibrary.Mathematics.Geometry;

namespace AutomationLibrary.Collections
{
    public static class CollectionExtensions
    {
        public static T FirstOr<T>(this IEnumerable<T> source, T alternate)
        {
            foreach (T t in source)
                return t;
            return alternate;
        }

        public static IEnumerable<T> After<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            bool haveSeen = false;
            foreach (var item in source)
            {
                if (haveSeen) yield return item;
                else if (predicate(item)) haveSeen = true;
            }
        }

        public static IEnumerable<Tuple<T, T>> AsPairs<T>(this IEnumerable<T> source)
        {
            var e = source.GetEnumerator();
            if (!e.MoveNext()) yield break;
            var prior = e.Current;
            while (e.MoveNext())
            {
                var current = e.Current;
                yield return Tuple.Create(prior, current);
                prior = current;
            }
        }

        public static IEnumerable<LineSegment2> AsLineSegments(this IEnumerable<Vector2> points)
        {
            foreach(var pair in points.AsPairs())
            {
                yield return LineSegment2.FromOriginAndDestination(pair.Item1, pair.Item2);
            }
        }

        public static IEnumerable<T> Before<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item)) yield break;
                else yield return item;
            }
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        public static IEnumerable<T> SkipBothEnds<T>(this IEnumerable<T> items, int n)
        {
            var queue = new Queue<T>(n);
            foreach (var item in items.Skip(n))
            {
                queue.Enqueue(item);
                if (queue.Count > n) yield return queue.Dequeue();
            }
        }

        public static void PlaceInAscendingOrder<T>(ref T a, ref T b)
            where T : IComparable<T>
        {
            if (b.CompareTo(a) < 0) Swap<T>(ref a, ref b);
        }

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
