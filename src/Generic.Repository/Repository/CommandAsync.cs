using Generic.Repository.Cache;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.ThrowError;

namespace Generic.Repository.Repository
{
    public class CommandAsync<TValue, TContext> : QueryAsync<TValue, TContext>
        where TValue : class
        where TContext : DbContext
    {
        public CommandAsync(TContext context, ICacheRepository cacheService)
            : base(context, cacheService)
        {
        }

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        public virtual async Task<TValue> CreateAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(entity, nameof(entity), nameof(CreateAsync));

            Context.Attach(entity).State = EntityState.Added;

            await SaveChangesAsync(token).
                ConfigureAwait(false);

            return entity;
        }

        public virtual async Task CreateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(entityList, nameof(entityList), nameof(CreateAsync));

            await Context.
                AddRangeAsync(entityList, token).
                ConfigureAwait(false);

            await SaveChangesAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(entity, nameof(entity), nameof(DeleteAsync));

            Context.Remove(entity);

            await SaveChangesAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(entityList, nameof(entityList), nameof(DeleteAsync));

            Context.RemoveRange(entityList);

            await SaveChangesAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(entity, nameof(entity), nameof(UpdateAsync));

            Context.Attach(entity).State = EntityState.Modified;

            await SaveChangesAsync(token).
                ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(entityList, nameof(entityList), nameof(UpdateAsync));

            Context.UpdateRange(entityList);

            await SaveChangesAsync(token).
                ConfigureAwait(false);
        }

        #endregion COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        #region COMMIT

        public async Task MultiTransactionsAsync(
            Action<DbContext> transaction,
            CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(transaction, nameof(transaction), nameof(MultiTransactionsAsync));

            using (var contextTransaction = await Context.Database.BeginTransactionAsync().
                ConfigureAwait(false))
            {
                try
                {
                    transaction(Context);

                    await contextTransaction.CommitAsync().
                        ConfigureAwait(false);
                }
                catch
                {
                    await contextTransaction.RollbackAsync().
                        ConfigureAwait(false);
                    throw;
                }
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
                 Context.SaveChangesAsync(cancellationToken);

        #endregion COMMIT
    }
}