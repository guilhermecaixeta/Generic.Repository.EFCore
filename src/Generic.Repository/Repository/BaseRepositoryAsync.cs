using Generic.Repository.Cache;
using Generic.Repository.Extension.Error;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Extension.Validation;
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

        private readonly BaseRepositoryFacade<TValue, TFilter> repositoryFacade;

        #endregion

        #region Ctor

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context)
        {
            CacheService = cacheService;
            Context = context;
            repositoryFacade = new BaseRepositoryFacade<TValue, TFilter>(Context, CacheService, SetIncludes);
            repositoryFacade.StartCache();
        }

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context,
            bool useCommit)
        {
            CacheService = cacheService;
            UseCommit = useCommit;
            Context = context;
        }

        #endregion

        #region QUERY

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(bool enableAsNoTracking) =>
            await repositoryFacade.GetAllQueryable(enableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            repositoryFacade.ThrowErrorNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await repositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TValue>> FilterAllAsync(
            TFilter filter,
            bool enableAsNoTracking) =>
                await GetAllByAsync(repositoryFacade.GetExpressionByFilter(filter), enableAsNoTracking);

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            repositoryFacade.ThrowErrorNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await repositoryFacade.GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            repositoryFacade.ThrowErrorNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            return await repositoryFacade.GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
                await Task.FromResult(repositoryFacade.GetPage(repositoryFacade.GetAllQueryable(enableAsNoTracking), config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
                 await Task.FromResult(repositoryFacade.GetPage(
                     repositoryFacade.GetAllQueryable(enableAsNoTracking).
                     Where(repositoryFacade.GetExpressionByFilter(filter)),
                     config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                await Task.FromResult(repositoryFacade.GetPage(
                    repositoryFacade.GetAllQueryable(enableAsNoTracking).
                    Where(predicate),
                    config));

        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate)
        {
            repositoryFacade.ThrowErrorNullValue(predicate, nameof(predicate), nameof(CountAsync));

            return await repositoryFacade.GetAllQueryable(true).CountAsync(predicate);
        }

        public virtual async Task<int> CountAsync() => await repositoryFacade.GetAllQueryable(true).CountAsync();

        #endregion

        #region Includes Methods
        public void AddInclude(string include) =>
            includesString.Add(include);

        public void AddInclude(Expression<Func<TValue, object>> predicate) =>
            includesExp.Add(predicate);

        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity, CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullValue(entity, nameof(entity), nameof(CreateAsync));

            repositoryFacade.SetState(EntityState.Added, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullOrEmptyList(entityList, nameof(entityList), nameof(CreateAsync));

            await Context.AddRangeAsync(entityList);

            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }

        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullValue(entity, nameof(entity), nameof(UpdateAsync));

            repositoryFacade.SetState(EntityState.Modified, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullOrEmptyList(entityList, nameof(entityList), nameof(UpdateAsync));

            Context.UpdateRange(entityList);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullValue(entity, nameof(entity), nameof(DeleteAsync));

            Context.Remove(entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            repositoryFacade.ThrowErrorNullOrEmptyList(entityList, nameof(entityList), nameof(DeleteAsync));

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
        public Task SaveChangesAsync() =>
            SaveChangesAsync(default);

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
         Context.SaveChangesAsync(cancellationToken);
        #endregion

        #region Protected Methods

        protected IQueryable<TValue> SetIncludes(IQueryable<TValue> query) =>
            !includesString.IsNull() && includesExp.IsNull() ?
                includesString.Aggregate(query, (current, include) => current.Include(include)) :
                !includesExp.IsNull() ?
                includesExp.Aggregate(query, (current, include) => current.Include(include)) :
                query;

        protected void InitiateFacade(BaseRepositoryFacade<TValue, TFilter> repositoryFacade)
        {
            repositoryFacade = new BaseRepositoryFacade<TValue, TFilter>(Context, CacheService, SetIncludes);
            repositoryFacade.StartCache();
        }
        #endregion
    }
}