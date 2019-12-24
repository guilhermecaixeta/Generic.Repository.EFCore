using System;

namespace Generic.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class NoCacheableAttribute : Attribute
    {
    }
}