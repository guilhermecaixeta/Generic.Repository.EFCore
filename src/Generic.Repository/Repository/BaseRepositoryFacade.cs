using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryFacade<TValue, TFilter> : BaseRepositoryFacade<TValue>
        where TValue : class
        where TFilter : class, IFilter
    {
        public BaseRepositoryFacade(
            DbSet<TValue> context,
            ICacheRepository cache,
            Func<IQueryable<TValue>, IQueryable<TValue>> funcSetInclude) : base(context, cache, funcSetInclude)
        { }

        #region public Methods

        public override async Task StartCache()
        {
            await base.StartCache();
            await _cache.AddGet<TFilter>();
            await _cache.AddAttribute<TFilter>();
        }

        public async Task<Expression<Func<TValue, bool>>> GetExpressionByFilter(TFilter filter) =>
            await filter.CreateGenericFilter<TValue, TFilter>(_cache);

        public new IPage<TValue> GetPage(IQueryable<TValue> query, IPageConfig config) =>
            query.ToPageFiltred<TValue, TFilter>(_cache, config);

        public new IPage<TResult> GetPage<TResult>(IQueryable<TValue> query, IPageConfig config, Func<IEnumerable<object>, IEnumerable<TResult>> mapping)
            where TResult : class =>
            query.ToPageFiltred<TValue, TFilter, TResult>(_cache, mapping, config);
        #endregion
    }

    public class BaseRepositoryFacade<TValue>
        where TValue : class
    {
        protected readonly Func<IQueryable<TValue>, IQueryable<TValue>> _funcAddIncludes;

        protected readonly ICacheRepository _cache;

        protected readonly DbSet<TValue> _context;

        public BaseRepositoryFacade(
            DbSet<TValue> context,
            ICacheRepository cache,
            Func<IQueryable<TValue>, IQueryable<TValue>> funcSetInclude)
        {
            _cache = cache;
            _context = context;
            _funcAddIncludes = funcSetInclude;
        }

        #region public Methods

        public IQueryable<TValue> GetAllQueryable(bool enableAsNoTracking)
        {
            var query = _funcAddIncludes(_context);
            if (enableAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public virtual async Task StartCache()
        {
            await _cache.AddGet<TValue>();
            await _cache.AddSet<TValue>();
            await _cache.AddProperty<TValue>();
        }

        public void SetState(EntityState state, TValue item) =>
            _context.Attach(item).State = state;

        public virtual IPage<TValue> GetPage(IQueryable<TValue> query, IPageConfig config) =>
            query.ToPage(_cache, config);

        public virtual IPage<TResult> GetPage<TResult>(IQueryable<TValue> query, IPageConfig config, Func<IEnumerable<object>, IEnumerable<TResult>> mapping)
            where TResult : class =>
            query.ToPage(_cache, config, mapping);
        #endregion
    }
}
