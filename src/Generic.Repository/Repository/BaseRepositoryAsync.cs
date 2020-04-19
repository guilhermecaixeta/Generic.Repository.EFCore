using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryAsync<TValue, TFilter, TContext> :
            Pageable<TValue, TFilter, TContext>,
            IBaseRepositoryAsync<TValue, TFilter, TContext>
        where TValue : class
        where TFilter : class, IFilter
        where TContext : DbContext
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepositoryAsync{TValue, TFilter, TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cacheService">The cache service.</param>
        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) :
            base(context, cacheService)
        {
        }

        #endregion Ctor

        #region INTERNALS METHODS

        /// <summary>
        /// Creates the query filtered.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        internal async Task CreateQueryFiltered(
                                            TFilter filter,
                                            bool notTracking,
                                            CancellationToken token)
        {
            var predicate = await filter.
                CreateGenericFilter<TValue, TFilter>(CacheService, token).
                ConfigureAwait(false);

            CreateQueryFiltered(predicate, notTracking, token);
        }

        #endregion INTERNALS METHODS

        #region QUERIES

        /// <summary>
        /// Return all data Filtered
        /// </summary>
        /// <param name="filter">Filter to apply</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TValue>> FilterAllAsync(
                                                            TFilter filter,
                                                            bool notTracking,
                                                            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, notTracking, token).
                ConfigureAwait(false);

            return await CreateList(notTracking, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Return all data Filtered
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="filter">Filter to apply</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TReturn>> FilterAllAsync<TReturn>(
                                                                        TFilter filter,
                                                                        bool notTracking,
                                                                        Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                                        CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));

            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, notTracking, token).ConfigureAwait(false);

            var list = await CreateList(notTracking, token).ConfigureAwait(false);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
                                                                    bool notTracking,
                                                                    Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                                    CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            var list = await CreateList(notTracking, token).ConfigureAwait(false);

            return mapper(list).ToList();
        }

        /// <summary>
        /// Return all data from predicate informed
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="predicate">Condition to apply on data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
                                                                    bool notTracking,
                                                                    Expression<Func<TValue, bool>> predicate,
                                                                    Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                                    CancellationToken token)
        {
            var list = await CreateListFiltered(predicate, notTracking, token).
                ConfigureAwait(false);

            return mapper(list).ToList();
        }

        /// <summary>
        /// Return page.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
                                                            IPageConfig config,
                                                            bool notTracking,
                                                            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                            CancellationToken token)
            where TReturn : class
        {
            CreateQuery(notTracking, token);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Return page Filtered.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="filter">Filter data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
                                                            IPageConfig config,
                                                            bool notTracking,
                                                            TFilter filter,
                                                            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                            CancellationToken token)
            where TReturn : class
        {
            await CreateQueryFiltered(filter, notTracking, token).ConfigureAwait(false);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Return page Filtered.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="predicate">Predicate to filter data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
                                                            IPageConfig config,
                                                            bool notTracking,
                                                            Expression<Func<TValue, bool>> predicate,
                                                            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                            CancellationToken token)
        where TReturn : class
        {
            CreateQueryFiltered(predicate, notTracking, token);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        #endregion QUERIES

        #region OVERRIDE

        protected override void InitializeCache<TCacheValue>(CancellationToken token)
        {
            base.InitializeCache<TFilter>(token);

            base.InitializeCache<TCacheValue>(token);
        }

        #endregion OVERRIDE
    }

    public class BaseRepositoryAsync<TValue, TContext> :
        Pageable<TValue, TContext>, IBaseRepositoryAsync<TValue, TContext>
        where TContext : DbContext
        where TValue : class
    {
        #region CTOR

        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) :
            base(context, cacheService)
        {
        }

        #endregion CTOR

        #region QUERY

        /// <summary>
        /// Return all data
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
                                                                    bool notTracking,
                                                                    Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                                    CancellationToken token)
            where TReturn : class
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetAllAsync));

            var list = await CreateList(notTracking, token).ConfigureAwait(false);

            return mapper(list).ToList();
        }

        /// <summary>
        /// Return all data from predicate informed
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="predicate">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
                                                                    Expression<Func<TValue, bool>> predicate,
                                                                    bool notTracking,
                                                                    Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                                    CancellationToken token)
            where TReturn : class
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            var list = await CreateListFiltered(predicate, notTracking, token).ConfigureAwait(false);

            return mapper(list).ToList();
        }

        /// <summary>
        /// Return page.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
                                                            IPageConfig config,
                                                            bool notTracking,
                                                            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                            CancellationToken token)
        where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));

            CreateQuery(notTracking, token);

            return await Query.ToPage(CacheService, config, mapper, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Return page Filtered.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <param name="predicate">Predicate to filter data</param>
        /// <param name="mapper"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
                                                            IPageConfig config,
                                                            bool notTracking,
                                                            Expression<Func<TValue, bool>> predicate,
                                                            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
                                                            CancellationToken token)
        where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            CreateQueryFiltered(predicate, notTracking, token);

            return await Query.ToPage(CacheService, config, mapper, token).ConfigureAwait(false);
        }

        #endregion QUERY
    }
}