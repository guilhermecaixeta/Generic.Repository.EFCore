using Generic.Repository.Cache;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Models.PageAggregation
{
    public abstract class PageFilterAbstract<TValue, TFilter, TResult> : PageAttrAbstract<TValue, TResult>
        where TValue : class
        where TFilter : class, IFilter
        where TResult : class
    {
        #region PARAMETERS CTOR

        /// <summary>The mapping</summary>
        protected Func<IEnumerable<object>, IEnumerable<TResult>> Mapping;

        #endregion PARAMETERS CTOR

        #region CTOR

        protected PageFilterAbstract(
            IPageConfig config,
            IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping
            ) : base(listEntities, config, cacheRepository)
        {
            StartCtor(config, listEntities, cacheRepository, mapping);
        }

        public void StartCtor(
        IPageConfig config,
        IQueryable<TValue> listEntities,
        ICacheRepository cacheRepository,
        Func<IEnumerable<object>, IEnumerable<TResult>> mapping)
        {
            ThrowErrorIf.IsNullValue(mapping, nameof(mapping), typeof(PageAbstract<,>).Name);
            Mapping = mapping;
        }

        #endregion CTOR

        /// <summary>Gets the items.</summary>
        /// <returns></returns>
        protected new async Task<IReadOnlyList<TResult>> GetItems(CancellationToken token)
        {
            var result = await GetQueryable(token).
                ConfigureAwait(false);

            return Mapping(result).ToList();
        }
    }

    public abstract class PageFilterAbstract<TValue, TFilter> : PageAttrAbstract<TValue, TValue>
        where TValue : class
        where TFilter : class, IFilter
    {
        #region CTOR

        protected PageFilterAbstract(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config
        ) : base(listEntities, config, cacheRepository)
        { }

        #endregion CTOR
    }
}