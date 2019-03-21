using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Generic.Service.Entity.IFilter;
using Generic.Service.Extensions.Filter;
using Microsoft.EntityFrameworkCore;

namespace Generic.Service.Base
{
    ///<summary>
    /// This is a Base Service implementation which any entity or Service should be have.
    /// TValue - Entity Type, F inheritance of IBaseFilter or IBaseFilter
    ///</summary>
    public abstract class BaseService<TValue, TFilter> : IBaseService<TValue, TFilter>
        where TValue : class
    where TFilter : class, IBaseFilter {
        protected readonly DbContext _context;
        private readonly string _includeDateNameField;
        private readonly bool _useCommit;

        public BaseService (DbContext context) {
            _context = context;
            _useCommit = false;
            _includeDateNameField = "dateInclusion";

        }

        public BaseService (DbContext context, bool useCommit) {
            _includeDateNameField = "dateInclusion";
            _useCommit = useCommit;
            _context = context;
        }

        public BaseService (DbContext context, string includeDateNameField) {
            _includeDateNameField = includeDateNameField ??
                throw new ArgumentNullException (nameof (includeDateNameField));
            _useCommit = false;
            _context = context;
        }

        public BaseService (DbContext context, string includeDateNameField, bool useCommit) {
            _includeDateNameField = includeDateNameField ??
                throw new ArgumentNullException (nameof (includeDateNameField));
            _useCommit = useCommit;
            _context = context;
        }

        public virtual IQueryable<TValue> GetAll (bool EnableAsNoTracking) => EnableAsNoTracking ? _context.Set<TValue> ().AsNoTracking () : _context.Set<TValue> ();

        public virtual IQueryable<TValue> GetAllBy (Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => predicate != null ?
            GetAll (EnableAsNoTracking).Where (predicate) : GetAll (EnableAsNoTracking);

        public virtual IQueryable<TValue> FilterAll (TFilter filter, bool EnableAsNoTracking) => GetAllBy (filter.GenerateLambda<TValue, TFilter> (), EnableAsNoTracking);

        public virtual async Task<TValue> GetByAsync (Expression<Func<TValue, bool>> predicate, bool EnableAsNoTracking) => EnableAsNoTracking? await _context.Set<TValue> ().AsNoTracking ().SingleOrDefaultAsync (predicate) : await _context.Set<TValue> ().SingleOrDefaultAsync (predicate);

        public virtual async Task<TValue> CreateAsync (TValue entity) {
            SetState (EntityState.Added, entity);
            if (!_useCommit)
                await CommitAsync ();
            return entity;
        }

        public virtual async Task UpdateAsync (TValue entity) {
            SetState (EntityState.Modified, entity);
            if (!_useCommit)
                await CommitAsync ();
        }

        public virtual async Task DeleteAsync (TValue entity) {
            _context.Remove (entity);
            if (!_useCommit)
                await CommitAsync ();
        }

        private void SetState (EntityState state, TValue item) => _context.Entry<TValue> (item).State = state;

        public Task CommitAsync (CancellationToken cancellationToken = default (CancellationToken)) => _context.SaveChangesAsync (cancellationToken);

    }
}