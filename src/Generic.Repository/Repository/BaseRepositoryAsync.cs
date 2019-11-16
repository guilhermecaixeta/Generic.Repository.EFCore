namespace Generic.Repository.Repository
{
    using Generic.Repository.Cache;
    using Generic.Repository.Interfaces.Repository;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.Page;
    using Generic.Repository.Models.PageAggregation.PageConfig;
    using Generic.Repository.Validations.Extension.Validation;
    using Generic.Repository.Validations.ThrowError;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class BaseRepositoryAsync<TValue, TFilter, TContext> :
        CommandAsync<TValue, TContext>, IBaseRepositoryAsync<TValue, TFilter, TContext>
        where TValue : class
        where TFilter : class, IFilter
        where TContext : DbContext
    {
        #region Ctor
        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) : base(context, cacheService)
        {

        }

        public Task<IReadOnlyList<TValue>> FilterAllAsync(TFilter filter, bool enableAsNoTracking, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TReturn>> FilterAllAsync<TReturn>(TFilter filter, bool enableAsNoTracking, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(bool enableAsNoTracking, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(bool enableAsNoTracking, Expression<Func<TValue, bool>> predicate, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IPage<TValue>> GetPageAsync(IPageConfig config, TFilter filter, bool enableAsNoTracking, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task<IPage<TReturn>> GetPageAsync<TReturn>(IPageConfig config, bool enableAsNoTracking, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token) where TReturn : class
        {
            throw new NotImplementedException();
        }

        public Task<IPage<TReturn>> GetPageAsync<TReturn>(IPageConfig config, TFilter filter, bool enableAsNoTracking, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token) where TReturn : class
        {
            throw new NotImplementedException();
        }

        public Task<IPage<TReturn>> GetPageAsync<TReturn>(IPageConfig config, bool enableAsNoTracking, Expression<Func<TValue, bool>> predicate, Func<IEnumerable<object>, IEnumerable<TReturn>> mapper, CancellationToken token) where TReturn : class
        {
            throw new NotImplementedException();
        }
        #endregion



    }

    public class BaseRepositoryAsync<TValue, TContext> :
        CommandAsync<TValue, TContext>, IBaseRepositoryAsync<TValue, TContext>
    where TContext : DbContext
    where TValue : class
    {

        #region Ctor
        public BaseRepositoryAsync(
            TContext context,
            ICacheRepository cacheService) : base(context, cacheService)
        {

        }

        public async Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool enableAsNoTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(enableAsNoTracking, nameof(enableAsNoTracking), nameof(GetPageAsync));

            var listAll = await RepositoryFacade.
                GetAllQueryable(enableAsNoTracking).
                ToListAsync().
                ConfigureAwait(false);
            return mapper(listAll).ToList();
        }

        public async Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(enableAsNoTracking, nameof(enableAsNoTracking), nameof(GetPageAsync));

            var listAll = await RepositoryFacade.
                GetAllQueryable(enableAsNoTracking).
                Where(predicate).
                ToListAsync().
                ConfigureAwait(false);
            return mapper(listAll).ToList();
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNoTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(enableAsNoTracking, nameof(enableAsNoTracking), nameof(GetPageAsync));

            var queryable = RepositoryFacade.GetAllQueryable(enableAsNoTracking);

            return await Task.Run(() =>
            {
                return RepositoryFacade.GetPage(queryable, config, mapper);
            }); throw new NotImplementedException();
        }

        public async Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNoTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class
        {
            ThrowErrorIf.IsNullValue(config, nameof(config), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(mapper, nameof(mapper), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetPageAsync));
            ThrowErrorIf.IsNullValue(enableAsNoTracking, nameof(enableAsNoTracking), nameof(GetPageAsync));

            var queryable = RepositoryFacade.GetAllQueryable(enableAsNoTracking).Where(predicate);

            return await Task.Run(() =>
            {
                return RepositoryFacade.GetPage(queryable, config, mapper);
            });
        }
        #endregion

    }
}