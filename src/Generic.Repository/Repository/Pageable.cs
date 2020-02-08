using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public abstract class Pageable<TValue, TContext> : CommandAsync<TValue, TContext>
        where TValue : class
        where TContext : DbContext
    {
        protected Pageable(TContext context, ICacheRepository cacheService) : base(context, cacheService)
        {
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.ToPage(CacheService, config, token).
                    ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            await CreateQueryFiltered(predicate, notTracking, token).ConfigureAwait(false);

            return await Query.ToPage(CacheService, config, token).
                    ConfigureAwait(false);
        }
    }

    public abstract class Pageable<TValue, TFilter, TContext> : Pageable<TValue, TContext>
        where TValue : class
        where TFilter : class, IFilter
        where TContext : DbContext
    {
        protected Pageable(TContext context, ICacheRepository cacheService) : base(context, cacheService)
        {
        }

        public override async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.ToPageFiltered<TValue, TFilter>(CacheService, config, token).
                    ConfigureAwait(false);
        }

        public override async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            await CreateQueryFiltered(predicate, notTracking, token).ConfigureAwait(false);

            return await Query.ToPageFiltered<TValue, TFilter>(CacheService, config, token).
                    ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.
                IsNullValue(filter, nameof(filter), nameof(GetPageAsync));

            var predicate = await filter.CreateGenericFilter<TValue, TFilter>(CacheService, token);

            await CreateQueryFiltered(predicate, notTracking, token).ConfigureAwait(false);

            return await Query.
                    ToPageFiltered<TValue, TFilter>(CacheService, config, token).
                    ConfigureAwait(false);
        }
    }
}