using Generic.Repository.Attributes;
using Generic.Repository.Models.Filter;

namespace Generic.Repository.Test.Model.Filter
{
    public class FakeFilter : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Contains)]
        public string Value { get; set; }
    }

    public class FakeFilterEquals : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Equals)]
        public string Value { get; set; }
    }

    public class FakeFilterGreaterThan : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.GreaterThan)]
        public string Value { get; set; }
    }

    public class GreaterThanOrEqual : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.GreaterThanOrEqual)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThan : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.LessThan)]
        public string Value { get; set; }
    }

    public class FakeFilterLessThanOrEqual : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.LessThanOrEqual)]
        public string Value { get; set; }
    }
}
