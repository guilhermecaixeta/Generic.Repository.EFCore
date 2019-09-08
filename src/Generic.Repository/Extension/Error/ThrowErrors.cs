using Generic.Repository.Extension.Validation;
using System;
using System.Collections.Generic;

namespace Generic.Repository.Extension.Error
{
    internal static class ThrowErrors
    {
        /// <summary>Throws the error null value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void ThrowErrorNullValue(this object obj, string attributeName, string methodName)
        {
            var result = obj.IsNull();
            if (result)
            {
                throw new ArgumentNullException($"Attribute> {attributeName} MethodName> {methodName}");
            }
        }

        /// <summary>Throws the error null or empty list.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <exception cref="ArgumentNullException">MethodName &gt; {methodName}</exception>
        public static void ThrowErrorNullOrEmptyList<T>(this IEnumerable<T> obj, string methodName)
        {
            var result = obj.HasAny();
            if (result)
            {
                throw new ArgumentNullException($"MethodName > {methodName}");
            }
        }
    }
}
