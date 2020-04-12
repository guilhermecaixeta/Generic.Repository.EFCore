using Generic.Repository.Extension.Repository;
using Generic.RepositoryTest.Int.Data;
using Generic.RepositoryTest.Int.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Generic.RepositoryTest.Int.Utils
{
    public static class FakeFactory
    {
        /// <summary>
        /// The size list test
        /// </summary>
        public const int SizeListTest = 10_000;

        /// <summary>
        /// The chars
        /// </summary>
        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// The size name
        /// </summary>
        private const int SizeName = 5;

        /// <summary>
        /// The random
        /// </summary>
        private static Random _random = new Random();

        /// <summary>
        /// Gets the fake.
        /// </summary>
        /// <returns></returns>
        public static FakeInt GetFake() =>
            new FakeInt
            {
                Value = GetFakeValue()
            };

        /// <summary>
        /// Gets the fake value.
        /// </summary>
        /// <returns></returns>
        public static string GetFakeValue() =>
            new string(Enumerable.Repeat(Chars, SizeName).Select(s => s[_random.Next(s.Length)]).ToArray());

        /// <summary>
        /// Gets the list fake.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<FakeInt> GetListFake()
        {
            for (var i = 0; i < SizeListTest; i++)
            {
                var value = GetFakeValue();
                yield return new FakeInt { Value = value };
            }
        }

        /// <summary>
        /// Inserts the data.
        /// </summary>
        public static async Task InsertData()
        {
            var repositoryAsync = DataInjector.GetRepositoryAsync();

            var list = GetListFake();

            await repositoryAsync.
                BulkInsertAsync(list, 10, default).
                ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the fake.
        /// </summary>
        /// <param name="fakeInt">The fake int.</param>
        /// <returns></returns>
        public static FakeInt UpdateFake(FakeInt fakeInt)
        {
            fakeInt.Value = GetFakeValue();
            return fakeInt;
        }

        /// <summary>
        /// Updates the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IEnumerable<FakeInt> UpdateList(IEnumerable<FakeInt> list)
        {
            foreach (var fake in list)
            {
                yield return UpdateFake(fake);
            }
        }
    }
}