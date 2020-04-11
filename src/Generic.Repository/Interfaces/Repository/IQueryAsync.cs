using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IQueryAsync<TValue>
        where TValue : class
    {
        #region ATTR

        IList<Expression<Func<TValue, object>>> IncludesExp { get; set; }
        IList<string> IncludesString { get; set; }

        #endregion ATTR

        #region INCLUDES METHODS

        void AddInclude(string include);

        void AddInclude(Expression<Func<TValue, object>> predicate);

        #endregion INCLUDES METHODS

        #region COMMONS QUERY

        /// <summary>
        /// Count elements using a predicate informed.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync(
            Expression<Func<TValue, bool>> predicate, 
            CancellationToken token);

        /// <summary>
        /// Count all elements on base.
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync(
            CancellationToken token);

        /// <summary>Finds the asynchronous.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<TValue> FindAsync(
            params object[] values);

        ///<summary>
        /// Return all data
        ///</summary>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TValue>> GetAllAsync(
            bool notTracking, 
            CancellationToken token);

        ///<summary>
        /// Return all data from predicate informed
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token);

        /// <summary>
        /// Return first data from a informed predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<TValue> GetFirstOrDefaultAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token);

        /// <summary>
        /// Return single data from a informed predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///<param name="notTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<TValue> GetSingleOrDefaultAsync(
            Expression<Func<TValue, bool>> predicate,
            bool notTracking,
            CancellationToken token);

        #endregion COMMONS QUERY
    }
}