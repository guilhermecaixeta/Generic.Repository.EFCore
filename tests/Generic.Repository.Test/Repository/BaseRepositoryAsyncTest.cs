using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.RepositoryTest.Unit.Model;
using Generic.RepositoryTest.Unit.Model.Filter;
using Generic.RepositoryTest.Unit.Repository.Commom;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Generic.RepositoryTest.Unit.Repository
{
    [TestFixture]
    public class BaseRepositoryAsyncTest :
        BaseRepositoryExceptionTest<FakeObject, FakeFilter>
    {
        private readonly CommomMethods Commom = new CommomMethods();

        public BaseRepositoryAsyncTest()
        {
            ComparableListLength = Commom.SizeListTest;
            ComparablePageLength = 5;
            ComparablePageFilterResult = 1;
        }

        internal override Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            Commom.GetFakeExpression();

        internal override FakeFilter GetFilterFake() =>
            Commom.GetFilterFake();

        internal override IEnumerable<FakeObject> GetListFake() =>
            Commom.GetListFake();

        internal override IPageConfig GetPageConfigFake() =>
                                    Commom.GetPageConfigFake();

        protected override FakeObject CreateFakeValue() =>
            new FakeObject
            {
                Value = Commom.GetFakeName()
            };

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression(FakeObject value) =>
                    Commom.GetFakeExpression(value);

        protected override FakeObject UpdateFakeValue(FakeObject value)
        {
            var data = Commom.GetFakeName();

            if (data.Equals(value.Value))
            {
                _ = UpdateFakeValue(value);
            }

            value.Value = data;
            return value;
        }
    }
}