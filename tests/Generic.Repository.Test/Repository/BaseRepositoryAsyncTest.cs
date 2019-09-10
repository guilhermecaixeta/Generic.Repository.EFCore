using Generic.Repository.Test.Repository.Commom;

namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Page.PageConfig;
    using Generic.Repository.Test.Model;
    using Generic.Repository.Test.Model.Filter;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

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

        internal override IPageConfig GetPageConfigFake() =>
            Commom.GetPageConfigFake();

        internal override FakeFilter GetFilterFake() =>
            Commom.GetFilterFake();

        internal override IEnumerable<FakeObject> GetListFake() =>
            Commom.GetListFake();

        internal override Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            Commom.GetFakeExpression();

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression(FakeObject value) =>
            Commom.GetFakeExpression(value);

        protected override FakeObject CreateFakeValue() =>
            new FakeObject
            {
                Value = Commom.GetFakeName()
            };

        protected override FakeObject UpdateFakeValue(FakeObject value)
        {
            var data = Commom.GetFakeName();

            if (data.Equals(value.Value))
            {
                UpdateFakeValue(value);
            }

            value.Value = data;
            return value;
        }
    }
}
