using Generic.Repository.Cache;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.Validations.ThrowError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public class QueryAsync<TValue, TContext>
        where TValue : class
        where TContext : DbContext
    {
        protected readonly ICacheRepository CacheService;

        protected readonly TContext Context;

        public QueryAsync(TContext context, ICacheRepository cacheService)
        {
            ThrowErrorIf.
                InitializeCache(cacheService);

            ThrowErrorIf.
                IsNullValue(context, nameof(context), typeof(QueryAsync<,>).Name);

            Context = context;
            CacheService = cacheService;
            Query = Context.Set<TValue>();
        }

        public IList<Expression<Func<TValue, object>>> IncludesExp { get; set; } =
            new List<Expression<Func<TValue, object>>>();

        public IList<string> IncludesString { get; set; } = new List<string>();

        protected IQueryable<TValue> Query { get; set; }

        internal virtual async Task<BaseRepositoryFacade<TValue>> GetRepositoryFacade(
            bool enableAsNotTracking,
            CancellationToken token) =>
                await BaseRepositoryFacade<TValue>.
                    Initializer(CacheService, token).
                        ConfigureAwait(false);

        #region QUERY

        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(CountAsync));

            CreateQuery(true);

            return await Query.CountAsync(predicate, token).
                ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(CancellationToken token)
        {
            CreateQuery(true);

            return await Query.CountAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task<TValue> FindAsync(
            params object[] parameters)
        {
            ThrowErrorIf.
                IsNullValue(parameters, nameof(parameters), nameof(FindAsync));

            return await Context.FindAsync<TValue>(parameters).
                ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(
            bool enableAsNotTracking,
            CancellationToken token)
        {
            CreateQuery(enableAsNotTracking);

            return await Query.ToListAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.Where(predicate).ToListAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.FirstOrDefaultAsync(predicate, token).
                ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                ConfigureAwait(false);

            CreateQuery(enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, token).
                ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(config, nameof(config), nameof(GetPageAsync));

            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token).
                ConfigureAwait(false);

            CreateQueryFiltered(predicate, enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, token).
                ConfigureAwait(false);
        }

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.SingleOrDefaultAsync(predicate, token).
                ConfigureAwait(false);
        }

        #endregion QUERY

        #region INCLUDES METHODS

        public void AddInclude(string include) =>
            IncludesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) =>
            IncludesExp.Add(predicate);

        internal IQueryable<TValue> SetIncludes(IQueryable<TValue> query)
        {
            query = IncludesString.
                Aggregate(query, (current, include) => current.Include(include));

            query = IncludesExp.
                Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        #endregion INCLUDES METHODS

        #region INTERNAL

        internal async Task<IReadOnlyList<TValue>> CreateList(
            bool enableAsNotTracking)
        {
            CreateQuery(enableAsNotTracking);

            return await Query.
                ToListAsync().
                ConfigureAwait(false);
        }

        internal async Task<IReadOnlyList<TValue>> CreateListFiltered(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking)
        {
            CreateQueryFiltered(predicate, enableAsNotTracking);

            return await Query.
                ToListAsync().
                ConfigureAwait(false);
        }

        internal void CreateQuery(
                            bool enableAsNotTracking)
        {
            Query = SetIncludes(Query);

            if (enableAsNotTracking)
            {
                Query = Query.AsNoTracking();
            }
        }

        internal void CreateQueryFiltered(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking)
        {
            CreateQuery(enableAsNotTracking);

            Query = Query.Where(predicate);
        }

        #endregion INTERNAL
    }
}