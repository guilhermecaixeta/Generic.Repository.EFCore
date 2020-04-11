using Generic.Repository.Cache;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Repository
{
    public abstract class CommandAsync<TValue, TContext> : QueryAsync<TValue, TContext>
        where TValue : class
        where TContext : DbContext
    {
        protected CommandAsync(TContext context, ICacheRepository cacheService)
            : base(context, cacheService)
        {
        }

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        public virtual async Task<TValue> CreateAsync(
            TValue entity,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullValue(entity, nameof(entity), nameof(CreateAsync));

            Context.Attach(entity).State = EntityState.Added;

            if (!useUnitOfWork)
            {
                await SaveChangesAsync(token).
                    ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(
                    entityList,
                    nameof(entityList),
                    nameof(CreateAsync));

            await Context.
                AddRangeAsync(entityList, token).
                ConfigureAwait(false);

            await SaveChangesAsync(useUnitOfWork, token).
                ConfigureAwait(false);
        }

        public virtual Task DeleteAsync(
            TValue entity,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullValue(
                entity,
                nameof(entity),
                nameof(DeleteAsync));

            Context.Remove(entity);

            return SaveChangesAsync(useUnitOfWork, token);
        }

        public virtual Task DeleteAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(
                    entityList,
                    nameof(entityList),
                    nameof(DeleteAsync));

            Context.RemoveRange(entityList);

            return SaveChangesAsync(useUnitOfWork, token);
        }

        public virtual Task UpdateAsync(
            TValue entity,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullValue(
                    entity,
                    nameof(entity),
                    nameof(UpdateAsync));

            Context.
                Attach(entity).State = EntityState.Modified;

            return SaveChangesAsync(useUnitOfWork, token);
        }

        public virtual Task UpdateAsync(
            IEnumerable<TValue> entityList,
            CancellationToken token,
            bool useUnitOfWork = false)
        {
            ThrowErrorIf.
                IsNullOrEmptyList(
                    entityList,
                    nameof(entityList),
                    nameof(UpdateAsync));

            Context.UpdateRange(entityList);

            return SaveChangesAsync(useUnitOfWork, token);
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private async Task SaveChangesAsync(
            bool useUnitOfWork,
            CancellationToken token)
        {
            if (!useUnitOfWork)
            {
                await SaveChangesAsync(token);
            }
        }

        #endregion COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

    }
}
