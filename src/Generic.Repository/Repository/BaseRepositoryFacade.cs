using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Generic.Repository.Repository
{
    internal class BaseRepositoryFacade<TValue, TFilter>
        where TValue : class
        where TFilter : class, IFilter
    {
        private readonly Func<IQueryable<TValue>, IQueryable<TValue>> _funcAddIncludes;

        private readonly ICacheRepository _cache;

        private readonly DbContext _context;

        public BaseRepositoryFacade(
            DbContext context,
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
            var query = _funcAddIncludes(_context.Set<TValue>());
            if (enableAsNoTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public void StartCache()
        {
            _cache.AddGet<TValue>();
            _cache.AddSet<TValue>();
            _cache.AddProperty<TValue>();

            _cache.AddGet<TFilter>();
            _cache.AddAttribute<TFilter>();
        }

        public void SetState(EntityState state, TValue item) =>
            _context.Attach(item).State = state;

        public Expression<Func<TValue, bool>> GetExpressionByFilter(TFilter filter) =>
            filter.GeneratePredicate<TValue, TFilter>(_cache);

        public IPage<TValue> GetPage(IQueryable<TValue> query, IPageConfig config) =>
            query.ToPage(_cache, config);
        
        #endregion
    }
}
