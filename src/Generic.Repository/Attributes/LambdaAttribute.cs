using System;
using Generic.Repository.Enums;

namespace Generic.Repository.Attributes
{
    public class LambdaAttribute : Attribute
    {
        public LambdaAttribute()
        {
            MergeOption = LambdaMerge.And;
            MethodOption = LambdaMethod.Equals;
        }
        /// <summary>
        /// Type of merge of lambda, examples: And/Or
        /// </summary>
        /// <value></value>
        public LambdaMerge MergeOption { get; set; }
        /// <summary>
        /// Type of method which will been used to auto generate lambda from filter.
        /// </summary>
        /// <value></value>
        public LambdaMethod MethodOption { get; set; }
        /// <summary>
        /// Name of property of entity
        /// </summary>
        /// <value></value>
        public string NameProperty { get; set; }
    }
}
