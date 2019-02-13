using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EntityFilter;
using Microsoft.EntityFrameworkCore;
namespace IRepository
{
    public interface IBaseRepository<E, F, C>
    where E: class
    where F: BaseFilter
    where C: DbContext
    {
        ///<summary>
        /// Return all data
        ///</summary>
        IQueryable<E> GetAll();
        ///<summary>
        /// Return all data filtred
        ///</summary>
        ///<param name="filter">Filter to apply</param>
        IQueryable<E> FilterAll(F filter);
        ///<summary>
        /// Return all data with pass on the predicate
        ///</summary>
        ///<param name="predicate">Condition to apply on data</param>
        IQueryable<E> GetAllBy(Expression<Func<E, bool>> predicate);
        Task<E> GetByIdAsync(long id);
        Task<E> GetByAsync(Expression<Func<E, bool>> predicate);
        Task<E> CreateAsync(E data);
        Task UpdateAsync(E data);
        Task DeleteAsync(long id);

    }
}