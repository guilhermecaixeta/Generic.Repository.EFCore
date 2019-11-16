using Generic.Repository.Attributes;
using Generic.Repository.Models.Filter;

namespace Generic.Repository.Test.Model.Filter
{
    public class FakeFilter : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.Contains)]
        public string Value { get; set; }
    }

    public class FakeFilterEquals : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.Equals)]
        public string Value { get; set; }
    }

    public class FakeFilterGreaterThan : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.GreaterThan)]
        public string Value { get; set; }
    }

    public class GreaterThanOrEqual : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.GreaterThanOrEqual)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThan : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.LessThan)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThanOrEqual : IFilter
    {
        [Filter(MethodOption = Enums.LambdaMethod.LessThanOrEqual)]
        public string Value { get; set; }
    }
}
