using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Repository.UnitTest.Cache
{
    public abstract class CacheRepositoryIOAccessTest<T>
        : CacheRepositoryExceptionTest<T>
        where T : class
    {
        private const int MaxIterations = 10;

        [Test]
        public async Task CacheAccess_StressInput()
        {
            var valid = true;
            for (var j = 0; j <= MaxIterations; j++)
            {
                var scheduledListTask = new List<Task>();
                try
                {
                    var mainTask = Task.Run(async () =>
                    {
                        var listTaskIO = new List<Task>();

                        for (var i = 0; i <= MaxIterations; i++)
                        {
                            Cache.ClearCache();

                            var @task = Task.Run(async () => await Cache.AddGet<T>(default).ConfigureAwait(false));
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () => await Cache.AddSet<T>(default).ConfigureAwait(false));
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () => await Cache.AddProperty<T>(default).ConfigureAwait(false));
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () => await Cache.AddAttribute<T>(default).ConfigureAwait(false));
                            listTaskIO.Add(@task);
                        }

                        await Task.WhenAll(listTaskIO);
                    });

                    scheduledListTask.Add(mainTask);

                    await Task.WhenAll(scheduledListTask);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            Assert.IsTrue(valid);
        }

        [Test]
        public async Task CacheAccess_StressOutput()
        {
            var valid = true;

            void CheckIfIsValid(object result)
            {
                if (!valid)
                {
                    return;
                }
                valid = result != null;
            }

            for (var j = 0; j <= MaxIterations; j++)
            {
                var scheduledListTask = new List<Task>();
                try
                {
                    var mainTask = Task.Run(async () =>
                    {
                        var listTaskIO = new List<Task>();

                        for (var i = 0; i <= MaxIterations; i++)
                        {
                            var @task = Task.Run(async () =>
                            {
                                var result = await Cache.GetMethodGet(NameType, NameProperty, default).ConfigureAwait(false);
                                CheckIfIsValid(result);
                            });
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () =>
                            {
                                var result = await Cache.GetMethodSet(NameType, NameProperty, default).ConfigureAwait(false);
                                CheckIfIsValid(result);
                            });
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () =>
                            {
                                var result = await Cache.GetProperty(NameType, NameProperty, default).ConfigureAwait(false);
                                CheckIfIsValid(result);
                            });
                            listTaskIO.Add(@task);

                            @task = Task.Run(async () =>
                            {
                                var result = await Cache.GetAttribute(NameType, NameProperty, NameAttribute, default).ConfigureAwait(false);
                                CheckIfIsValid(result);
                            });
                            listTaskIO.Add(@task);
                        }

                        await Task.WhenAll(listTaskIO);
                    });

                    scheduledListTask.Add(mainTask);
                }
                catch (Exception)
                {
                    valid = false;
                }

                await Task.WhenAll(scheduledListTask);
            }

            Assert.IsTrue(valid);
        }
    }
}