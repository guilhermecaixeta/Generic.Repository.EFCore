using Generic.Repository.Cache;
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
using System.Threading.Tasks;
using Generic.Repository.ThrowError;

namespace Generic.Repository.Repository
{
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
            ThrowErrorIf.
                IsNullValue(mapperList, nameof(mapperList), nameof(BaseRepositoryAsync<TValue, TResult, TFilter>));
            ThrowErrorIf.
                IsNullValue(mapperData, nameof(mapperData), nameof(BaseRepositoryAsync<TValue, TResult, TFilter>));

            this.mapperList = mapperList;
            this.mapperData = mapperData;
        }

        #endregion

        #region ATTRIBUTES
        public Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList { get; set; }
        public Func<TValue, TResult> mapperData { get; set; }
        #endregion

        #region QUERY
        public new virtual async Task<IReadOnlyList<TResult>> GetAllAsync(bool enableAsNoTracking)
            => mapperList(await RepositoryFacade.GetAllQueryable(enableAsNoTracking).ToListAsync()).ToList();

        public new virtual async Task<IReadOnlyList<TResult>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            var queryList = RepositoryFacade.GetAllQueryable(enableAsNoTracking);
            var list = await queryList.ToListAsync();

            if (!predicate.IsNull())
            {
                list = await queryList.Where(predicate).ToListAsync();
            }

            return mapperList(list).ToList();
        }
        public new virtual async Task<IReadOnlyList<TResult>> FilterAllAsync(
            TFilter filter,
            bool enableAsNoTracking) =>
                await GetAllByAsync(RepositoryFacade.GetExpressionByFilter(filter), enableAsNoTracking);

        public new virtual async Task<TResult> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            var value = await RepositoryFacade.
                GetAllQueryable(enableAsNoTracking).
                SingleOrDefaultAsync(predicate);

            return mapperData(value);
        }

        public new virtual async Task<TResult> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorIf.IsNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));

            var value = await RepositoryFacade.
                GetAllQueryable(enableAsNoTracking).
                FirstOrDefaultAsync(predicate);

            return mapperData(value);
        }

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                {
                    var listToPage = RepositoryFacade.
                        GetAllQueryable(enableAsNoTracking);

                    return GetPage(listToPage, config);
                });

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                {
                    var expression = RepositoryFacade.
                        GetExpressionByFilter(filter);

                    var listToPage = RepositoryFacade.
                        GetAllQueryable(enableAsNoTracking).
                        Where(expression);

                    return GetPage(listToPage, config);
                });

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                {
                    var listToPage = RepositoryFacade.
                        GetAllQueryable(enableAsNoTracking).
                        Where(predicate);

                    return GetPage(listToPage, config);
                });
        #endregion

        #region Private Methods
        private IPage<TResult> GetPage(IQueryable<TValue> query, IPageConfig config) =>
                query.ToPage(CacheService, mapperList, config);
        #endregion

    }
}
