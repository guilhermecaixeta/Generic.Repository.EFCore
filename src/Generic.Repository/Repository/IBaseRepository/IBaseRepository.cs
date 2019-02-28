using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Generic.Repository.Entity.IFilter;
using Microsoft.EntityFrameworkCore;

namespace Generic.Repository.Base
{
    public interface IBaseRepository<E, F>
    where E : class
    where F : IBaseFilter
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
        /// <summary>
        /// Get data by id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>Data with id matches</returns>
        Task<E> GetByIdAsync(long id);
        /// <summary>
        /// Get first data where predicate matches
        /// </summary>
        /// <param name="predicate">Predicate to find data</param>
        /// <returns>First data matches</returns>
        Task<E> GetByAsync(Expression<Func<E, bool>> predicate);
        /// <summary>
        /// Save data on database
        /// </summary>
        /// <param name="data">data to persist</param>
        /// <returns>data</returns>
        Task<E> CreateAsync(E data);
        /// <summary>
        /// Update data on database
        /// </summary>
        /// <param name="data">data to updated</param>
        /// <returns>data</returns>
        Task UpdateAsync(E data);
        /// <summary>
        /// Delete data by id
        /// </summary>
        /// <param name="id">long</param>
        /// <returns>Task</returns>
        Task DeleteAsync(long id);

    }
}