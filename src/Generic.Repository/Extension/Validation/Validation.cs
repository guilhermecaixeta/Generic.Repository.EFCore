using System;

namespace Generic.Repository.Extension.Validation
{
    public static class Validation
    {
        public static bool IsNull(this object value, string ClassName, string nameObject) => value.ValidateNullableOfObject() ?
        HandleNullError($"ClassName: {ClassName} {Environment.NewLine} Message: {nameObject} is null or empty.") : false;

        public static bool IsNull(this object value, string Message) => value.ValidateNullableOfObject() ? HandleNullError(Message) : false;

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

        public static bool IsString(this Type type, string className, string nameObject) => !isValidType(type, typeof(string)) ?
               HandleNotSupportedError($"ClassName: {className} {Environment.NewLine}Message: {nameObject} type is not string. This method only can be used by string type parameter.")
               : true;

        public static bool OperationIsValid(this bool isValid, string message) => !isValid ? throw new OperationCanceledException($"ERROR> {message}") : true;

        private static bool ValidateNullableOfObject(this object value) => isValidType(value.GetType(), typeof(string)) && string.IsNullOrEmpty(value.ToString()) || value == null;

        private static bool isValidType(Type typeObject, Type typeComparison) => typeObject == typeComparison;

        public static bool HandleNullError(string message) => throw new ArgumentNullException($"ERROR> {message}");

        public static bool HandleNotSupportedError(string message) => throw new NotSupportedException($"ERROR> {message}");
    }
}
