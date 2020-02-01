using Generic.Repository.Cache;
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
    public abstract class QueryAsync<TValue, TContext>
        where TValue : class
        where TContext : DbContext
    {
        protected readonly ICacheRepository CacheService;

        protected readonly TContext Context;

        protected QueryAsync(TContext context, ICacheRepository cacheService)
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

        #region QUERY

        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(CountAsync));

            await CreateQuery(true, token).ConfigureAwait(false);

            return await Query.CountAsync(predicate, token).
                ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(CancellationToken token)
        {
            await CreateQuery(true, token).ConfigureAwait(false);

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
            bool notTracking,
            CancellationToken token)
        {
            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.ToListAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.Where(predicate).ToListAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.FirstOrDefaultAsync(predicate, token).
                ConfigureAwait(false);
        }

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            await CreateQuery(notTracking, token).ConfigureAwait(false);

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

        #region INTERNAL - SET QUERY

        internal async Task<IReadOnlyList<TValue>> CreateList(
            bool notTracking,
            CancellationToken token)
        {
            await CreateQuery(notTracking, token).ConfigureAwait(false);

            return await Query.
                ToListAsync(token).
                ConfigureAwait(false);
        }

        internal async Task<IReadOnlyList<TValue>> CreateListFiltered(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            await CreateQueryFiltered(predicate, notTracking, token).ConfigureAwait(false);

            return await Query.
                ToListAsync(token).
                ConfigureAwait(false);
        }

        internal async Task CreateQuery(
                            bool notTracking,
                            CancellationToken token)
        {
            await InitializeCache(token).ConfigureAwait(false);

            Query = SetIncludes(Query);

            if (notTracking)
            {
                Query = Query.AsNoTracking();
            }
        }

        internal async Task CreateQueryFiltered(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token)
        {
            await CreateQuery(notTracking, token).ConfigureAwait(false);

            Query = Query.Where(predicate);
        }

        internal virtual async Task InitializeCache(CancellationToken token)
        {
            await CacheService.AddGet<TValue>(token).
                ConfigureAwait(false);

            await CacheService.AddSet<TValue>(token).
                ConfigureAwait(false);

            await CacheService.AddProperty<TValue>(token).
                ConfigureAwait(false);

            await CacheService.AddAttribute<TValue>(token).
                ConfigureAwait(false);
        }

        #endregion INTERNAL - SET QUERY
    }
}