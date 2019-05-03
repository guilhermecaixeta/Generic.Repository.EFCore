using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.Page;
using Generic.Repository.Models.Page.PageConfig;

namespace Generic.Repository.Repository
{
    public interface IBaseRepositoryAsync<TValue, TFilter>
    where TValue : class
    where TFilter : IFilter
    {
        #region ATTR
        IEnumerable<string> includesString { get; set; }
        IEnumerable<Expression<Func<TValue, object>>> includesExp { get; set; }
        #endregion

        #region Query
        ///<summary>
        /// Return all data
        ///</summary>
        Task<IReadOnlyList<TValue>> GetAllAsync(bool EnableAsNoTracking);
        ///<summary>
        /// Return all data filtred
        ///</summary>
        ///<param name="filter">Filter to apply</param>
        Task<IReadOnlyList<TValue>> FilterAllAsync(TFilter filter, bool EnableAsNoTracking);
        ///<summary>
        /// Return all data with pass on the predicate
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        Task<IReadOnlyList<TValue>> GetAllByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);
        Task<IPage<TValue>> GetPageAsync(IPageConfig config, bool EnableAsNoTracking);
        /// <summary>
        /// Return first data from a informed predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TValue> GetByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
        /// <summary>
        /// Save data async
        /// </summary>
        /// <returns></returns>
        Task<TValue> CreateAsync(TValue entity, CancellationToken token);
        /// <summary>
        /// Save list async
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token);
        /// <summary>
        /// Update data async
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(TValue entity, CancellationToken token);
        /// <summary>
        /// Update list async
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token);
        /// <summary>
        /// Delete data async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TValue entity, CancellationToken token);
        /// <summary>
        /// Delete list async
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token);
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) Without CancellationToken
        /// <summary>
        /// Save data async
        /// </summary>
        /// <returns></returns>
        Task<TValue> CreateAsync(TValue entity);
        /// <summary>
        /// Save list async
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(IEnumerable<TValue> entityList);
        /// <summary>
        /// Update data async
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(TValue entity);
        /// <summary>
        /// Update list async
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TValue> entityList);
        /// <summary>
        /// Delete data async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TValue entity);
        /// <summary>
        /// Delete list async
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(IEnumerable<TValue> entityList);
        #endregion

        #region COMMIT - (SAVECHANGES)
        /// <summary>
        /// Commit async transaction if useCommit is true 
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();
        /// <summary>
        /// Commit async transaction if useCommit is true 
        /// </summary>
        /// <returns></returns>
        Task CommitAsync(CancellationToken cancellationToken);
        #endregion
    }

    public interface IBaseRepositoryAsync<TValue, TResult, TFilter> : IBaseRepositoryAsync<TValue, TFilter>
    where TValue : class
    where TResult : class
    where TFilter : IFilter
    {
        Func<IEnumerable<TValue>, IEnumerable<TResult>> mapperList { get; set; }
        Func<TValue, TResult> mapperData { get; set; }

        #region Query
        ///<summary>
        /// Return all data
        ///</summary>
        new Task<IReadOnlyList<TResult>> GetAllAsync(bool EnableAsNoTracking);
        ///<summary>
        /// Return all data filtred
        ///</summary>
        ///<param name="filter">Filter to apply</param>
        new Task<IReadOnlyList<TResult>> FilterAllAsync(TFilter filter, bool EnableAsNoTracking);
        ///<summary>
        /// Return all data with pass on the predicate
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        new Task<IReadOnlyList<TResult>> GetAllByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);

        new Task<IPage<TResult>> GetPageAsync(IPageConfig config, bool EnableAsNoTracking);
        /// <summary>
        /// Return first data from a informed predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        new Task<TResult> GetByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);
        #endregion

    }
}
