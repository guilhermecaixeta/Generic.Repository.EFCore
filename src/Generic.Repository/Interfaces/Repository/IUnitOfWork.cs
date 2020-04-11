using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Disables the autotransaction and begin trnsaction.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task DisableAutotransactionAndBeginTransaction(CancellationToken token);

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task BeginTransactionAsync(CancellationToken token);

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task CommitAsync(CancellationToken token);

        /// <summary>
        /// Saves the changes and commit asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task SaveChangesAndCommitAsync(CancellationToken token);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken token);

        /// <summary>
        /// Multis the transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkTransactionsAsync(
            Func<DbContext, Task> transaction,
            CancellationToken token);

        /// <summary>
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkScopedTransactionsAsync(
            Func<Task> transaction,
            CancellationToken token);
    }
}
