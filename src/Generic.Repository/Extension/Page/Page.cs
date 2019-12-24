using Generic.Repository.Cache;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Extension.Page
{
    /// <summary>
    /// Extension method to paginate entity TValue
    /// </summary>
    public static class Page
    {
        #region Page<TValue>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IPage<TValue> ToPage<TValue>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config
            )
            where TValue : class =>
            new Page<TValue>(
                cacheRepository,
                listEntities,
                config);

        #endregion Page<TValue>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static IPage<TResult> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping)
            where TValue : class
            where TResult : class =>
            new Page<TValue, TResult>(
                cacheRepository,
                listEntities,
                config,
                mapping);

        #region Page<TValue, TFilter>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IPage<TValue> ToPageFiltered<TValue, TFilter>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config)
            where TValue : class
            where TFilter : class, IFilter =>
            new PageFiltered<TValue, TFilter>(
                cacheRepository,
                listEntities,
                config);

        #endregion Page<TValue, TFilter>

        #region Page<TValue, TFilter, TResult>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="mapperTo">The mapper to.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IPage<TResult> ToPageFiltered<TValue, TFilter, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            IPageConfig config)
            where TValue : class
            where TResult : class
            where TFilter : class, IFilter =>
                new PageFiltered<TValue, TFilter, TResult>(
                    cacheRepository,
                    listEntities,
                    mapping,
                    config);

        #endregion Page<TValue, TFilter, TResult>
    }
}