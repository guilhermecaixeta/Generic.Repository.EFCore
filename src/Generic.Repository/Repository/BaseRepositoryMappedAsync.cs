using Generic.Repository.Cache;
using Generic.Repository.Extension.Error;
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
            mapperListFunc.ThrowErrorNullValue(nameof(mapperListFunc), nameof(IsValidCtor));
            mapperDataFunc.ThrowErrorNullValue(nameof(mapperDataFunc), nameof(IsValidCtor));

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
            bool enableAsNoTracking)
        {
            var queryList = GetAllQueryable(enableAsNoTracking);
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
                await GetAllByAsync(GetExpressionByFilter(filter), enableAsNoTracking);

        public new virtual async Task<TResult> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorNullValue(predicate, nameof(predicate), nameof(GetSingleByAsync));

            var value = await GetAllQueryable(enableAsNoTracking).SingleOrDefaultAsync(predicate);
            return mapperData(value);
        }

        public new virtual async Task<TResult> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking)
        {
            ThrowErrorNullValue(predicate, nameof(predicate), nameof(GetFirstByAsync));
            var value = await GetAllQueryable(enableAsNoTracking).FirstOrDefaultAsync(predicate);
            return mapperData(value);
        }

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                    GetPage(GetAllQueryable(enableAsNoTracking), config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                    GetPage(GetAllQueryable(enableAsNoTracking).Where(GetExpressionByFilter(filter)), config));

        public new virtual async Task<IPage<TResult>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNoTracking) =>
                await Task.Run(() =>
                    GetPage(GetAllQueryable(enableAsNoTracking).Where(predicate), config));
        #endregion

        #region Private Methods
        private IPage<TResult> GetPage(IQueryable<TValue> query, IPageConfig config) =>
                query.ToPage(CacheService, mapperList, config);
        #endregion

    }
}
