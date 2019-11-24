using Generic.Repository.Cache;
using Generic.Repository.Models.Page;
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
        protected readonly TContext Context;

        protected readonly ICacheRepository CacheService;

        protected IQueryable<TValue> Query { get; set; }

        public IList<string> IncludesString { get; set; } = new List<string>();

        public IList<Expression<Func<TValue, object>>> IncludesExp { get; set; } =
            new List<Expression<Func<TValue, object>>>();

        public QueryAsync(TContext context, ICacheRepository cacheService)
        {
            ThrowErrorIf.InitializeCache(cacheService);
            ThrowErrorIf.IsNullValue(context, nameof(context), typeof(QueryAsync<,>).Name);

            Context = context;
            CacheService = cacheService;
            Query = Context.Set<TValue>();
        }

        protected virtual async Task<BaseRepositoryFacade<TValue>> GetRepositoryFacade(
            bool enableASNotTracking,
            CancellationToken token) =>
                await BaseRepositoryFacade<TValue>.
                Initializer(CacheService, token);

        #region QUERY

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(
            bool enableAsNotTracking,
            CancellationToken token)
        {
            CreateQuery(enableAsNotTracking);

            return await Query.ToListAsync(token);
        }

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.Where(predicate).ToListAsync(token);
        }

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.SingleOrDefaultAsync(predicate, token).ConfigureAwait(false);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            CreateQuery(enableAsNotTracking);

            return await Query.FirstOrDefaultAsync(predicate, token).ConfigureAwait(false);
        }

        public virtual async Task<TValue> FindAsync(
            params object[] parameters)
        {
            ThrowErrorIf.IsNullValue(parameters, nameof(parameters), nameof(FindAsync));

            return await Context.FindAsync<TValue>(parameters).ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token);

            CreateQuery(enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, token).ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));

            var repositoryFacade = await GetRepositoryFacade(enableAsNotTracking, token);
            
            CreateQueryFiltred(predicate, enableAsNotTracking);

            return await repositoryFacade.GetPage(Query, config, token).ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(CountAsync));

            CreateQuery(true);

            return await Query.CountAsync(predicate, token).ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(CancellationToken token)
        {
            CreateQuery(true);

            return await Query.CountAsync(token).ConfigureAwait(false);
        }
        #endregion

        #region Includes Methods
        public void AddInclude(string include) =>
            IncludesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) =>
            IncludesExp.Add(predicate);

        internal IQueryable<TValue> SetIncludes(IQueryable<TValue> query)
        {
            query = IncludesString.Aggregate(query, (current, include) => current.Include(include));

            query = IncludesExp.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        #endregion

        #region Internal

        internal void CreateQueryFiltred(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking)
        {
            CreateQuery(enableAsNotTracking);

            Query = Query.Where(predicate);
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

        internal async Task<IReadOnlyList<TValue>> CreatList(
            bool enableAsNotTracking)
        {
            CreateQuery(enableAsNotTracking);

            return await Query.
                ToListAsync().
                ConfigureAwait(false);
        }

        internal async Task<IReadOnlyList<TValue>> CreatList(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking)
        {
            CreateQueryFiltred(predicate, enableAsNotTracking);

            return await Query.
                ToListAsync().
                ConfigureAwait(false);
        }

        #endregion
    }
}
