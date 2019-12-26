using System;
using Generic.Repository.Cache;
using Generic.Repository.Validations.ThrowError;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(CreateAsync));

            Context.Attach(entity).State = EntityState.Added;

            await SaveChangesAsync(token).ConfigureAwait(false);

            return entity;
        }

        public virtual async Task CreateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(CreateAsync));

            await Context.AddRangeAsync(entityList, token).ConfigureAwait(false);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(DeleteAsync));

            Context.Remove(entity);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(DeleteAsync));

            Context.RemoveRange(entityList);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(UpdateAsync));

            Context.Attach(entity).State = EntityState.Modified;

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(UpdateAsync));

            Context.UpdateRange(entityList);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        #endregion COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        #region COMMIT

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
         Context.SaveChangesAsync(cancellationToken);

        public async Task TransactionAsync(Action<DbSet<TValue>> transaction, CancellationToken token)
        {
            ThrowErrorIf.
                IsNullValue(transaction, nameof(transaction), nameof(TransactionAsync));

            using (var contextTransaction = await Context.Database.BeginTransactionAsync().
                ConfigureAwait(false))
            {
                try
                {
                    transaction(Context.Set<TValue>());

                    await contextTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    await contextTransaction.RollbackAsync();
                }
            }
        }

        #endregion COMMIT
    }
}