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
    public class BaseRepositoryAsyncTest : BaseRepositoryAsyncQueryTest<FakeObject, FakeFilter>
    {
        private string fakeSearchValue;
        public BaseRepositoryAsyncTest()
        {
            count = 10;
            count2 = 5;
            count3 = 1;
        }

        protected override FakeObject CreateFakeValue() => new FakeObject { Name = "Test_1" };

        protected override FakeObject UpdateFakeValue(FakeObject value)
        {
            value.Name = "Test_2";
            return value;
        }

        protected override Expression<Func<FakeObject, bool>> ExpressionGeneric(FakeObject value)
            => GetExpression(x => x.Id == value.Id);

        internal override IPageConfig GetPageConfig()
        => new PageConfig { order = "Name", page = 0, size = 5, sort = "ASC" };

        internal override FakeFilter GetFilter()
        => new FakeFilter { Name = fakeSearchValue };

        internal override Expression<Func<FakeObject, bool>> ExpressionGeneric()
        => GetExpression(x => fakeSearchValue.Contains(x.Name));

        private Expression<Func<FakeObject, bool>> GetExpression(
            Expression<Func<FakeObject, bool>> expression) => expression;

        internal override IEnumerable<FakeObject> GetListFake()
        {
            bool checkout = false;
            for (int i = 0; i < 10; i++)
            {
                var name = GetFakeName();
                if (!checkout)
                {
                    fakeSearchValue = name;
                    checkout = true;
                }
                yield return new FakeObject { Name = name };
            }
        }

        private string GetFakeName()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal override void SaveList(IEnumerable<FakeObject> list)
        {
            _repository.CreateAsync(list).Wait();
        }

    }
}
