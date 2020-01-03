using System;
using Generic.Repository.Extension.List;
using Generic.Repository.Interfaces.Repository;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Generic.Repository.ThrowError;

namespace Generic.Repository.Extension.Repository
{
    public static class Command
    {
        /// <summary>Bulks the delete asynchronous.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        public static async Task BulkDeleteAsync<TValue>(
            this IBaseRepositoryAsync<TValue> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
        {
            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(BulkDeleteAsync));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.
                ProcessInternalTask(repository.DeleteAsync, list, chunkSize, token);
        }

        /// <summary>Bulks the insert asynchronous.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        public static async Task BulkInsertAsync<TValue>(
            this IBaseRepositoryAsync<TValue> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
        {
            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(BulkInsertAsync));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.
                ProcessInternalTask(repository.CreateAsync, list, chunkSize, token);
        }

        /// <summary>Bulks the update asynchronous.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        public static async Task BulkUpdateAsync<TValue>(
            this IBaseRepositoryAsync<TValue> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
        {
            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(BulkUpdateAsync));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.
                ProcessInternalTask(repository.UpdateAsync, list, chunkSize, token);
        }

        /// <summary>Processes the internal task.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="funcCrud">The function crud.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        internal static async Task ProcessInternalTask<TValue>(
            this IBaseRepositoryAsync<TValue> repository,
            Func<IEnumerable<TValue>, CancellationToken, Task> funcCrud,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
        {
            ThrowErrorIf.
                IsNullValue(funcCrud, nameof(funcCrud), nameof(ProcessInternalTask));

            ThrowErrorIf.
                IsNullOrEmptyList(list, nameof(list), nameof(ProcessInternalTask));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplit = list.SplitList(chunkSize);

                    foreach (var split in listSplit)
                    {
                        await funcCrud(split, token);
                    }
                }, token);
        }
    }
}