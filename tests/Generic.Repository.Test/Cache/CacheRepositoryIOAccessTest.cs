﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Generic.Repository.Test.Cache
{
    public abstract class CacheRepositoryIOAccessTest<T>
        : CacheRepositoryExceptionTest<T>
        where T : class
    {
        [Test]
        public async Task CacheAccess_StressInput()
        {
            var valid = true;
            for (int j = 0; j <= 10; j++)
            {
                var listaTarefas = new List<Task>();
                try
                {
                    var tarefaGeral = Task.Run(async () =>
                    {
                        var listaTarefaIO = new List<Task>();

                        for (int i = 0; i <= 10; i++)
                        {
                            Cache.ClearCache();

                            var tarefa = Task.Run(async () => await Cache.AddGet<T>(default));
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () => await Cache.AddSet<T>(default));
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () => await Cache.AddProperty<T>(default));
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () => await Cache.AddAttribute<T>(default));
                            listaTarefaIO.Add(tarefa);
                        }

                        await Task.WhenAll(listaTarefaIO);
                    });

                    listaTarefas.Add(tarefaGeral);
                }
                catch (Exception)
                {
                    valid = false;
                }

                await Task.WhenAll(listaTarefas);
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

            for (int j = 0; j <= 10; j++)
            {
                var listaTarefas = new List<Task>();
                try
                {
                    var tarefaGeral = Task.Run(async () =>
                    {
                        var listaTarefaIO = new List<Task>();

                        for (int i = 0; i <= 10; i++)
                        {
                            var tarefa = Task.Run(async () =>
                            {
                                var result = await Cache.GetMethodGet(NameType, NameProperty, default);
                                CheckIfIsValid(result);
                            });
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () =>
                            {
                                var result = await Cache.GetMethodSet(NameType, NameProperty, default);
                                CheckIfIsValid(result);
                            });
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () =>
                            {
                                var result = await Cache.GetProperty(NameType, NameProperty, default);
                                CheckIfIsValid(result);
                            });
                            listaTarefaIO.Add(tarefa);

                            tarefa = Task.Run(async () =>
                            {
                                var result = await Cache.GetAttribute(NameType, NameProperty, NameAttribute, default);
                                CheckIfIsValid(result);
                            });
                            listaTarefaIO.Add(tarefa);
                        }

                        await Task.WhenAll(listaTarefaIO);
                    });

                    listaTarefas.Add(tarefaGeral);
                }
                catch (Exception)
                {
                    valid = false;
                }

                await Task.WhenAll(listaTarefas);
            }

            Assert.IsTrue(valid);
        }
    }
}