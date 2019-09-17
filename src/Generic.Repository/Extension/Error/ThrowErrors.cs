using Generic.Repository.Extension.Validation;
using System;
using System.Collections.Generic;

namespace Generic.Repository.Extension.Error
{
    internal static class ThrowErrors
    {

        /// <summary>Throws the error if string null or empty value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void ThrowErrorEmptyOrNullString(this string obj, string nameParameter, string nameMethod)
        {
            var result = !obj.IsStringNonNullOrEmpty();
            if (result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Throws the error null value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void ThrowErrorNullValue(this object obj, string nameParameter, string nameMethod)
        {
            var result = obj.IsNull();
            if (result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Throws the error null or empty list.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">MethodName &gt; {methodName}</exception>
        public static void ThrowErrorNullOrEmptyList<T>(this IEnumerable<T> obj, string nameParameter, string nameMethod)
        {
            var result = obj.HasAny();
            if (!result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }
    }
}
