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
    public abstract class QueryAsync<TValue, TContext> : UnitOfWorkAsync<TContext>
        where TValue : class
        where TContext : DbContext
    {
        /// <summary>
        /// The cache service
        /// </summary>
        protected readonly ICacheRepository CacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryAsync{TValue, TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cacheService">The cache service.</param>
        protected QueryAsync(
                            TContext context,
                            ICacheRepository cacheService) :
            base(context)
        {
            ThrowErrorIf.
                HasNoCache(cacheService, typeof(QueryAsync<,>).Name);

            CacheService = cacheService;

            Query = Context.
                Set<TValue>().
                AsQueryable();

            IncludesExp = new List<Expression<Func<TValue, object>>>();

            IncludesString = new List<string>();
        }

        /// <summary>
        /// Gets or sets the includes exp.
        /// </summary>
        /// <value>
        /// The includes exp.
        /// </value>
        public IList<Expression<Func<TValue, object>>> IncludesExp { get; set; }

        /// <summary>
        /// Gets or sets the includes string.
        /// </summary>
        /// <value>
        /// The includes string.
        /// </value>
        public IList<string> IncludesString { get; set; }

        /// <summary>
        /// Gets or sets the query.
        /// </summary>
        /// <value>
        /// The query.
        /// </value>
        protected IQueryable<TValue> Query { get; set; }

        #region QUERY

        /// <summary>
        /// Counts the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(
                                                Expression<Func<TValue, bool>> predicate,
                                                CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(
                predicate,
                nameof(predicate),
                nameof(CountAsync));

            CreateQuery(true, token);

            return await Query.
                CountAsync(predicate, token).
                ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync(
                                                CancellationToken token)
        {
            CreateQuery(true, token);

            return await Query.
                CountAsync(token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Finds the asynchronous.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public virtual async Task<TValue> FindAsync(
                                                  params object[] parameters)
        {
            ThrowErrorIf.
                IsNullValue(parameters, nameof(parameters), nameof(FindAsync));

            return await Context.
                FindAsync<TValue>(parameters).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual Task<IReadOnlyList<TValue>> GetAllAsync(
                                                            bool notTracking,
                                                            CancellationToken token)
        {
            return CreateList(notTracking, token);
        }

        /// <summary>
        /// Gets all by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual Task<IReadOnlyList<TValue>> GetAllByAsync(
                                                                Expression<Func<TValue, bool>> predicate,
                                                                bool notTracking,
                                                                CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(
                    predicate,
                    nameof(predicate),
                    nameof(GetSingleOrDefaultAsync));

            return CreateListFiltered(predicate, notTracking, token);
        }

        /// <summary>
        /// Gets the first by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual async Task<TValue> GetFirstOrDefaultAsync(
                                                                Expression<Func<TValue, bool>> predicate,
                                                                bool notTracking,
                                                                CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(
                    predicate,
                    nameof(predicate),
                    nameof(GetFirstOrDefaultAsync));

            CreateQuery(notTracking, token);

            return await Query.
                FirstOrDefaultAsync(predicate, token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the single by asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual async Task<TValue> GetSingleOrDefaultAsync(
                                                                Expression<Func<TValue, bool>> predicate,
                                                                bool notTracking,
                                                                CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(
                    predicate,
                    nameof(predicate),
                    nameof(GetSingleOrDefaultAsync));

            CreateQuery(notTracking, token);

            return await Query.
                SingleOrDefaultAsync(predicate, token).
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

        /// <summary>
        /// Creates the list.
        /// </summary>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        internal async Task<IReadOnlyList<TValue>> CreateList(
                                                            bool notTracking,
                                                            CancellationToken token)
        {
            CreateQuery(notTracking, token);

            return await Query.
                ToListAsync(token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the list filtered.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        internal async Task<IReadOnlyList<TValue>> CreateListFiltered(
                                                                    Expression<Func<TValue, bool>> predicate,
                                                                    bool notTracking,
                                                                    CancellationToken token)
        {
            CreateQueryFiltered(
                   predicate,
                   notTracking,
                   token);

            return await Query.
                ToListAsync(token).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Creates the query.
        /// </summary>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        internal void CreateQuery(
                                bool notTracking,
                                CancellationToken token)
        {
            InitializeCache<TValue>(token);

            if (notTracking)
            {
                Query = Query.AsNoTracking();
            }

            Query = SetIncludes(Query);
        }

        /// <summary>
        /// Creates the query filtered.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        internal void CreateQueryFiltered(
                                        Expression<Func<TValue, bool>> predicate,
                                        bool notTracking,
                                        CancellationToken token)
        {
            CreateQuery(notTracking, token);

            Query = Query.Where(predicate);
        }

        /// <summary>
        /// Initializes the cache.
        /// </summary>
        /// <param name="token">The token.</param>
        protected virtual void InitializeCache<TCacheValue>(
                                                    CancellationToken token)
        {
            Parallel.Invoke(
                () => CacheService.AddGet<TCacheValue>(token),
                () => CacheService.AddSet<TCacheValue>(token),
                () => CacheService.AddProperty<TCacheValue>(token),
                () => CacheService.AddAttribute<TCacheValue>(token));
        }

        #endregion INTERNAL - SET QUERY
    }
}
