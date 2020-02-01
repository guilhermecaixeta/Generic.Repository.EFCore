using Generic.Repository.Extension.List;
using Generic.Repository.Interfaces.Repository;
using Generic.Repository.ThrowError;
using Microsoft.EntityFrameworkCore;
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
        public static async Task BulkDeleteAsync<TValue, TContext>(
            this IBaseRepositoryAsync<TValue, TContext> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
            where TContext : DbContext
        {
            ThrowErrorIf.IsNullValue(repository, nameof(repository), nameof(BulkDeleteAsync));

            ThrowErrorIf.IsNullOrEmptyList(list, nameof(list), nameof(BulkDeleteAsync));

            ThrowErrorIf.IsLessThanOrEqualsZero(chunkSize);

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplit = list.SplitList(chunkSize);

                    foreach (var split in listSplit)
                    {
                        await Task.
                            Run(() => ctx.Set<TValue>().RemoveRange(split), token).
                            ConfigureAwait(false);
                    }
                }, token).ConfigureAwait(false);
        }

        /// <summary>Bulks the insert asynchronous.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        public static async Task BulkInsertAsync<TValue, TContext>(
            this IBaseRepositoryAsync<TValue, TContext> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
            where TContext : DbContext
        {
            ThrowErrorIf.IsNullValue(repository, nameof(repository), nameof(BulkInsertAsync));

            ThrowErrorIf.IsNullOrEmptyList(list, nameof(list), nameof(BulkInsertAsync));

            ThrowErrorIf.IsLessThanOrEqualsZero(chunkSize);

            var listSplit = list.SplitList(chunkSize);

            await repository.MultiTransactionsAsync(
            async ctx =>
            {
                var i = 0;
                //await ctx.SaveChangesAsync(token);
                foreach (var split in listSplit)
                {
                    await ctx.Set<TValue>().AddRangeAsync(split, token).
                                    ConfigureAwait(false);

                    await ctx.SaveChangesAsync(token).
                        ConfigureAwait(false);
                }
            }, token).
            ConfigureAwait(false);
        }

        /// <summary>Bulks the update asynchronous.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="list">The list.</param>
        /// <param name="chunkSize">Size of the chunk.</param>
        /// <param name="token">The token.</param>
        public static async Task BulkUpdateAsync<TValue, TContext>(
            this IBaseRepositoryAsync<TValue, TContext> repository,
            IEnumerable<TValue> list,
            int chunkSize,
            CancellationToken token)
            where TValue : class
            where TContext : DbContext
        {
            ThrowErrorIf.IsNullValue(repository, nameof(repository), nameof(BulkUpdateAsync));

            ThrowErrorIf.IsNullOrEmptyList(list, nameof(list), nameof(BulkUpdateAsync));

            ThrowErrorIf.IsLessThanOrEqualsZero(chunkSize);

            await repository.MultiTransactionsAsync(
                async ctx =>
                {
                    var listSplit = list.SplitList(chunkSize);

                    foreach (var split in listSplit)
                    {
                        ctx.Set<TValue>().UpdateRange(split);
                    }
                }, token).ConfigureAwait(false);
        }
    }
}