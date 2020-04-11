using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IBaseRepositoryAsync<TValue, TFilter, TContext> :
        IPageable<TValue, TFilter>, ICommandRepository<TValue>, IQueryAsync<TValue>, IUnitOfWork
        where TValue : class
        where TFilter : class, IFilter
        where TContext : DbContext
    {
        #region QUERY

        /// <summary>
        ///     Return all data Filtered
        /// </summary>
        /// <param name="filter">Filter to apply</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TValue>> FilterAllAsync(
            TFilter filter,
            bool notTracking,
            CancellationToken token);

        /// <summary>
        ///     Return page Filtered.
        /// </summary>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="filter">Filter data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            TFilter filter,
            bool notTracking,
            CancellationToken token);

        #endregion QUERY

        #region QUERY<R>

        /// <summary>
        ///     Return all data Filtered
        /// </summary>
        /// <param name="filter">Filter to apply</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TReturn>> FilterAllAsync<TReturn>(
            TFilter filter,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token);

        /// <summary>
        ///     Return all data
        /// </summary>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token);

        /// <summary>
        ///     Return all data from predicate informed
        /// </summary>
        /// <param name="predicate">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            bool notTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token);

        /// <summary>
        ///     Return page.
        /// </summary>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        /// <summary>
        ///     Return page Filtered.
        /// </summary>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="filter">Filter data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            TFilter filter,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        /// <summary>
        ///     Return page Filtered.
        /// </summary>
        /// <param name="config">Condition to apply on data</param>
        /// <param name="predicate">Predicate to filter data</param>
        /// <param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool notTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        #endregion QUERY<R>
    }
}