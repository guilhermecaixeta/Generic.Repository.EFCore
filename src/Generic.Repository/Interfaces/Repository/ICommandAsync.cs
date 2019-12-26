using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        #endregion COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        #region COMMIT - (SAVECHANGES)

        /// <summary>Saves the changes asynchronous.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken cancellationToken);

        /// <summary>  Exceute the specified actions inside a transaction.</summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task TransactionAsync(Action<DbSet<TValue>> transaction, CancellationToken token);

        #endregion COMMIT - (SAVECHANGES)
    }
}