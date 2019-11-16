using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface ICommandRepository<TValue>
     where TValue : class
    {
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
        Task SaveChangesAsync();
        /// <summary>
        /// Commit async transaction if useCommit is true 
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);
        #endregion
    }
}
