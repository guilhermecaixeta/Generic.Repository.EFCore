using Generic.Repository.Cache;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public static async Task<IPage<TValue>> ToPage<TValue>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config,
            CancellationToken token)
            where TValue : class =>
                await new Page<TValue>(
                    cacheRepository,
                    listEntities,
                    config).Init(token).
                            ConfigureAwait(false);

        #endregion Page<TValue>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns></returns>
        public static async Task<IPage<TResult>> ToPage<TValue, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            CancellationToken token)
            where TValue : class
            where TResult : class =>
                await new Page<TValue, TResult>(
                    cacheRepository,
                    listEntities,
                    config,
                    mapping).Init(token).
                        ConfigureAwait(false);

        #region Page<TValue, TFilter>

        /// <summary>Converts to page.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="listEntities">The list entities.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static async Task<IPage<TValue>> ToPageFiltered<TValue, TFilter>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            IPageConfig config,
            CancellationToken token)
            where TValue : class
            where TFilter : class, IFilter =>
                await new PageFiltered<TValue, TFilter>(
                    cacheRepository,
                    listEntities,
                    config).Init(token).
                        ConfigureAwait(false);

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
        public static async Task<IPage<TResult>> ToPageFiltered<TValue, TFilter, TResult>(
            this IQueryable<TValue> listEntities,
            ICacheRepository cacheRepository,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            IPageConfig config,
            CancellationToken token)
            where TValue : class
            where TResult : class
            where TFilter : class, IFilter =>
                await new PageFiltered<TValue, TFilter, TResult>(
                    cacheRepository,
                    listEntities,
                    mapping,
                    config).Init(token).
                        ConfigureAwait(false);

        #endregion Page<TValue, TFilter, TResult>
    }
}