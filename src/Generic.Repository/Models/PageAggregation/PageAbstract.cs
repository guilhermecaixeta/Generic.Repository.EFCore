using Generic.Repository.Cache;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Models.PageAggregation
{
    public abstract class PageAbstract<TValue> : PageAttrAbstract<TValue, TValue>
    where TValue : class
    {
        protected PageAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config
        ) : base(listEntities, config, cacheRepository) { }
    }

    public abstract class PageAbstract<TValue, TResult> : PageAttrAbstract<TValue, TResult>
        where TValue : class
        where TResult : class
    {
        #region CONST AND READONLY

        /// <summary>The mapping</summary>
        protected readonly Func<IEnumerable<object>, IEnumerable<TResult>> Mapping;

        #endregion CONST AND READONLY

        #region CTOR

        protected PageAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping
        ) : base(listEntities, config, cacheRepository)
        {
            ThrowErrorIf.IsNullValue(mapping, nameof(mapping), typeof(PageAbstract<,>).Name);
            Mapping = mapping;
        }

        #endregion CTOR

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected new async Task<IReadOnlyList<TResult>> GetItems(CancellationToken token)
        {
            var result = await GetQueryable(token).ConfigureAwait(false);

            return Mapping(result).ToList();
        }
    }
}