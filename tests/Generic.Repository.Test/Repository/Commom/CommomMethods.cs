using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.Test.Model;
using Generic.Repository.Test.Model.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Generic.Repository.Test.Repository.Commom
{
    internal class CommomMethods
    {
        private string _fakeSearchValue;

        public readonly int SizeListTest = 50;

        private const int SizeName = 5;

        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private readonly Random _random = new Random();

        public FakeFilter GetFilterFake() =>
            new FakeFilter { Value = _fakeSearchValue };

        public IPageConfig GetPageConfigFake() =>
            new PageConfig
            {
                Order = "Value",
                Page = 0,
                Size = 5,
                Sort = Enums.PageSort.ASC
            };

        public Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            GetExpression(x => _fakeSearchValue.Contains(x.Value));

        public IEnumerable<FakeObject> GetListFake()
        {
            var numberRandom = _random.Next(SizeListTest);

            for (var i = 0; i < SizeListTest; i++)
            {
                var value = GetFakeName();
                if (numberRandom == i)
                {
                    _fakeSearchValue = value;
                }
                yield return new FakeObject { Value = value };
            }
        }

        public Expression<Func<FakeObject, bool>> GetFakeExpression(FakeObject value) =>
            GetExpression(x => x.Id == value.Id);

        public Expression<Func<FakeObject, bool>> GetExpression(Expression<Func<FakeObject, bool>> expression) =>
            expression;

        public string GetFakeName() =>
            new string(Enumerable.Repeat(Chars, SizeName)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
