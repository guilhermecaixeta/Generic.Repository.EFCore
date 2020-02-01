using Generic.Repository.IntTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.IntTest.Utils
{
    public static class FakeList
    {
        public const int SizeListTest = 50;
        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SizeName = 5;
        private static Random _random = new Random();

        public static string GetFakeName() =>
            new string(Enumerable.Repeat(Chars, SizeName).Select(s => s[_random.Next(s.Length)]).ToArray());

        public static IEnumerable<FakeInt> GetListFake()
        {
            for (var i = 0; i < SizeListTest; i++)
            {
                var value = GetFakeName();
                yield return new FakeInt { Value = value };
            }
        }
    }
}
