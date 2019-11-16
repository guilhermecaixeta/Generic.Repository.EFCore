using Generic.Repository.Cache;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Models.Page
{
    /// <summary>
    /// Page Class
    /// Default values:
    /// sort = ASC
    /// order = Id
    /// </summary>
    /// <typeparam name="TValue">Type of page return</typeparam>
    public class Page<TValue> : PageAbstract<TValue>
    where TValue : class
    {
        public Page(
        ICacheRepository cacheRepository,
        IQueryable<TValue> listEntities,
        IPageConfig config) :
        base(
        cacheRepository,
        listEntities,
        config)
        { }
    }

    public class Page<TValue, TResult> : PageAbstract<TValue, TResult>
    where TValue : class
    where TResult : class
    {
        public Page(
            ICacheRepository cacheRepository,
            IQueryable<TValue> listEntities,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping
        ) :
        base(
        cacheRepository,
        listEntities,
        config,
        mapping)
        { }
    }

    /// <summary>
    /// Page Class
    /// Default values:
    /// sort = ASC
    /// order = Id
    /// </summary>
    /// <typeparam name="TValue">Type of page return</typeparam>
    public class PageFiltred<TValue, TFilter> : PageFilterAbstract<TValue, TFilter>
    where TValue : class
    where TFilter : class, IFilter
    {
        public PageFiltred(
        ICacheRepository cacheRepository,
        IQueryable<TValue> listEntities,
        IPageConfig config
        ) :
        base(
        cacheRepository,
        listEntities,
        config)
        { }
    }

    public class PageFiltred<TValue, TFilter, TResult> : PageFilterAbstract<TValue, TFilter, TResult>
    where TValue : class
    where TFilter : class, IFilter
    where TResult : class
    {
        public PageFiltred(
                ICacheRepository cacheRepository,
                IQueryable<TValue> listEntities,
                Func<IEnumerable<object>, IEnumerable<TResult>> mapperTo,
                IPageConfig config
            ) :
            base(
                config,
                listEntities,
                cacheRepository,
                mapperTo
            )
        { }
    }
}
