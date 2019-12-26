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
        public readonly int SizeListTest = 50;
        private const string Chars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SizeName = 5;
        private readonly Random _random = new Random();
        private string _fakeSearchValue;

        public Expression<Func<FakeObject, bool>> GetExpression(Expression<Func<FakeObject, bool>> expression) =>
            expression;

        public Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            GetExpression(x => x.Value.Contains(_fakeSearchValue));

        public Expression<Func<FakeObject, bool>> GetFakeExpression(FakeObject value) =>
            GetExpression(x => x.Id == value.Id);

        public string GetFakeName() =>
            new string(Enumerable.Repeat(Chars, SizeName).Select(s => s[_random.Next(s.Length)]).ToArray());

        public FakeFilter GetFilterFake() =>
            new FakeFilter { Value = _fakeSearchValue };

        public IEnumerable<FakeObject> GetListFake()
        {
            var numberRandom = _random.Next(SizeListTest - 1);

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

        public IPageConfig GetPageConfigFake() =>
            new PageConfig
            {
                Order = "Value",
                Page = 0,
                Size = 5,
                Sort = Enums.PageSort.ASC
            };
    }
}