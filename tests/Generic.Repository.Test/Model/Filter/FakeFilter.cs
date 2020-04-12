using Generic.Repository.Attributes;
using Generic.Repository.Enums;
using Generic.Repository.Models.Filter;

namespace Generic.RepositoryTest.Unit.Model.Filter
{
    public class FakeFilter : IFilter
    {
        public FakeFilterEquals Nested { get; set; }

        [NoCacheable]
        public string Unkown { get; set; }

        [Filter(MethodOption = LambdaMethod.Contains)]
        public string Value { get; set; }
    }

    public class FakeFilterEquals : IFilter
    {
        [Filter(MethodOption = LambdaMethod.Equals)]
        public string Value { get; set; }
    }

    public class FakeFilterGreaterThan : IFilter
    {
        [Filter(MethodOption = LambdaMethod.GreaterThan)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThan : IFilter
    {
        [Filter(MethodOption = LambdaMethod.LessThan)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThanOrEqual : IFilter
    {
        [Filter(MethodOption = LambdaMethod.LessThanOrEqual)]
        public string Value { get; set; }
    }

    public class GreaterThanOrEqual : IFilter
    {
        [Filter(MethodOption = LambdaMethod.GreaterThanOrEqual)]
        public string Value { get; set; }
    }
}