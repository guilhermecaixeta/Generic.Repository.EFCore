using Generic.Service.Entity.IFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Service.Base
{
    public interface IBaseService<TValue, TFilter>
    where TFilter : IFilter
    where TValue : class
    {
#region Query
        ///<summary>
        /// Return all data
        ///</summary>
        IQueryable<TValue> GetAll(bool EnableAsNoTracking);
        ///<summary>
        /// Return all data filtred
        ///</summary>
        ///<param name="filter">Filter to apply</param>
        IQueryable<TValue> FilterAll(TFilter filter, bool EnableAsNoTracking);
        ///<summary>
        /// Return all data with pass on the predicate
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        IQueryable<TValue> GetAllBy(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);
        /// <summary>
        /// Return first data from a informed predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TValue> GetByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking);
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
}