using Generic.Repository.Extension.List;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.Validations.ThrowError;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplited = list.SplitList(chunkSize);

                    foreach (var enumerable in listSplited)
                    {
                        await repository.DeleteAsync(enumerable, token);
                    }
                }, token);
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
                IsNullOrEmptyList(list, nameof(list), nameof(BulkDeleteAsync));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplited = list.SplitList(chunkSize);

                    foreach (var enumerable in listSplited)
                    {
                        await repository.CreateAsync(enumerable, token);
                    }
                }, token);
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
                IsNullOrEmptyList(list, nameof(list), nameof(BulkDeleteAsync));

            ThrowErrorIf.
                IsLessThanOrEqualsZero(chunkSize);

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplited = list.SplitList(chunkSize);

                    foreach (var enumerable in listSplited)
                    {
                        await repository.UpdateAsync(enumerable, token);
                    }
                }, token);
        }
    }
}