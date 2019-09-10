using System;
using System.Collections.Generic;
using System.Text;
using Generic.Repository.Attributes;
using Generic.Repository.Models.Filter;

namespace Generic.Repository.Test.Model.Filter
{
    public class FakeFilter : IFilter
    {
        [Lambda(MethodOption = Enums.LambdaMethod.Contains)]
        public string Value { get; set; }
    }
}
