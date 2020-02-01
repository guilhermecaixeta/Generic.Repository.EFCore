using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryAsync<TValue, TFilter, TContext> :
        Pageable<TValue, TFilter, TContext>, IBaseRepositoryAsync<TValue, TFilter, TContext>
        where TValue : class
        where TFilter : class, IFilter
        where TContext : DbContext
    {
        #region Ctor

        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) :
            base(context, cacheService)
        {
        }

        #endregion Ctor

        #region INTERNALS METHODS

        internal async Task CreateQueryFiltered(
            TFilter filter,
            bool notTracking,
            CancellationToken token)
        {
            var predicate = await filter.
                CreateGenericFilter<TValue, TFilter>(CacheService, token).
                ConfigureAwait(false);

            await CreateQueryFiltered(predicate, notTracking, token);
        }

        #endregion INTERNALS METHODS

        #region QUERIES

        public async Task<IReadOnlyList<TValue>> FilterAllAsync(
            TFilter filter,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, notTracking, token).ConfigureAwait(false);

            return await Query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TReturn>> FilterAllAsync<TReturn>(
            TFilter filter,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, notTracking, token).ConfigureAwait(false);

            var list = await CreateList(notTracking, token);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            var list = await CreateList(notTracking, token);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            bool notTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            await CreateQueryFiltered(predicate, notTracking, token);

            var list = await CreateList(notTracking, token);

            return mapper(list).ToList();
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            await CreateQuery(notTracking, token);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            TFilter filter,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            await CreateQueryFiltered(filter, notTracking, token).ConfigureAwait(false);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            await CreateQueryFiltered(predicate, notTracking, token);

            return await Query.
                ToPageFiltered<TValue, TFilter, TReturn>(CacheService, mapper, config, token).
                ConfigureAwait(false);
        }

        #endregion QUERIES

        #region OVERRIDE
        internal override async Task InitializeCache(CancellationToken token)
        {
            await CacheService.AddGet<TFilter>(token).
                ConfigureAwait(false);

            await CacheService.AddSet<TFilter>(token).
                ConfigureAwait(false);

            await CacheService.AddProperty<TFilter>(token).
                ConfigureAwait(false);

            await CacheService.AddAttribute<TFilter>(token).
                ConfigureAwait(false);

            await base.InitializeCache(token);
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
            ICacheRepository cacheService) : base(context, cacheService)
        {
        }

        #endregion CTOR

        #region QUERY

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

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));


            await CreateQuery(notTracking, token);

            return await Query.ToPage(CacheService, config, mapper, token).ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));

            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            await CreateQueryFiltered(predicate, notTracking, token);

            return await Query.ToPage(CacheService, config, mapper, token).ConfigureAwait(false);
        }

        #endregion QUERY
    }
}
