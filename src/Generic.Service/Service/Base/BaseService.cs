using Generic.Service.Entity.IFilter;
using Generic.Service.Extensions.Filter;
using Generic.Service.Extensions.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Service.Base
{
    ///<summary>
    /// This is a Base Service implementation which any entity or Service should be have.
    /// TValue - Entity Type, F inheritance of IFilter or IFilter
    ///</summary>
    public abstract class BaseService<TValue, TFilter> : IBaseService<TValue, TFilter>
        where TValue : class
    where TFilter : class, IFilter
    {
        protected readonly DbContext _context;
        private readonly string _includeDateNameField;
        private readonly bool _useCommit;

        protected BaseService(DbContext context)
        {
            _context = context;
            _useCommit = false;
            _includeDateNameField = "dateInclusion";

        }

        protected BaseService(DbContext context, bool useCommit)
        {
            _includeDateNameField = "dateInclusion";
            _useCommit = useCommit;
            _context = context;
        }

        protected BaseService(DbContext context, string includeDateNameField)
        {
            _includeDateNameField = includeDateNameField ??
                throw new ArgumentNullException(nameof(includeDateNameField));
            _useCommit = false;
            _context = context;
        }

        protected BaseService(DbContext context, string includeDateNameField, bool useCommit)
        {
            _includeDateNameField.IsNull(nameof(BaseService<TValue, TFilter>), nameof(_includeDateNameField));
            _useCommit = useCommit;
            _context = context;
        }

        public virtual IQueryable<TValue> GetAll(bool EnableAsNoTracking) => EnableAsNoTracking ? _context.Set<TValue>().AsNoTracking() : _context.Set<TValue>();

        public virtual IQueryable<TValue> GetAllBy(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => predicate != null ?
            GetAll(EnableAsNoTracking) : GetAll(EnableAsNoTracking);

        public virtual IQueryable<TValue> FilterAll(TFilter filter, bool EnableAsNoTracking)
        {
            var predicate = filter.GenerateLambda<TValue, TFilter>();
            return predicate == null ? GetAll(EnableAsNoTracking) : GetAllBy(predicate, EnableAsNoTracking);
        }

        public virtual async Task<TValue> GetByAsync(Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => !predicate.IsNull(nameof(GetByAsync), nameof(predicate)) &&
        EnableAsNoTracking ? await _context.Set<TValue>().AsNoTracking().SingleOrDefaultAsync(predicate) : await _context.Set<TValue>().SingleOrDefaultAsync(predicate);

        public virtual async Task<TValue> CreateAsync(TValue entity)
        {
            entity.IsNull(nameof(CreateAsync), nameof(entity));
            SetState(EntityState.Added, entity);
            if (!_useCommit)
            {
                await CommitAsync().ConfigureAwait(false);
            }
            return entity;
        }

        public virtual async Task UpdateAsync(TValue entity)
        {
            entity.IsNull(nameof(UpdateAsync), nameof(entity));
            SetState(EntityState.Modified, entity);
            if (!_useCommit)
            {
                await CommitAsync().ConfigureAwait(false);
            }
        }

        public virtual async Task DeleteAsync(TValue entity)
        {
            entity.IsNull(nameof(DeleteAsync), nameof(entity));
            _context.Remove(entity);
            if (!_useCommit)
            {
                await CommitAsync();
            }
        }

        private void SetState(EntityState state, TValue item) => _context.Entry(item).State = state;

        public Task CommitAsync() => CommitAsync(default(CancellationToken));

        public Task CommitAsync(CancellationToken cancellationToken) => _context.SaveChangesAsync(cancellationToken);
    }
}