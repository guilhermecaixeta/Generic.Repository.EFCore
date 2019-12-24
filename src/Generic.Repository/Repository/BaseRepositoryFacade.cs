using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.Validations.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryFacade<TValue, TFilter> : BaseRepositoryFacade<TValue>
        where TValue : class
        where TFilter : class, IFilter
    {
        private BaseRepositoryFacade(
            ICacheRepository cache) : base(cache)
        { }

        #region public Methods

        /// <summary>Initializers the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="enableAsNotTracking">if set to <c>true</c> [enable as no tracking].</param>
        /// <param name="funcSetInclude">The function set include.</param>
        /// <returns></returns>
        public new static async Task<BaseRepositoryFacade<TValue, TFilter>> Initializer(
            ICacheRepository cache,
            CancellationToken token)
        {
            var instance = new BaseRepositoryFacade<TValue, TFilter>(cache);

            await instance.StartCache(token);

            return instance;
        }

        public async Task<Expression<Func<TValue, bool>>> GetExpressionByFilter(
            TFilter filter,
            CancellationToken token) =>
                await filter.
                    CreateGenericFilter<TValue, TFilter>(_cache, token);

        public new async Task<IPage<TValue>> GetPage(
            IQueryable<TValue> query,
            IPageConfig config,
            CancellationToken token) =>
                await Task.Run(() => query.
                    ToPageFiltered<TValue, TFilter>(_cache, config), token);

        public new async Task<IPage<TResult>> GetPage<TResult>(
            IQueryable<TValue> query,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            CancellationToken token)
            where TResult : class =>
                await Task.Run(() => query.
                    ToPageFiltered<TValue, TFilter, TResult>(_cache, mapping, config), token);

        protected override async Task StartCache(CancellationToken token)
        {
            await base.StartCache(token);
            await _cache.AddGet<TFilter>(token);
            await _cache.AddAttribute<TFilter>(token);
        }

        #endregion public Methods
    }

    public class BaseRepositoryFacade<TValue>
        where TValue : class
    {
        protected readonly ICacheRepository _cache;

        protected BaseRepositoryFacade(
            ICacheRepository cache)
        {
            ThrowErrorIf.IsNullValue(cache, nameof(cache), typeof(BaseRepositoryFacade<,>).Name);

            _cache = cache;
        }

        #region public Methods

        /// <summary>Initializers the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="enableAsNotTracking">if set to <c>true</c> [enable as no tracking].</param>
        /// <param name="funcSetInclude">The function set include.</param>
        /// <returns></returns>
        public static async Task<BaseRepositoryFacade<TValue>> Initializer(
            ICacheRepository cache,
            CancellationToken token)
        {
            var instance = new BaseRepositoryFacade<TValue>(cache);

            await instance.StartCache(token);

            return instance;
        }

        public virtual async Task<IPage<TValue>> GetPage(
            IQueryable<TValue> query,
            IPageConfig config,
            CancellationToken token) =>
                await Task.Run(() => query.ToPage(_cache, config), token);

        public virtual async Task<IPage<TResult>> GetPage<TResult>(
            IQueryable<TValue> query,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            CancellationToken token)
            where TResult : class =>
                await Task.Run(() => query.ToPage(_cache, config, mapping), token);

        protected virtual async Task StartCache(CancellationToken token)
        {
            await _cache.AddGet<TValue>(token);
            await _cache.AddSet<TValue>(token);
            await _cache.AddProperty<TValue>(token);
        }

        #endregion public Methods
    }
}