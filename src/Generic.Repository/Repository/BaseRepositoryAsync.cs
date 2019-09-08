using Generic.Repository.Cache;
using Generic.Repository.Extension.Error;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryAsync<TValue, TFilter> : IBaseRepositoryAsync<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        #region Attr

        public IList<string> includesString { get; set; } = new List<string>();

        public IList<Expression<Func<TValue, object>>> includesExp { get; set; } =
            new List<Expression<Func<TValue, object>>>();

        protected readonly ICacheRepository CacheService;

        protected readonly DbContext Context;

        protected readonly bool UseCommit;

        #endregion

        #region Ctor

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context)
        {
            CacheService = cacheService;
            Context = context;
            StartCache();
        }

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context,
            bool useCommit)
        {
            CacheService = cacheService;
            UseCommit = useCommit;
            Context = context;
            StartCache();
        }

        #endregion

        #region QUERY

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(bool enableAsNoTracking) =>
            await GetAllQueryable(enableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
            predicate != null
                ? await GetAllQueryable(enableAsNoTracking).Where(predicate).ToListAsync()
                : await GetAllQueryable(enableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> FilterAllAsync(TFilter filter, bool enableAsNoTracking) =>
            await GetAllByAsync(filter.GeneratePredicate<TValue, TFilter>(CacheService), enableAsNoTracking);

        public virtual async Task<TValue> GetSingleByAsync(Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            predicate.ThrowErrorNullValue(nameof(predicate), nameof(GetSingleByAsync));
            return await GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TValue> GetFirstByAsync(Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            predicate.ThrowErrorNullValue(nameof(predicate), nameof(GetFirstByAsync));
            return await GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
        await Task.Run(() => GetAllQueryable(enableAsNoTracking).ToPage<TValue>(CacheService, config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
         await Task.Run(() => GetAllQueryable(enableAsNoTracking)
         .Where(filter.GeneratePredicate<TValue, TFilter>(CacheService))
         .ToPage<TValue>(CacheService, config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
        await Task.Run(() => GetAllQueryable(enableAsNoTracking)
        .Where(predicate)
        .ToPage<TValue>(CacheService, config));

        public virtual async Task<int> CountAsync(Expression<Func<TValue, bool>> predicate)
        {
            predicate.ThrowErrorNullValue(nameof(predicate), nameof(GetSingleByAsync));
            return await GetAllQueryable(true).CountAsync(predicate);
        }

        public virtual async Task<int> CountAsync() => await GetAllQueryable(true).CountAsync();

        #endregion

        #region Includes Methods
        public void AddInclude(string include) => includesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) => includesExp.Add(predicate);
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity, CancellationToken token)
        {
            entity.ThrowErrorNullValue(nameof(entity), nameof(CreateAsync));
            SetState(EntityState.Added, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.ThrowErrorNullValue(nameof(entityList), nameof(CreateAsync));
            await Context.AddRangeAsync(entityList);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }

        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            entity.ThrowErrorNullValue(nameof(entity), nameof(UpdateAsync));
            SetState(EntityState.Modified, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.ThrowErrorNullOrEmptyList(nameof(UpdateAsync));
            Context.UpdateRange(entityList);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            entity.ThrowErrorNullValue(nameof(entity), nameof(DeleteAsync));
            Context.Remove(entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.ThrowErrorNullOrEmptyList(nameof(DeleteAsync));

            Context.RemoveRange(entityList);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }

        }
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) Without CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity) =>
        await CreateAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList) =>
        await CreateAsync(entityList, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task UpdateAsync(TValue entity) =>
        await UpdateAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList) =>
        await UpdateAsync(entityList, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task DeleteAsync(TValue entity) =>
        await DeleteAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList) =>
        await DeleteAsync(entityList, default(CancellationToken)).ConfigureAwait(false);
        #endregion

        #region COMMIT
        public Task SaveChangesAsync() => SaveChangesAsync(default);

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
         Context.SaveChangesAsync(cancellationToken);
        #endregion

        #region Protected Methods
        protected IQueryable<TValue> SetIncludes(IQueryable<TValue> query) =>
        includesString != null && includesString.Any() ?
                includesString.Aggregate(query, (current, include) =>
                current.Include(include)) : includesExp != null && includesExp.Any() ?
                includesExp.Aggregate(query, (current, include) =>
                current.Include(include)) : query;

        protected IQueryable<TValue> GetAllQueryable(bool enableAsNoTracking)
        {
            var query = SetIncludes(Context.Set<TValue>());
            if (enableAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        protected void StartCache()
        {
            CacheService.Add<TValue>();
            CacheService.Add<TFilter>();
        }

        protected void SetState(EntityState state, TValue item) => Context.Attach(item).State = state;
        #endregion
    }
}