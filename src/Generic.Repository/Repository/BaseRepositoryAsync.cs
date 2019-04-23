using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.Extension.Filter;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.Repository
{
    public class BaseRepositoryAsync<TValue, TFilter> : IBaseRepositoryAsync<TValue, TFilter>
    where TValue : class
    where TFilter : class, IFilter
    {
        #region Attr
        protected readonly DbContext _context;
        private readonly string _nameField;
        private readonly bool _useCommit;
        #endregion

        #region Ctor
        protected BaseRepositoryAsync(DbContext context)
        {
            _context = context;
        }

        protected BaseRepositoryAsync(DbContext context, bool useCommit)
        {
            _useCommit = useCommit;
            _context = context;
        }

        protected BaseRepositoryAsync(DbContext context, string includeDateNameField)
        {
            _nameField.IsNull(nameof(BaseRepositoryAsync<TValue, TFilter>), nameof(_nameField));
            _useCommit = false;
            _context = context;
        }

        protected BaseRepositoryAsync(DbContext context, string includeDateNameField, bool useCommit)
        {
            _nameField.IsNull(nameof(BaseRepositoryAsync<TValue, TFilter>), nameof(_nameField));
            _useCommit = useCommit;
            _context = context;
        }
        #endregion

        #region QUERY
        public virtual async Task<IReadOnlyList<TValue>> GetAll(bool EnableAsNoTracking) => await GetAllQueryable(EnableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> GetAllBy(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => predicate != null ?
            await GetAllQueryable(EnableAsNoTracking).Where(predicate).ToListAsync() : await GetAllQueryable(EnableAsNoTracking).ToListAsync();

        public virtual async Task<IReadOnlyList<TValue>> FilterAll(TFilter filter, bool EnableAsNoTracking) => await GetAllBy(filter.GenerateLambda<TValue, TFilter>(), EnableAsNoTracking);

        public virtual async Task<TValue> GetByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => !predicate.IsNull(nameof(GetByAsync), nameof(predicate)) &&
        EnableAsNoTracking ? await _context.Set<TValue>().AsNoTracking().SingleOrDefaultAsync(predicate) : await _context.Set<TValue>().SingleOrDefaultAsync(predicate);

        private IQueryable<TValue> GetAllQueryable(bool EnableAsNoTracking) => EnableAsNoTracking ? _context.Set<TValue>().AsNoTracking() : _context.Set<TValue>();
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(CreateAsync), nameof(entity));
            SetState(EntityState.Added, entity);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.IsNull(nameof(CreateAsync), nameof(entityList));
            await _context.AddRangeAsync(entityList);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(UpdateAsync), nameof(entity));
            SetState(EntityState.Modified, entity);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.IsNull(nameof(UpdateAsync), nameof(entityList));
            _context.UpdateRange(entityList);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TValue entity, CancellationToken token)
        {
            entity.IsNull(nameof(DeleteAsync), nameof(entity));
            _context.Remove(entity);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token)
        {
            entityList.IsNull(nameof(DeleteAsync), nameof(entityList));
            _context.RemoveRange(entityList);
            if (!_useCommit)
            {
                await CommitAsync(token).ConfigureAwait(false);
            }
        }
        #endregion

        #region COMMAND - (CREAT, UPDATE, DELETE) Without CancellationToken
        public virtual async Task<TValue> CreateAsync(TValue entity) => await CreateAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task CreateAsync(IEnumerable<TValue> entityList) => await CreateAsync(entityList, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task UpdateAsync(TValue entity) => await UpdateAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task UpdateAsync(IEnumerable<TValue> entityList) => await UpdateAsync(entityList, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task DeleteAsync(TValue entity) => await DeleteAsync(entity, default(CancellationToken)).ConfigureAwait(false);

        public virtual async Task DeleteAsync(IEnumerable<TValue> entityList) => await DeleteAsync(entityList, default(CancellationToken)).ConfigureAwait(false);
        #endregion

        #region COMMIT
        public Task CommitAsync() => CommitAsync(default(CancellationToken));

        public Task CommitAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
        #endregion

        #region Private Methods
        private void SetState(EntityState state, TValue item) => _context.Attach(item).State = state;
        #endregion
    }
}
