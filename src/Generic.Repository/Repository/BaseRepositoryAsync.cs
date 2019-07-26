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
        public IList<string> includesString { get; set; } =
        new List<string>();
        public IList<Expression<Func<TValue, object>>> includesExp { get; set; } =
        new List<Expression<Func<TValue, object>>>();

        protected readonly ICacheRepository _cacheService;
        protected readonly DbContext _context;
        protected readonly bool _useCommit;

        #endregion

        #region Ctor
        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context)
        {
            _cacheService = cacheService;
            _context = context;
            StartCache();
        }

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context,
            bool useCommit)
        {
            _cacheService = cacheService;
            _useCommit = useCommit;
            _context = context;
            StartCache();
        }

        #endregion

        #region QUERY
        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(bool EnableAsNoTracking) =>
        await GetAllQueryable(EnableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) =>
        predicate != null ?
            await GetAllQueryable(EnableAsNoTracking).Where(predicate).ToListAsync() :
            await GetAllQueryable(EnableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> FilterAllAsync(TFilter filter, bool EnableAsNoTracking) =>
        await GetAllByAsync(filter.GenerateLambda<TValue, TFilter>(_cacheService), EnableAsNoTracking);

        public virtual async Task<TValue> GetSingleByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) =>
        !predicate.IsNull(nameof(GetSingleByAsync), nameof(predicate)) ? await GetAllQueryable(EnableAsNoTracking).SingleOrDefaultAsync(predicate) : null;

        public virtual async Task<TValue> GetFirstByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) =>
        !predicate.IsNull(nameof(GetSingleByAsync), nameof(predicate)) ? await GetAllQueryable(EnableAsNoTracking).FirstOrDefaultAsync(predicate) : null;

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config, 
            bool EnableAsNoTracking) =>
        await Task.Run(() => GetAllQueryable(EnableAsNoTracking).ToPage<TValue>(_cacheService, config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool EnableAsNoTracking) =>
         await Task.Run(() => GetAllQueryable(EnableAsNoTracking)
         .Where(filter.GenerateLambda<TValue, TFilter>(_cacheService))
         .ToPage<TValue>(_cacheService, config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking) =>
        await Task.Run(() => GetAllQueryable(EnableAsNoTracking)
        .Where(predicate)
        .ToPage<TValue>(_cacheService, config));

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
            entity.IsNull(nameof(CreateAsync), nameof(entity));
            SetState(EntityState.Added, entity);
            if (!_useCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            if (entityList.HasAny(nameof(CreateAsync), nameof(entityList)))
            {
                await _context.AddRangeAsync(entityList);
                if (!_useCommit)
                {
                    await SaveChangesAsync(token).ConfigureAwait(false);
                }
            }
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(UpdateAsync), nameof(entity));
            SetState(EntityState.Modified, entity);
            if (!_useCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            if (entityList.HasAny(nameof(DeleteAsync), nameof(entityList)))
            {
                _context.UpdateRange(entityList);
                if (!_useCommit)
                {
                    await SaveChangesAsync(token).ConfigureAwait(false);
                }
            }
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(DeleteAsync), nameof(entity));
            _context.Remove(entity);
            if (!_useCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            if (entityList.HasAny(nameof(DeleteAsync), nameof(entityList)))
            {
                _context.RemoveRange(entityList);
                if (!_useCommit)
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
         _context.SaveChangesAsync(cancellationToken);
        #endregion

        #region Protected Methods
        protected IQueryable<TValue> SetIncludes(IQueryable<TValue> query) =>
        includesString != null && includesString.Any() ?
                includesString.Aggregate(query, (current, include) =>
                current.Include(include)) : includesExp != null && includesExp.Any() ?
                includesExp.Aggregate(query, (current, include) =>
                current.Include(include)) : query;

        protected IQueryable<TValue> GetAllQueryable(bool EnableAsNoTracking)
        {
            var query = SetIncludes(_context.Set<TValue>());
            if (EnableAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        protected void StartCache()
        {
            _cacheService.SaveOnCacheIfNonExists<TValue>();
            _cacheService.SaveOnCacheIfNonExists<TFilter>();
        }

        protected void SetState(EntityState state, TValue item) => _context.Attach(item).State = state;
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
            if (IsValidCtor(mapperList, mapperData))
            {
                this.mapperList = mapperList;
                this.mapperData = mapperData;
            }
        }

        private bool IsValidCtor(
            Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList,
            Func<TValue, TResult> mapperData)
        {
            if (!mapperList.IsNull(
                nameof(BaseRepositoryAsync<TValue, TResult, TFilter>),
                nameof(mapperList))
            &&
                !mapperData.
                IsNull(nameof(BaseRepositoryAsync<TValue, TResult, TFilter>),
                nameof(mapperData)))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region ATTRIBUTES
        public Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList { get; set; }
        public Func<TValue, TResult> mapperData { get; set; }
        #endregion

        #region QUERY
        public new virtual async Task<IReadOnlyList<TResult>> GetAllAsync(bool EnableAsNoTracking) 
            => mapperList(await GetAllQueryable(EnableAsNoTracking).ToListAsync()).ToList();

        public new virtual async Task<IReadOnlyList<TResult>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking) =>
                predicate != null
                ?
                mapperList(
                    await GetAllQueryable(EnableAsNoTracking).
                    Where(predicate).
                    ToListAsync()).
                ToList()
                 :
                 mapperList(
                    await GetAllQueryable(EnableAsNoTracking).
                    ToListAsync()).
                 ToList();

        public new virtual async Task<IReadOnlyList<TResult>> FilterAllAsync(
            TFilter filter,
            bool EnableAsNoTracking) =>
            await GetAllByAsync(
                filter.
                GenerateLambda<TValue, TFilter>(_cacheService),
                EnableAsNoTracking);

        public new virtual async Task<TResult> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking) =>
                !predicate.
                IsNull(
                    nameof(GetSingleByAsync),
                    nameof(predicate))
                ?
                    mapperData(
                    await GetAllQueryable(EnableAsNoTracking).
                    SingleOrDefaultAsync(predicate))
                :
                null;

        public new virtual async Task<TResult> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking) =>
            !predicate.
            IsNull(
                nameof(GetSingleByAsync),
                nameof(predicate))
            ?
                mapperData(await GetAllQueryable(EnableAsNoTracking).
                FirstOrDefaultAsync(predicate))
            : null;

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            bool EnableAsNoTracking) =>
            await Task.
            Run(() =>
                GetAllQueryable(EnableAsNoTracking).
                ToPage<TValue, TResult>(
                    _cacheService,
                    mapperList,
                    config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool EnableAsNoTracking) =>
                await Task.
                Run(() =>
                GetAllQueryable(EnableAsNoTracking).
                ToPage<TValue, TResult>(
                    _cacheService,
                    mapperList,
                    config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking) =>
                await Task.Run(() =>
                    GetAllQueryable(EnableAsNoTracking).
                    ToPage<TValue, TResult>(
                        _cacheService,
                        mapperList,
                        config));
        #endregion

    }
}