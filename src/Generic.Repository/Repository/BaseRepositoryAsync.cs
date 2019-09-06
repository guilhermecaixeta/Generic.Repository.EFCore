using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;
using Microsoft.EntityFrameworkCore;

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
            predicate.IsThrowNullError(nameof(GetSingleByAsync));
            return await GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TValue> GetFirstByAsync(Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            predicate.IsThrowNullError(nameof(GetFirstByAsync));
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

        public virtual async Task<int> CountAsync(Expression<Func<TValue, bool>> predicate) =>
        !predicate.IsNull(nameof(GetSingleByAsync), nameof(predicate)) ?
            await GetAllQueryable(true).CountAsync(predicate)
            : 0;

        public virtual async Task<int> CountAsync() => await GetAllQueryable(true).CountAsync();

        #endregion

        #region Includes Methods
        public void AddInclude(string include) => includesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) => includesExp.Add(predicate);
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity, CancellationToken token)
        {
            entity.IsThrowNullError(nameof(CreateAsync));
            SetState(EntityState.Added, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.IsThrowNullError(nameof(CreateAsync));
                await Context.AddRangeAsync(entityList);
                if (!UseCommit)
                {
                    await SaveChangesAsync(token).ConfigureAwait(false);
                }
            
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            entity.IsThrowNullError(nameof(UpdateAsync));
            SetState(EntityState.Modified, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            if (entityList.HasAny(nameof(DeleteAsync), nameof(entityList)))
            {
                Context.UpdateRange(entityList);
                if (!UseCommit)
                {
                    await SaveChangesAsync(token).ConfigureAwait(false);
                }
            }
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(DeleteAsync), nameof(entity));
            Context.Remove(entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            if (entityList.HasAny(nameof(DeleteAsync), nameof(entityList)))
            {
                Context.RemoveRange(entityList);
                if (!UseCommit)
                {
                    await SaveChangesAsync(token).ConfigureAwait(false);
                }
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
        public Task SaveChangesAsync() => SaveChangesAsync(default(CancellationToken));

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

    public class BaseRepositoryAsync<TValue, TResult, TFilter> :
    BaseRepositoryAsync<TValue, TFilter>, IBaseRepositoryAsync<TValue, TResult, TFilter>
    where TValue : class
    where TResult : class
    where TFilter : class, IFilter
    {
        #region CTOR
        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context, Func<IEnumerable<TValue>,
            IEnumerable<TResult>> mapperList,
            Func<TValue, TResult> mapperData)
         :
         base(cacheService, context)
        {
            if (IsValidCtor(mapperList, mapperData))
            {
                this.mapperList = mapperList;
                this.mapperData = mapperData;
            }
        }
        public BaseRepositoryAsync(
            ICacheRepository cacheRepository,
            DbContext context,
            bool useCommit,
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList,
            Func<TValue, TResult> mapperData)
         :
         base(
            cacheRepository,
            context,
            useCommit)
        {
            if (!IsValidCtor(mapperList, mapperData)) return;
            this.mapperList = mapperList;
            this.mapperData = mapperData;
        }

        private bool IsValidCtor(
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperListFunc,
            Func<TValue, TResult> mapperDataFunc)
        {
            mapperListFunc.IsThrowNullError(nameof(IsValidCtor));
            mapperDataFunc.IsThrowNullError(nameof(IsValidCtor));

            return true;
        }
        #endregion

        #region ATTRIBUTES
        public Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList { get; set; }
        public Func<TValue, TResult> mapperData { get; set; }
        #endregion

        #region QUERY
        public new virtual async Task<IReadOnlyList<TResult>> GetAllAsync(bool enableAsNoTracking)
            => mapperList(await GetAllQueryable(enableAsNoTracking).ToListAsync()).ToList();

        public new virtual async Task<IReadOnlyList<TResult>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                predicate != null
                ?
                mapperList(
                    await GetAllQueryable(enableAsNoTracking).
                    Where(predicate).
                    ToListAsync()).
                ToList()
                 :
                 mapperList(
                    await GetAllQueryable(enableAsNoTracking).
                    ToListAsync()).
                 ToList();

        public new virtual async Task<IReadOnlyList<TResult>> FilterAllAsync(
            TFilter filter,
            bool enableAsNoTracking) =>
            await GetAllByAsync(
                filter.
                GeneratePredicate<TValue, TFilter>(CacheService),
                enableAsNoTracking);

        public new virtual async Task<TResult> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            predicate.IsThrowNullError(nameof(GetSingleByAsync));
            var value = await GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
            return mapperData(value);
        }

        public new virtual async Task<TResult> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            predicate.IsThrowNullError(nameof(GetSingleByAsync));
            var value = await GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate);
            return mapperData(value);
        }

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
            await Task.
            Run(() =>
                GetAllQueryable(enableAsNoTracking).
                ToPage<TValue, TResult>(
                    CacheService,
                    mapperList,
                    config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
                await Task.
                Run(() =>
                GetAllQueryable(enableAsNoTracking).
                ToPage<TValue, TResult>(
                    CacheService,
                    mapperList,
                    config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                    GetAllQueryable(enableAsNoTracking).
                    ToPage<TValue, TResult>(
                        CacheService,
                        mapperList,
                        config));
        #endregion

    }
}