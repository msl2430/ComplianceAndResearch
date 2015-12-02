using System;
using System.Collections.Generic;
using System.Linq;
using Robot.Core.Models;

namespace Robot.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<int> ToListOfIntegers(this IEnumerable<string> listOfStrings)
        {
            try
            {
                return listOfStrings.Select(x => Convert.ToInt32(x));
            }
            catch
            {
                return new List<int>();
            }
        }

        public static IEnumerable<string> ToListOfStrings(this IEnumerable<int> listOfIntegers)
        {
            return listOfIntegers.Select(x => x.ToString());
        }

        public static IEnumerable<string> ToStringList<T>(this IEnumerable<T> list)
        {
            foreach (object item in list)
            {
                yield return item.ToString();
            }
        }

        public static IEnumerable<T> DistinctBy<T, Y>(this IEnumerable<T> source, Func<T, Y> keySelector)
        {
            if (source == null)
            {
                yield return default(T);
                yield break;
            }

            HashSet<Y> keys = new HashSet<Y>();
            foreach (T item in source)
            {
                if (keys.Add(keySelector(item)))
                {
                    yield return item;
                }
            }
        }
        public static IList<IdNamePair> ToIdNamePair(this IEnumerable<object[]> self)
        {
            return self.Select(x => new IdNamePair(Convert.ToInt32(x[0]), Convert.ToString(x[1]))).ToList();
        }

        public static IEnumerable<TResult> SelectDistinct<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select(selector).Distinct();
        }

        public static IList<T> Insert<T>(this IList<T> array, T itemToAdd)
        {
            var objectList = array.ToList();
            objectList.Add(itemToAdd);
            return objectList.ToArray();
        }
    }
}
