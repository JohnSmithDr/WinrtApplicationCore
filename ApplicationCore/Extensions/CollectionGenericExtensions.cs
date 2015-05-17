using System.Linq;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    public static class CollectionGenericExtentions
    {
        #region Add

        public static void Add<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        public static void Add<TSource>(this ICollection<TSource> source, params TSource[] items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

        #endregion

        #region AddTo

        public static TObj AddTo<TObj>(this TObj source, ICollection<TObj> collection)
        {
            collection.Add(source);
            return source;
        }

        public static TObj AddTo<TObj, TColl>(this TObj source, ICollection<TColl> collection, Func<TObj, TColl> convert)
        {
            collection.Add(convert.Invoke(source));
            return source;
        }

        public static TCollection AddTo<TCollection, TElement>(this TCollection source, ICollection<TElement> collection)
            where TCollection : IEnumerable<TElement>
        {
            foreach (var item in source)
            {
                collection.Add(item);
            }
            return source;
        }

        #endregion

        #region Remove

        public static void Remove<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Remove(item);
            }
        }

        public static void Remove<TSource>(this ICollection<TSource> source, Func<TSource, bool> predicate)
        {
            var items = source.Where(predicate).ToList();

            if (items.Any())
            {
                source.Remove(items);
            }
        }

        #endregion

        #region Replace

        public static void Replace<TSource>(this IList<TSource> source, TSource oldItem, TSource newItem)
        {
            var index = source.IndexOf(oldItem);

            if (index > 0)
            {
                source.RemoveAt(index);
                source.Insert(index, newItem);
            }
        }

        #endregion

        #region ForEach

        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        public static async Task ForEachAsync<TSource>(this IEnumerable<TSource> source, Func<TSource, Task> taskFactory)
        {
            foreach (var item in source)
            {
                await taskFactory.Invoke(item);
            }
        }

        #endregion

        #region Empty

        public static bool IsEmpty<TSource>(this ICollection<TSource> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool IsEmpty<TSource>(this IReadOnlyCollection<TSource> source)
        {
            return source == null || source.Count == 0;
        }

        public static bool IsNotEmpty<TSource>(this ICollection<TSource> source)
        {
            return source != null && source.Count > 0;
        }

        public static bool IsNotEmpty<TSource>(this IReadOnlyCollection<TSource> source)
        {
            return source != null && source.Count > 0;
        }

        #endregion
        
        #region SortBy

        public static void SortBy<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sorted = source.OrderBy(keySelector).ToList();

            source.Clear();
            source.Add(sorted);
        }

        public static void SortBy<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var sorted = source.OrderBy(keySelector, comparer).ToList();

            source.Clear();
            source.Add(sorted);
        }

        public static void SortByDescending<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            var sorted = source.OrderByDescending(keySelector).ToList();

            source.Clear();
            source.Add(sorted);
        }

        public static void SortByDescending<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            var sorted = source.OrderByDescending(keySelector, comparer).ToList();

            source.Clear();
            source.Add(sorted);
        }

        #endregion
    }
}
