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

namespace Generic.Repository.Repository
{
    internal class BaseRepositoryFacade<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        private Func<IQueryable<TValue>, IQueryable<TValue>> FuncAddIncludes;

        private ICacheRepository Cache;

        private DbContext Context;

        public BaseRepositoryFacade(
            DbContext context,
            ICacheRepository cache,
            Func<IQueryable<TValue>, IQueryable<TValue>> funcSetInclude)
        {
            Cache = cache;
            Context = context;
            FuncAddIncludes = funcSetInclude;
        }

        #region public Methods

        public IQueryable<TValue> GetAllQueryable(bool enableAsNoTracking)
        {
            var query = FuncAddIncludes(Context.Set<TValue>());
            if (enableAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public void StartCache()
        {
            Cache.AddGet<TValue>();
            Cache.AddSet<TValue>();
            Cache.AddGet<TFilter>();
            Cache.AddProperty<TValue>();
            Cache.AddAttribute<TFilter>();
        }

        public void SetState(EntityState state, TValue item) =>
            Context.Attach(item).State = state;

        public Expression<Func<TValue, bool>> GetExpressionByFilter(TFilter filter) =>
            filter.GeneratePredicate<TValue, TFilter>(Cache);

        public IPage<TValue> GetPage(IQueryable<TValue> query, IPageConfig config) =>
            query.ToPage(Cache, config);

        public void ThrowErrorNullValue(
            object obj,
            string nameAttribute,
            string nameMethod) =>
                obj.ThrowErrorNullValue(nameAttribute, nameMethod);

        public void ThrowErrorNullOrEmptyList(
            IEnumerable<TValue> list,
            string nameParameter,
            string nameMethod) =>
                list.ThrowErrorNullOrEmptyList(nameParameter, nameMethod);

        #endregion
    }
}
