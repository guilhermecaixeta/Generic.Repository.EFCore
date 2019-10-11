using System;

namespace Generic.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PageAttribute : Attribute
    {
        public string NameProperty { get; set; }
    }
}
