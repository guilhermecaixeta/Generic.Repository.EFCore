namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Models.Page.PageConfig;
    using Generic.Repository.Test.Model;
    using Generic.Repository.Test.Model.DTO;
    using Generic.Repository.Test.Model.Filter;
    using Generic.Repository.Test.Repository.Commom;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    [TestFixture]
    public class BaseRepositoryMapAsyncTest :
        BaseRepositoryMapAsyncQueryTest<FakeObject, FakeDTO, FakeFilter>
    {
        private readonly CommomMethods _commom = new CommomMethods();

        public BaseRepositoryMapAsyncTest()
        {
            ComparableListLength = _commom.SizeListTest;
            ComparablePageLength = 5;
            ComparablePageFilterResult = 1;
        }

        protected override FakeDTO MapperDate(FakeObject value) =>
            new FakeDTO { Value = value.Value };

        protected override IEnumerable<FakeDTO> MapperList(IEnumerable<FakeObject> value) =>
            value.Select(MapperDate);


        protected override IPageConfig GetPageConfigFake() =>
            _commom.GetPageConfigFake();

        protected override FakeFilter GetFilterFake() =>
            _commom.GetFilterFake();

        protected override IEnumerable<FakeObject> GetListFake() =>
            _commom.GetListFake();

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            _commom.GetFakeExpression();
    }
}
