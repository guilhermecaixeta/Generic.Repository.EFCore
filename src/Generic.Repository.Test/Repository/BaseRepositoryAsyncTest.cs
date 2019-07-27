namespace Generic.Repository.Test.Repository
{
    using Generic.Repository.Attributes;
    using Generic.Repository.Models.Filter;
    using Generic.Repository.Models.Page.PageConfig;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class SimpleObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SimpleObjectFilter : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Contains)]
        public string Name { get; set; }
    }

    [TestFixture]
    public class BaseRepositoryAsyncTest : BaseRepositoryAsyncQueryTest<SimpleObject, SimpleObjectFilter>
    {
        public BaseRepositoryAsyncTest()
        {
            count = 10;
            count2 = 5;
            count3 = 1;
        }

        protected override SimpleObject CreateTValue() => new SimpleObject { Name = "Test_1" };

        protected override SimpleObject UpdateTValue(SimpleObject value)
        {
            value.Name = "Test_2";
            return value;
        }

        protected override Expression<Func<SimpleObject, bool>> ExpressionGeneric(SimpleObject value)
            => GetExpression(x => x.Id == value.Id);

        internal override IPageConfig GetPageConfig()
        => new PageConfig{ order = "Name", page = 0, size = 5, sort = "ASC" };

        internal override SimpleObjectFilter GetFilter()
        => new SimpleObjectFilter { Name = "aaa" };

        internal override Expression<Func<SimpleObject, bool>> ExpressionGeneric()
        => GetExpression(x => "aaa".Contains(x.Name));

        private Expression<Func<SimpleObject, bool>> GetExpression(
            Expression<Func<SimpleObject, bool>> expression) => expression;

        protected override IEnumerable<SimpleObject> GetListSimpleObject() =>
            new List<SimpleObject>{
                new SimpleObject{Name = "mmm"},
                new SimpleObject{Name = "ddd"},
                new SimpleObject{Name = "eee"},
                new SimpleObject{Name = "yyy"},
                new SimpleObject{Name = "fff"},
                new SimpleObject{Name = "kkk"},
                new SimpleObject{Name = "hhh"},
                new SimpleObject{Name = "qqq"},
                new SimpleObject{Name = "aaa"},
                new SimpleObject{Name = "ppp"},
            };

        internal override void SaveList(IEnumerable<SimpleObject> list)
        {
            _repository.CreateAsync(list).Wait();
        }

    }
}
