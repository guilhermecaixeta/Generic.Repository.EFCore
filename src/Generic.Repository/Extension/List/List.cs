using Generic.Repository.ThrowError;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Extension.List
{
    public static class List
    {
        /// <summary>
        /// Splits the list.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<TValue>> SplitList<TValue>(
            this IEnumerable<TValue> list, 
            int size)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(SplitList));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(size);

            var listSplited = list.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(x => x.Select(v => v.Value));

            return listSplited;
        }
    }
}
