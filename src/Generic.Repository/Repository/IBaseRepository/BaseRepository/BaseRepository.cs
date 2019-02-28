using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Repository.Entity.IFilter;
using Microsoft.EntityFrameworkCore;
using Generic.Repository.Extension.Repository;

namespace Generic.Repository.Base
{
    public abstract class BaseRepository<E, F>
    where E: class
    where F: IBaseFilter
    {
        protected readonly DbContext _context;
        private readonly string _dataInclusionNameField; 
        public BaseRepository(DbContext context)
        {
            _context = context;
            _dataInclusionNameField = "dateInclusion";
        }

        public BaseRepository(DbContext context, string dataInclusionNameField)
        {
            _context = context;
            _dataInclusionNameField = dataInclusionNameField;
        }

        public virtual IQueryable<E> GetAll() => _context.Set<E>().AsNoTracking();

        public virtual IQueryable<E> GetAllBy(Expression<Func<E, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return GetAll().Where(predicate);
        }

        public IQueryable<E> FilterAll(F filter) => GetAll().Where(filter.GenerateLambda<E, F>());

        public virtual async Task<E> GetByIdAsync(long id) => await _context.Set<E>().FindAsync(id);

        public virtual async Task<E> GetByAsync(Expression<Func<E, bool>> predicate) => await _context.Set<E>().FirstOrDefaultAsync(predicate);

        public virtual async Task<E> CreateAsync(E data)
        {
            SetDateInclusion();
            SetState(EntityState.Added, data);
            await _context.SaveChangesAsync();
            return data;
        }

        private void SetState(EntityState state, E item) => _context.Entry<E>(item).State = state;
        
        public virtual async Task DeleteAsync(long id)
        {
            _context.Remove(await GetByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(E data)
        {
            SetState(EntityState.Modified, data);
            await _context.SaveChangesAsync();
        }

        public void SetDateInclusion()
        {
            if(this.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(_dataInclusionNameField)) != null)
                this.GetType().GetProperty(_dataInclusionNameField).SetValue(this, DateTime.Now);
        }

    }
}