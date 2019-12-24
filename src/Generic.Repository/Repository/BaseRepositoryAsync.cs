using Generic.Repository.Models.PageAggregation;

namespace Generic.Repository.Repository
{
    using Generic.Repository.Cache;
    using Generic.Repository.Interfaces.Repository;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.PageAggregation.PageConfig;
    using Generic.Repository.Validations.ThrowError;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class BaseRepositoryAsync<TValue, TFilter, TContext> :
        CommandAsync<TValue, TContext>, IBaseRepositoryAsync<TValue, TFilter>
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

        #region Queries

        public async Task<IReadOnlyList<TValue>> FilterAllAsync(
            TFilter filter,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, enableAsNotTracking, token).
                                        ConfigureAwait(false);

            return await Query.ToListAsync().
                            ConfigureAwait(false);
        }

        public async Task<IReadOnlyList<TReturn>> FilterAllAsync<TReturn>(
            TFilter filter,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, enableAsNotTracking, token).
                                        ConfigureAwait(false);

            var list = await CreateList(enableAsNotTracking);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(FilterAllAsync));

            var list = await CreateList(enableAsNotTracking);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            bool enableAsNotTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            CreateQueryFiltered(predicate, enableAsNotTracking);

            var list = await CreateList(enableAsNotTracking);

            return mapper(list).ToList();
        }

        public async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(filter, nameof(filter), nameof(FilterAllAsync));
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(FilterAllAsync));

            await CreateQueryFiltered(filter, enableAsNotTracking, token).
                    ConfigureAwait(false);

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token);

            return await repositoryFacade.GetPage(Query, config, token).
                            ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                                            ConfigureAwait(false);

            CreateQuery(enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, mapper, token).
                            ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            TFilter filter,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                                            ConfigureAwait(false);

            await CreateQueryFiltered(filter, enableAsNotTracking, token).
                        ConfigureAwait(false);

            return await repositoryFacade.GetPage(Query, config, mapper, token).
                            ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                ConfigureAwait(false);

            CreateQueryFiltered(predicate, enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, mapper, token).
                        ConfigureAwait(false);
        }

        protected new async Task<BaseRepositoryFacade<TValue, TFilter>> GetRepositoryFacade(
            CancellationToken token) =>
                await BaseRepositoryFacade<TValue, TFilter>.
                    Initializer(CacheService, token);

        #endregion Queries

        #region Internals Methods

        internal async Task CreateQueryFiltered(
            TFilter filter,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            var repositoryFacade = await GetRepositoryFacade(token).
                        ConfigureAwait(false);

            var predicate = await repositoryFacade.GetExpressionByFilter(filter, token).
                                    ConfigureAwait(false);

            base.CreateQueryFiltered(predicate, enableAsNotTracking);
        }

        #endregion Internals Methods
    }

    public class BaseRepositoryAsync<TValue, TContext> :
        CommandAsync<TValue, TContext>, IBaseRepositoryAsync<TValue>
    where TContext : DbContext
    where TValue : class
    {
        #region Ctor

        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) : base(context, cacheService)
        {
        }

        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
            where TReturn : class
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetAllAsync));

            var list = await CreateList(enableAsNotTracking);

            return mapper(list).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
            where TReturn : class
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            var list = await CreateListFiltered(predicate, enableAsNotTracking).
                ConfigureAwait(false);

            return mapper(list).ToList();
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                                            ConfigureAwait(false);

            CreateQuery(enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, mapper, token).
                ConfigureAwait(false);
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                                            ConfigureAwait(false);

            CreateQueryFiltered(predicate, enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, mapper, token).
                ConfigureAwait(false);
        }

        #endregion Ctor
    }
}