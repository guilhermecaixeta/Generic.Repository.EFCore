using Generic.Repository.Enums;
using System;

namespace Generic.Repository.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FilterAttribute : Attribute
    {
        public FilterAttribute()
        {
            MergeOption = LambdaMerge.And;
            MethodOption = LambdaMethod.Equals;
        }

        /// <summary>Gets or sets the merge option.</summary>
        /// <value>The merge option.</value>
        public LambdaMerge MergeOption { get; set; }

        /// <summary>Gets or sets the method option.</summary>
        /// <value>The method option.</value>
        public LambdaMethod MethodOption { get; set; }

        /// <summary>Gets or sets the name property.</summary>
        /// <value>The name property.</value>
        public string NameProperty { get; set; }
    }
}
