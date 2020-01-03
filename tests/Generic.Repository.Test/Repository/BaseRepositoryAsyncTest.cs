using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.UnitTest.Model;
using Generic.Repository.UnitTest.Model.Filter;
using Generic.Repository.UnitTest.Repository.Commom;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Repository
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