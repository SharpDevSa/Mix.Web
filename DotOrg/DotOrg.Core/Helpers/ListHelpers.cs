using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotOrg.Core.Helpers
{
	public static class ListHelpers
	{
		public static TOut GetValueOrDefault<TIn, TOut>(this Dictionary<TIn, TOut> dictionay, TIn key)
		{
			if (dictionay.ContainsKey(key)) return dictionay[key];
			return default(TOut);
		}

		public static IEnumerable<IList<T>> TakeBy<T>(this IEnumerable<T> list, int count)
		{
			var index = 0;
			return list.GroupBy(item => index++ / count).Select(g => g.ToList());
		}

        /// <summary>
        /// Равномерно разбивает элементы по колонкам.
        /// </summary>
        /// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
        /// <param name="count">Количество колонок</param>
        /// <returns></returns>
        public static IEnumerable<IList<T>> DivideBy<T>(this IEnumerable<T> list, int count)
        {
			var listLength = list.Count();
			var rest = listLength % count;
			var minColLength = listLength / count;
			var skip = 0;
			for (var i = 0; i < count; i++)
			{
				var colLength = minColLength + Math.Sign(rest);
				yield return list.Skip(skip).Take(colLength).ToList();
				skip += colLength;
				if (rest > 0)
					rest--;
			}
			//var list = enumerable.ToList();
			//var minItems = list.Count/count;
			//var remainder = list.Count%count;
			//var groupSizes = new int[count];
			//for (var i = 0; i < count; i++)
			//{
			//	groupSizes[i] = minItems;
			//	if (i < remainder)
			//		groupSizes[i]++;
			//}

			//var result = new List<IList<T>>();
			//var returnedCount = 0;
			//for (var i = 0; i < count; i++)
			//{
			//	var group = new List<T>(list.Skip(returnedCount).Take(groupSizes[i]));
			//	result.Add(group);
			//	returnedCount += groupSizes[i];
			//}
			//return result;
        }

		public static IEnumerable<TOut> GetOf<TIn, TOut>(this IList<TIn> list) where TOut : TIn
		{
			return list.Where(source => source is TOut).Cast<TOut>();
		}

		public static IEnumerable<TIn> GetNotOf<TIn, TOut>(this IList<TIn> list) where TOut : TIn
		{
			return list.Where(source => !(source is TOut));
		}

		public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> enumerable, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
		{
			return enumerable.Except(second, new LambdaComparer<TSource>(comparer));
		}

		public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> enumerable, Func<TSource, TSource, bool> comparer)
		{
			return enumerable.Distinct(new LambdaComparer<TSource>(comparer));
		}

		private class LambdaComparer<T> : IEqualityComparer<T>
		{
			private readonly Func<T, T, bool> _lambdaComparer;
			private readonly Func<T, int> _lambdaHash;

			public LambdaComparer(Func<T, T, bool> lambdaComparer) :
				this(lambdaComparer, o => 0)
			{
			}

			private LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
			{
				_lambdaComparer = lambdaComparer;
				_lambdaHash = lambdaHash;
			}

			public bool Equals(T x, T y)
			{
				return _lambdaComparer(x, y);
			}

			public int GetHashCode(T obj)
			{
				return _lambdaHash(obj);
			}
		}
	}
}
