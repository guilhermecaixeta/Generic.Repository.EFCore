using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.UnitTest.Model;
using Generic.Repository.UnitTest.Model.DTO;
using Generic.Repository.UnitTest.Model.Filter;
using Generic.Repository.UnitTest.Repository.Commom;
using NUnit.Framework;

namespace Generic.Repository.UnitTest.Repository
{
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

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            _commom.GetFakeExpression();

        protected override FakeFilter GetFilterFake() =>
_commom.GetFilterFake();

        protected override IEnumerable<FakeObject> GetListFake() =>
_commom.GetListFake();

        protected override IPageConfig GetPageConfigFake() =>
_commom.GetPageConfigFake();

        protected override FakeDTO MapperDate(FakeObject value) =>
                                    new FakeDTO { Value = value.Value };

        protected override IEnumerable<FakeDTO> MapperList(IEnumerable<FakeObject> value) =>
            value.Select(MapperDate);

        protected override FakeObject MapperReturnToDate(FakeDTO value)
        {
            throw new NotImplementedException();
        }
    }
}