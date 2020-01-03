using Generic.Repository.Cache;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Page;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.ThrowError;

namespace Generic.Repository.Repository
{
    internal class BaseRepositoryFacade<TValue>
        where TValue : class
    {
        protected readonly ICacheRepository Cache;

        protected BaseRepositoryFacade(
            ICacheRepository cache)
        {
            ThrowErrorIf.
                IsNullValue(cache, nameof(cache), typeof(BaseRepositoryFacade<,>).Name);

            Cache = cache;
        }

        #region PUBLIC METHODS

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

            await instance.StartCache(token).
                ConfigureAwait(false);

            return instance;
        }

        public virtual async Task<IPage<TValue>> GetPage(
            IQueryable<TValue> query,
            IPageConfig config,
            CancellationToken token) =>
                await Task.Run(() => query.ToPage(Cache, config), token).
                    ConfigureAwait(false);

        public virtual async Task<IPage<TResult>> GetPage<TResult>(
            IQueryable<TValue> query,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            CancellationToken token)
            where TResult : class =>
                await Task.Run(() => query.ToPage(Cache, config, mapping), token).
                    ConfigureAwait(false);

        protected virtual async Task StartCache(CancellationToken token)
        {
            await Cache.AddGet<TValue>(token).
                ConfigureAwait(false);

            await Cache.AddSet<TValue>(token).
                ConfigureAwait(false);

            await Cache.AddProperty<TValue>(token).
                ConfigureAwait(false);

            await Cache.AddProperty<TValue>(token).
                ConfigureAwait(false);
        }

        #endregion PUBLIC METHODS
    }

    internal class BaseRepositoryFacade<TValue, TFilter> : BaseRepositoryFacade<TValue>
            where TValue : class
        where TFilter : class, IFilter
    {
        private BaseRepositoryFacade(
            ICacheRepository cache) : base(cache)
        { }

        #region PUBLIC METHODS

        /// <summary>Initializers the specified cache.</summary>
        /// <param name="cache">The cache.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public new static async Task<BaseRepositoryFacade<TValue, TFilter>> Initializer(
            ICacheRepository cache,
            CancellationToken token)
        {
            var instance = new BaseRepositoryFacade<TValue, TFilter>(cache);

            await instance.StartCache(token).
                ConfigureAwait(false);

            return instance;
        }

        /// <summary>Gets the expression by filter.</summary>
        /// <param name="filter">The filter.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<Expression<Func<TValue, bool>>> GetExpressionByFilter(
            TFilter filter,
            CancellationToken token) =>
                await filter.
                    CreateGenericFilter<TValue, TFilter>(Cache, token).
                    ConfigureAwait(false);

        /// <summary>Gets the page.</summary>
        /// <param name="query">The query.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public new async Task<IPage<TValue>> GetPage(
            IQueryable<TValue> query,
            IPageConfig config,
            CancellationToken token) =>
                await Task.Run(() => query.
                    ToPageFiltered<TValue, TFilter>(Cache, config), token).
                    ConfigureAwait(false);

        public new async Task<IPage<TResult>> GetPage<TResult>(
            IQueryable<TValue> query,
            IPageConfig config,
            Func<IEnumerable<object>, IEnumerable<TResult>> mapping,
            CancellationToken token)
            where TResult : class =>
                await Task.Run(() => query.
                    ToPageFiltered<TValue, TFilter, TResult>(Cache, mapping, config), token).
                    ConfigureAwait(false);

        protected override async Task StartCache(CancellationToken token)
        {
            await base.StartCache(token).
                ConfigureAwait(false);

            await Cache.AddGet<TFilter>(token).
                ConfigureAwait(false);

            await Cache.AddAttribute<TFilter>(token).
                ConfigureAwait(false);
        }

        #endregion PUBLIC METHODS
    }
}