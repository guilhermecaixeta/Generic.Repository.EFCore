namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Attributes;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.Page.PageConfig;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class FakeObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FakeFilter : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Contains)]
        public string Name { get; set; }
    }

    [TestFixture]
    public class BaseRepositoryAsyncTest :
        BaseRepositoryAsyncQueryTest<FakeObject, FakeFilter>
    {
        private string _fakeSearchValue;
        public BaseRepositoryAsyncTest()
        {
            ComparableListLength = 10;
            ComparablePageLength = 5;
            ComparablePageFilterResult = 1;
        }

        internal override IPageConfig GetFakePageConfig()
            => new PageConfig { order = "Name", page = 0, size = 5, sort = "ASC" };

        internal override FakeFilter GetFakeFilter()
        => new FakeFilter { Name = _fakeSearchValue };

        internal override Expression<Func<FakeObject, bool>> GetFakeExpression()
        => GetExpression(x => _fakeSearchValue.Contains(x.Name));

        internal override IEnumerable<FakeObject> GetListFake()
        {
            bool checkout = false;
            for (int i = 0; i < 10; i++)
            {
                var name = GetFakeName();
                if (!checkout)
                {
                    _fakeSearchValue = name;
                    checkout = true;
                }
                yield return new FakeObject { Name = name };
            }
        }

        protected override FakeObject CreateFakeValue() =>
            new FakeObject { Name = GetFakeName() };

        protected override FakeObject UpdateFakeValue(FakeObject value)s
        {
            var name = GetFakeName();
            if (!name.Equals(value.Name))
            {
                value.Name = name;
            }
            else
            {
                UpdateFakeValue(value);
}
            return value;
        }

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression(FakeObject value)
            => GetExpression(x => x.Id == value.Id);

private Expression<Func<FakeObject, bool>> GetExpression(
    Expression<Func<FakeObject, bool>> expression) => expression;

private static string GetFakeName()
{
    var random = new Random();

    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

    return new string(Enumerable.Repeat(chars, 3)
      .Select(s => s[random.Next(s.Length)]).ToArray());
}

    }
}
