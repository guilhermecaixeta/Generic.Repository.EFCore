using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Generic.Repository.Repository
{
    public class UnitOfWorkAsync<TContext>
        where TContext : DbContext
    {
        private bool _autoTransaction;

        /// <summary>
        /// The context
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWorkAsync{TContext}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UnitOfWorkAsync(TContext context)
        {
            ThrowErrorIf.
               IsNullValue(
                context,
                nameof(context),
                typeof(UnitOfWorkAsync<>).Name);

            Context = context;
        }

        /// <summary>
        /// Disables the autotransaction and begin transaction.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task DisableAutotransactionAndBeginTransaction(CancellationToken token)
        {
            _autoTransaction = Context.Database.AutoTransactionsEnabled;

            Context.Database.AutoTransactionsEnabled = false;

            return Context.Database.BeginTransactionAsync(token);
        }

        /// <summary>
        /// Begins the transaction asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task BeginTransactionAsync(CancellationToken token) =>
            Context.Database.BeginTransactionAsync(token);

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task CommitAsync(CancellationToken token)
        {
            Context.Database.AutoTransactionsEnabled = _autoTransaction;

            return Task.Run(() => Context.Database.CommitTransaction(), token);
        }

        /// <summary>
        /// Saves the changes and commit asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        public async Task SaveChangesAndCommitAsync(CancellationToken token)
        {
            await SaveChangesAsync(token).
                ContinueWith(_ => CommitAsync(token)).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task SaveChangesAsync(CancellationToken token) =>
            Context.SaveChangesAsync(token);

        /// <summary>
        /// Unit of Work Transactions asynchronous.
        /// All transaction here are inside a Unit Of Work Block:
        /// (Begin transaction) { try{ Your Transactions }catch(Error){ Rollback; Throw Error;} } 
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        public async Task UnitOfWorkTransactionsAsync(
            Func<DbContext, Task> transaction,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(transaction, nameof(transaction), nameof(UnitOfWorkTransactionsAsync));

            var strategy = Context.
                Database.
                CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
               {
                   using (var contextTransaction = Context.
                   Database.
                   BeginTransaction())
                   {
                       try
                       {
                           await transaction(Context).
                                ConfigureAwait(false);

                           await contextTransaction.
                               CommitAsync(token).
                               ConfigureAwait(false);
                       }
                       catch (Exception e)
                       {
                           await contextTransaction.
                               RollbackAsync(token).
                               ConfigureAwait(false);

                           throw e;
                       }
                   };
               });
        }

        /// <summary>
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        public async Task UnitOfWorkScopedTransactionsAsync(
            Func<Task> transaction,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(
                    transaction,
                    nameof(transaction),
                    nameof(UnitOfWorkScopedTransactionsAsync));

            var strategy = Context.
                Database.
                CreateExecutionStrategy();

            await strategy.
                ExecuteAsync(async () =>
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {

                        await transaction().
                        ConfigureAwait(false);

                        transactionScope.Complete();
                    };
                });
        }
    }
}
