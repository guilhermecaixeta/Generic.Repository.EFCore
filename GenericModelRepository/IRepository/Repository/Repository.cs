using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityFilter;
using Microsoft.EntityFrameworkCore;

namespace IRepository
{
    public abstract class BaseRepository<E, F, C>
    where E: class
    where F: BaseFilter
    where C: DbContext
    {
        protected readonly C _context;
        private readonly string _dataInclusionNameField; 
        public BaseRepository(C context)
        {
            _context = context;
            _dataInclusionNameField = "dateInclusion";
        }

        public BaseRepository(C context, string dataInclusionNameField)
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

        public virtual async Task<E> GetByIdAsync(long id) => await _context.Set<E>().FindAsync(id);

        public virtual async Task<E> GetByAsync(Expression<Func<E, bool>> predicate) => await _context.Set<E>().FirstOrDefaultAsync(predicate);

        public virtual async Task<E> CreateAsync()
        {
            SetDateInclusion();
            var item = ReturnE();
            SetState(EntityState.Added, item);
            await _context.SaveChangesAsync();
            return item;
        }

        private E ReturnE() => (E)Convert.ChangeType(this, typeof(E));

        private void SetState(EntityState state, E item) => _context.Entry<E>(item).State = state;
        
        public virtual async Task DeleteAsync(long id)
        {
            _context.Remove(await GetByIdAsync(id));
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync()
        {
            var item = ReturnE();
            SetState(EntityState.Modified, item);
            await _context.SaveChangesAsync();
        }

        public void SetDateInclusion()
        {
            if(this.GetType().GetProperties().FirstOrDefault(x => x.Name.Equals(_dataInclusionNameField)) != null)
                this.GetType().GetProperty(_dataInclusionNameField).SetValue(this, DateTime.Now);
        }

        public abstract IQueryable<E> FilterAll(F filter);
    }
}