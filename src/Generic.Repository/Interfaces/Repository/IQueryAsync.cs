using Generic.Repository.Models.Page;
using Generic.Repository.Models.PageAggregation.PageConfig;
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
        IList<string> IncludesString { get; set; }

        IList<Expression<Func<TValue, object>>> IncludesExp { get; set; }
        #endregion

        #region Includes Methods
        void AddInclude(string include);

        void AddInclude(Expression<Func<TValue, object>> predicate);
        #endregion

        #region COMMONS QUERY
        ///<summary>
        /// Return all data
        ///</summary>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TValue>> GetAllAsync(bool EnableAsNoTracking, CancellationToken token);

        ///<summary>
        /// Return all data from predicate informed
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IReadOnlyList<TValue>> GetAllByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking,
            CancellationToken token);

        ///<summary>
        /// Return page.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            bool EnableAsNoTracking,
            CancellationToken token);

        ///<summary>
        /// Return page filtred.
        ///</summary>
        ///<param name="config">Condition to apply on data</param>
        /// <param name="predicate">Predicate to filter data</param>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<IPage<TValue>> GetPageAsync(
            IPageConfig config,
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking,
            CancellationToken token);

        /// <summary>
        /// Return single data from a informed predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<TValue> GetSingleByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking,
            CancellationToken token);

        /// <summary>
        /// Return first data from a informed predicate.
        /// </summary>
        /// <param name="predicate"></param>
        ///<param name="EnableAsNoTracking">Condition to tracking data</param>
        /// <returns></returns>
        Task<TValue> GetFirstByAsync(
            Expression<Func<TValue, bool>> predicate,
            bool EnableAsNoTracking,
            CancellationToken token);

        /// <summary>Finds the asynchronous.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        Task<TValue> FindAsync(
            params object[] values);

        /// <summary>
        /// Count elements using a predicate informed.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<int> CountAsync(Expression<Func<TValue, bool>> predicate, CancellationToken token);

        /// <summary>
        /// Count all elements on base.
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync(CancellationToken token);
        #endregion
    }
}
