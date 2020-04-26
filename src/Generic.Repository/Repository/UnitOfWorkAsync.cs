using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Generic.Repository.Repository
{
    public class UnitOfWorkAsync<TContext>
        where TContext : DbContext
    {
        /// <summary>
        /// The context
        /// </summary>
        protected readonly TContext Context;

        /// <summary>
        /// The automatic transaction
        /// </summary>
        private bool _autoTransaction;

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
            _autoTransaction = Context.Database.AutoTransactionsEnabled;
        }

        /// <summary>
        /// Begins the transaction asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task BeginTransactionAsync(
                                        CancellationToken token) =>
            Context.Database.BeginTransactionAsync(token);

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task CommitAsync(
                              CancellationToken token) =>
            Task.Run(() => Context.Database.CommitTransaction(), token).
                ContinueWith(_ =>
                {
                    Context.Database.AutoTransactionsEnabled = _autoTransaction;
                });

        /// <summary>
        /// Disables the autotransaction and begin transaction.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task DisableAutotransactionAndBeginTransactionAsync(
                                                            CancellationToken token)
        {
            Context.Database.AutoTransactionsEnabled = false;

            return Context.Database.BeginTransactionAsync(token);
        }

        /// <summary>
        /// Saves the changes and commit asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        public async Task SaveChangesAndCommitAsync(
                                                  CancellationToken token)
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
        public Task SaveChangesAsync(
                                    CancellationToken token) =>
            Context.SaveChangesAsync(token);

        /// <summary>
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        public Task UnitOfWorkScopedTransactionsAsync(
                                                    Func<CancellationToken, Task> transaction,
                                                    CancellationToken token)
        {
            ThrowErrorIf.
               IsNullValue(
                   transaction,
                   nameof(transaction),
                   nameof(UnitOfWorkScopedTransactionsAsync));

            return ProcessTransactionsAsync(async cancellationToken =>
                {
                    using (var transactionScope =
                    new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {

                        await transaction(cancellationToken).
                        ConfigureAwait(false);

                        await SaveChangesAsync(cancellationToken);
                        transactionScope.Complete();
                    };
                }, token);
        }

        /// <summary>
        /// Units the of work scoped transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task UnitOfWorkScopedTransactionsAsync(
                                                    Action transaction,
                                                    CancellationToken token)
        {
            ThrowErrorIf.
               IsNullValue(
                   transaction,
                   nameof(transaction),
                   nameof(UnitOfWorkScopedTransactionsAsync));

            return ProcessTransactionsAsync(() =>
                {
                    using (var transactionScope =
                    new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {

                        transaction();

                        transactionScope.Complete();
                    };
                }, token);
        }

        /// <summary>
        /// Unit of Work Transactions asynchronous.
        /// All transaction here are inside a Unit Of Work Block:
        /// (Begin transaction) { try{ Your_Transactions; Commit; }catch(Error){ Rollback; Throw;} } 
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        public Task UnitOfWorkTransactionsAsync(
                                            Func<DbContext, CancellationToken, Task> transaction,
                                            CancellationToken token)
        {
            ThrowErrorIf.
               IsNullValue(
                   transaction,
                   nameof(transaction),
                   nameof(UnitOfWorkTransactionsAsync));

            return ProcessTransactionsAsync(async cancellationToken =>
              {
                  using (var contextTransaction = await Context.
                     Database.
                     BeginTransactionAsync(cancellationToken).
                     ConfigureAwait(false))
                  {
                      try
                      {
                          await transaction(Context, cancellationToken).
                                ConfigureAwait(false);

                          await contextTransaction.
                              CommitAsync(cancellationToken).
                              ConfigureAwait(false);
                      }
                      catch
                      {
                          await contextTransaction.
                               RollbackAsync(cancellationToken).
                               ConfigureAwait(false);

                          throw;
                      }
                  };
              }, token);
        }

        /// <summary>
        /// Units the of work transactions asynchronous.
        /// All transaction here are inside a Unit Of Work Block:
        /// (Begin transaction) { try{ Your_Transactions; Commit; }catch(Error){ Rollback; Throw;} }
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public Task UnitOfWorkTransactionsAsync(
                                            Action<DbContext> transaction,
                                            CancellationToken token)
        {
            ThrowErrorIf.
               IsNullValue(
                   transaction,
                   nameof(transaction),
                   nameof(UnitOfWorkTransactionsAsync));

            return ProcessTransactionsAsync(async (cancellationToken) =>
               {
                   using (var contextTransaction = await Context.
                      Database.
                      BeginTransactionAsync(cancellationToken).
                      ConfigureAwait(false))
                   {
                       try
                       {
                           transaction(Context);

                           await contextTransaction.
                               CommitAsync(cancellationToken).
                               ConfigureAwait(false);
                       }
                       catch
                       {
                           await contextTransaction.
                                RollbackAsync(cancellationToken).
                                ConfigureAwait(false);

                           throw;
                       }
                   };
               }, token);
        }

        /// <summary>
        /// Processes the transactions.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private Task ProcessTransactionsAsync(
                                    Func<CancellationToken, Task> transaction,
                                    CancellationToken token) =>
            ExecutionStrategy
                .ExecuteAsync(cancellationToken => transaction(cancellationToken), token);

        /// <summary>
        /// Processes the transactions asynchronous.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private Task ProcessTransactionsAsync(
                                    Action transaction,
                                    CancellationToken token) =>
            ExecutionStrategy
                .ExecuteAsync(cancellationToken => Task.Run(() => transaction(), cancellationToken), token);

        /// <summary>
        /// Gets the execution strategy.
        /// </summary>
        /// <returns></returns>
        private IExecutionStrategy ExecutionStrategy =>
            Context.
                Database.
                CreateExecutionStrategy();
    }
}