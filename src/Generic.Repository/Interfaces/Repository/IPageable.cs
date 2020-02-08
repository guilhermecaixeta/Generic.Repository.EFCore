using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IPageable<TValue>
        where TValue : class
    {
        ///<summary>
        /// Return page.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool notTracking,
            CancellationToken token);

        ///<summary>
        /// Return page Filtered.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        /// <param name="predicate">Predicate to filter data</param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token);
    }

    public interface IPageable<TValue, TFilter>
    where TValue : class
    where TFilter : class, IFilter
    {
        ///<summary>
        /// Return page.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool notTracking,
            CancellationToken token);

        ///<summary>
        /// Return page Filtered.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        /// <param name="predicate">Predicate to filter data</param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token);

        /// <summary>Pages the asynchronous.</summary>
        /// <param name="config">The configuration.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="notTracking">if set to <c>true</c> [not tracking].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool notTracking,
            CancellationToken token);
    }
}