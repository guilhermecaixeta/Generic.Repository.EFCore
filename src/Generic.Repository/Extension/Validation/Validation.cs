using System;
using System.Collections.Generic;
using System.Linq;

namespace Generic.Repository.Extension.Validation
{
    public static class Validation
    {
        /// <summary>
        /// Validate if data is null.
        /// </summary>
        /// <param name="value">value to validate</param>
        /// <param name="className">name class to value</param>
        /// <param name="nameObject">name object of value</param>
        /// <returns></returns>
        public static bool IsNull(this object value, string className, string nameObject) => value.ValidateNullableOfObject() ?
        HandleNullError($"ClassName: {className} {Environment.NewLine} Message: {nameObject} is null or empty.") : false;

        /// <summary>
        /// Validate if data is null.
        /// </summary>
        /// <param name="value">value to valdate</param>
        /// <param name="Message">message to show</param>
        /// <returns></returns>
        public static bool IsNull(this object value, string Message) => value.ValidateNullableOfObject() ? HandleNullError(Message) : false;

        /// <summary>
        /// Validate if list is not null and has any element. 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="className"></param>
        /// <param name="nameObject"></param>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool HasAny<TValue>(this IEnumerable<TValue> list, string className, string nameObject) => !list.IsNull(className, nameObject) && !list.Any() ? true : throw new ArgumentException(MessageError($"ClassName: {ClassName} {Environment.NewLine}Message: {nameObject} don't has any element."));

        /// <summary>
        /// Validate if data type is not string
        /// </summary>
        /// <param name="type">Property to validate</param>
        /// <param name="className">Name of class will be used</param>
        /// <param name="nameObject">Name of object will be used</param>
        /// <param name="methodName">Name of method will be used</param>
        /// <returns>Return a bool</returns>
        public static bool IsNotString(this Type type, string className, string nameObject, string methodName) => isValidType(type, typeof(string)) ?
           HandleNotSupportedError($"ClassName: {className} {Environment.NewLine}Message: {nameObject} type is string. {methodName} method doesn't support this type.")
            : true;

        /// <summary>
        /// Verify if data is string.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="className"></param>
        /// <param name="nameObject"></param>
        /// <returns></returns>
        public static bool IsString(this Type type, string className, string nameObject) => !isValidType(type, typeof(string)) ?
               HandleNotSupportedError($"ClassName: {className} {Environment.NewLine}Message: {nameObject} type is not string. This method only can be used by string type parameter.")
               : true;

        /// <summary>
        /// Validate if operation is valid
        /// </summary>
        /// <param name="isValid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool OperationIsValid(this bool isValid, string message) => !isValid ? throw new OperationCanceledException(MessageError(message)) : true;

        /// <summary>
        /// Handle argument null exception to user.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HandleNullError(string message) => throw new ArgumentNullException(MessageError(message));

        /// <summary>
        /// Handle not supported exception to user.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool HandleNotSupportedError(string message) => throw new NotSupportedException(MessageError(message));

        private static bool ValidateNullableOfObject(this object value) => isValidType(value.GetType(), typeof(string)) && string.IsNullOrEmpty(value.ToString()) || value == null;

        private static bool isValidType(Type typeObject, Type typeComparison) => typeObject == typeComparison;

        private static string MessageError(string message) => $"ERROR> {message}";
    }
}
