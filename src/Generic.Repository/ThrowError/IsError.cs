using Generic.Repository.Exceptions;
using Generic.Repository.Extension.Validation;
using System;
using System.Collections.Generic;

namespace Generic.Repository.ThrowError
{
    internal class IsError
    {

        /// <summary>Throws the error if string null or empty value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public void IsThrowErrorEmptyOrNullString(string obj, string nameParameter, string nameMethod)
        {
            var result = !obj.IsStringNotNullOrEmpty();
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
        public void IsThrowErrorNullValue(object obj, string nameParameter, string nameMethod)
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
        public void IsThrowErrorNullOrEmptyList<T>(IEnumerable<T> obj, string nameParameter, string nameMethod)
        {
            var result = obj.HasAny();
            if (!result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Throws the error if the type is not allowed.</summary>
        /// <typeparam name="T">Type not allowed.</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="InvalidTypeException"></exception>
        public void ThrowErrorTypeIsNotAllowed<T>(object obj)
        {
            var isTypeValid = obj.IsType<T>();
            if (isTypeValid)
            {
                throw new InvalidTypeException(obj.GetType().Name);
            }
        }

        /// <summary>Throws the error type is not equal to T.</summary>
        /// <typeparam name="T">Type to compare</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="InvalidTypeException"></exception>
        public void IsThrowErrorTypeIsNotEqual<T>(object obj)
        {
            var isTypeValid = obj.IsType<T>();
            if (!isTypeValid)
            {
                throw new InvalidTypeException(obj.GetType().Name);
            }
        }
    }
}
