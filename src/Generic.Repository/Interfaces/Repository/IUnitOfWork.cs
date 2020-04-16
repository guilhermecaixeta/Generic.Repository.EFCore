using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface IUnitOfWork
    {
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
        /// Disables the autotransaction and begin transaction asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task DisableAutotransactionAndBeginTransactionAsync(CancellationToken token);

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
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkScopedTransactionsAsync(
            Func<CancellationToken, Task> transaction,
            CancellationToken token);

        /// <summary>
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkScopedTransactionsAsync(
            Action transaction,
            CancellationToken token);

        /// <summary>
        /// Units the of work transactions asynchronous.
        /// All transaction here are inside a Unit Of Work Block:
        /// (Begin transaction) { try{ Your_Transactions; Commit; }catch(Error){ Rollback; Throw;} }
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkTransactionsAsync(
            Func<DbContext, CancellationToken, Task> transaction,
            CancellationToken token);

        /// <summary>
        /// Unit of Work Transactions asynchronous.
        /// All transaction here are inside a Unit Of Work Block:
        /// (Begin transaction) { try{ Your_Transactions; Commit; }catch(Error){ Rollback; Throw;} }
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task UnitOfWorkTransactionsAsync(
            Action<DbContext> transaction,
            CancellationToken token);
    }
}
