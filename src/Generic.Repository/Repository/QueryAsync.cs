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

        protected readonly BaseRepositoryFacade<TValue> RepositoryFacade;

        public IList<string> IncludesString { get; set; } = new List<string>();

        public IList<Expression<Func<TValue, object>>> IncludesExp { get; set; } =
            new List<Expression<Func<TValue, object>>>();

        public QueryAsync(TContext context, ICacheRepository cacheService)
        {
            ThrowErrorIf.InitializeCache(cacheService);
            ThrowErrorIf.IsNullValue(context, nameof(context), typeof(QueryAsync<,>).Name);

            Context = context;
            CacheService = cacheService;
            RepositoryFacade = new BaseRepositoryFacade<TValue>(Context.Set<TValue>(), CacheService, SetIncludes);
            RepositoryFacade.StartCache().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #region QUERY

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(
            bool enableAsNoTracking,
            CancellationToken token) =>
            await RepositoryFacade.GetAllQueryable(enableAsNoTracking).ToListAsync(token);

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate).ToListAsync(token);
        }

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate, token);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate, token);
        }

        public virtual async Task<TValue> FindAsync(
            CancellationToken token,
            params object[] parameters)
        {
            ThrowErrorIf.IsNullValue(parameters, nameof(parameters), nameof(FindAsync));

            return await Context.FindAsync<TValue>(parameters).ConfigureAwait(false);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking,
            CancellationToken token) =>
                await RepositoryFacade.GetPage(RepositoryFacade.GetAllQueryable(enableAsNoTracking), config, token);

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking,
            CancellationToken token)
        {

            var listToPage = RepositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate);
            return await RepositoryFacade.GetPage(listToPage, config, token).ConfigureAwait(false);
        }
        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(CountAsync));

            return await RepositoryFacade.GetAllQueryable(true).CountAsync(predicate, token).ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(CancellationToken token) =>
            await RepositoryFacade.GetAllQueryable(true).CountAsync(token).ConfigureAwait(false);

        #endregion

        #region Includes Methods
        public void AddInclude(string include) =>
            IncludesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) =>
            IncludesExp.Add(predicate);

        #endregion

        internal IQueryable<TValue> SetIncludes(IQueryable<TValue> query)
        {
            query = IncludesString.Aggregate(query, (current, include) => current.Include(include));

            query = IncludesExp.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
