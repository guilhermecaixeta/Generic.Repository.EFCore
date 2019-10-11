using Generic.Repository.Cache;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;
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

        internal readonly BaseRepositoryFacade<TValue, TFilter> RepositoryFacade;

        #endregion

        #region Ctor

        public BaseRepositoryAsync(
            ICacheRepository cacheService,
            DbContext context)
        {
            CacheService = cacheService;
            Context = context;
            RepositoryFacade = new BaseRepositoryFacade<TValue, TFilter>(Context, CacheService, SetIncludes);
            RepositoryFacade.StartCache();
        }

        #endregion

        #region QUERY

        public virtual async Task<IReadOnlyList<TValue>> GetAllAsync(bool enableAsNoTracking) =>
            await RepositoryFacade.GetAllQueryable(enableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate).ToListAsync();
        }

        public virtual async Task<IReadOnlyList<TValue>> FilterAllAsync(
            TFilter filter,
            bool enableAsNoTracking) =>
                await GetAllByAsync(RepositoryFacade.GetExpressionByFilter(filter), enableAsNoTracking);

        public virtual async Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            return await RepositoryFacade.GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
                await Task.FromResult(RepositoryFacade.GetPage(RepositoryFacade.GetAllQueryable(enableAsNoTracking), config));

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
                 await Task.Run(() =>
                 {
                     var expression = RepositoryFacade.GetExpressionByFilter(filter);
                     var listToPage = RepositoryFacade.
                         GetAllQueryable(enableAsNoTracking).
                         Where(expression);
                     return RepositoryFacade.GetPage(listToPage, config);
                 });

        public virtual async Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                {
                    var listToPage = RepositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate);
                    return RepositoryFacade.GetPage(listToPage, config);
                }).ConfigureAwait(false);

        public virtual async Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(CountAsync));

            return await RepositoryFacade.GetAllQueryable(true).CountAsync(predicate).ConfigureAwait(false);
        }

        public virtual async Task<int> CountAsync() =>
            await RepositoryFacade.GetAllQueryable(true).CountAsync().ConfigureAwait(false);

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
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(CreateAsync));

            RepositoryFacade.SetState(EntityState.Added, entity);

            await SaveChangesAsync(token).ConfigureAwait(false);

            return entity;
        }

        public virtual async Task CreateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(CreateAsync));

            await Context.AddRangeAsync(entityList, token).ConfigureAwait(false);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(UpdateAsync));

            RepositoryFacade.SetState(EntityState.Modified, entity);
            if (!UseCommit)
            {
                await SaveChangesAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(UpdateAsync));

            Context.UpdateRange(entityList);

            await SaveChangesAsync(token).ConfigureAwait(false);

        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(DeleteAsync));

            Context.Remove(entity);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(DeleteAsync));

            Context.RemoveRange(entityList);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) Without CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity) =>
        await CreateAsync(entity, default).ConfigureAwait(false);

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList) =>
        await CreateAsync(entityList, default).ConfigureAwait(false);

        public virtual async Task UpdateAsync(TValue entity) =>
        await UpdateAsync(entity, default).ConfigureAwait(false);

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList) =>
        await UpdateAsync(entityList, default).ConfigureAwait(false);

        public virtual async Task DeleteAsync(TValue entity) =>
        await DeleteAsync(entity, default).ConfigureAwait(false);

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList) =>
        await DeleteAsync(entityList, default).ConfigureAwait(false);
        #endregion

        #region COMMIT

        public Task SaveChangesAsync() =>
            SaveChangesAsync(default);

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
         Context.SaveChangesAsync(cancellationToken);
        #endregion

        #region Protected Methods

        internal IQueryable<TValue> SetIncludes(IQueryable<TValue> query) =>
            !includesString.IsNull() && includesExp.IsNull() ?
                includesString.Aggregate(query, (current, include) => current.Include(include)) :
                !includesExp.IsNull() ?
                includesExp.Aggregate(query, (current, include) => current.Include(include)) :
                query;
        #endregion
    }
}