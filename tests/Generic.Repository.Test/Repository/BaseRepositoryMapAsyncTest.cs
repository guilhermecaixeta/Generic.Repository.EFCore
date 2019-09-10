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

namespace Generic.Repository.Test.Repository
{
    [TestFixture]
    public class BaseRepositoryMapAsyncTest :
        BaseRepositoryMapAsyncQueryTest<FakeObject, FakeDTO, FakeFilter>
    {
        private readonly CommomMethods Commom = new CommomMethods();

        public BaseRepositoryMapAsyncTest()
        {
            ComparableListLength = Commom.SizeListTest;
            ComparablePageLength = 5;
            ComparablePageFilterResult = 1;
        }

        protected override FakeDTO MapperDate(FakeObject value) =>
            new FakeDTO { Value = value.Value };

        protected override IEnumerable<FakeDTO> MapperList(IEnumerable<FakeObject> value) =>
            value.Select(MapperDate);


        protected override IPageConfig GetPageConfigFake() =>
            Commom.GetPageConfigFake();

        protected override FakeFilter GetFilterFake() =>
            Commom.GetFilterFake();

        protected override IEnumerable<FakeObject> GetListFake() =>
            Commom.GetListFake();

        protected override Expression<Func<FakeObject, bool>> GetFakeExpression() =>
            Commom.GetFakeExpression();
    }
}
