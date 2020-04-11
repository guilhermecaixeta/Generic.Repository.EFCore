using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Interfaces.Repository
{
    public interface ICommandRepository<TValue>
        where TValue : class
    {
        #region COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task<TValue> CreateAsync(TValue entity, CancellationToken token, bool useUnitOfWork = false);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task CreateAsync(IEnumerable<TValue> entityList, CancellationToken token, bool useUnitOfWork = false);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task DeleteAsync(TValue entity, CancellationToken token, bool useUnitOfWork = false);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task DeleteAsync(IEnumerable<TValue> entityList, CancellationToken token, bool useUnitOfWork = false);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task UpdateAsync(TValue entity, CancellationToken token, bool useUnitOfWork = false);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <param name="token">The token.</param>
        /// <param name="useUnitOfWork">if set to <c>true</c> [use unit of work].</param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TValue> entityList, CancellationToken token, bool useUnitOfWork = false);

        #endregion COMMAND - (CREAT, UPDATE, DELETE) With CancellationToken
    }
}
