using Generic.Repository.Validations.ThrowError;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Extension.List
{
    public static class List
    {
        public static IEnumerable<IEnumerable<TValue>> SplitList<TValue>(this IEnumerable<TValue> list, int size)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(SplitList));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(size);

            var listSplited = list.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / size)
                .Select(x => x.Select(v => v.Value).ToList());

            return listSplited;
        }
    }
}