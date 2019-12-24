using Generic.Repository.Models.PageAggregation;
using Generic.Repository.Models.PageAggregation.PageConfig;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IBaseRepositoryAsync<TValue> : ICommandRepository<TValue>, IQueryAsync<TValue>
     where TValue : class
    {
        #region QUERY<TReturn>

        ///<summary>
        /// Return all data
        ///</summary>
        ///<param name="enableAsNotTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TReturn>> GetAllAsync<TReturn>(
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        ///<summary>
        /// Return all data from predicate informed
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        ///<param name="enableAsNotTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TReturn>> GetAllByAsync<TReturn>(
            Expression<Func<TValue, bool>> predicate,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        ///<summary>
        /// Return page.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        ///<param name="enableAsNotTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        ///<summary>
        /// Return page Filtered.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        /// <param name="predicate">Predicate to filter data</param>
        ///<param name="enableAsNotTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TReturn>> GetPageAsync<TReturn>(
            IPageConfig config,
            bool enableAsNotTracking,
            Expression<Func<TValue, bool>> predicate,
            Func<IEnumerable<object>, IEnumerable<TReturn>> mapper,
            CancellationToken token) where TReturn : class;

        #endregion QUERY<TReturn>
    }
}