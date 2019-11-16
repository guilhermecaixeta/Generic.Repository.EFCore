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

            RepositoryFacade.SetState(EntityState.Added, entity);

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

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            ThrowErrorIf.IsNullValue(entity, nameof(entity), nameof(UpdateAsync));

            RepositoryFacade.SetState(EntityState.Modified, entity);

            await SaveChangesAsync(token).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            ThrowErrorIf.IsNullOrEmptyList(entityList, nameof(entityList), nameof(UpdateAsync));

            Context.UpdateRange(entityList);

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
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) Without CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity) =>
        await CreateAsync(entity, default).ConfigureAwait(false);

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList) =>
        await CreateAsync(entityList, default).ConfigureAwait(false);

        public virtual async Task UpdateAsync(TValue entity) =>
        await UpdateAsync(entity, default).ConfigureAwait(false);

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList) =>
        await UpdateAsync(entityList, default).ConfigureAwait(false);

        public virtual async Task DeleteAsync(TValue entity) =>
        await DeleteAsync(entity, default).ConfigureAwait(false);

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList) =>
        await DeleteAsync(entityList, default).ConfigureAwait(false);
        #endregion

        #region COMMIT

        public Task SaveChangesAsync() =>
            SaveChangesAsync(default);

        public Task SaveChangesAsync(CancellationToken cancellationToken) =>
         Context.SaveChangesAsync(cancellationToken);

        #endregion
    }
}
