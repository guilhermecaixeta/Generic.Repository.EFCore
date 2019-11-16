namespace Generic.Repository.Validations.ThrowError
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Generic.Repository.Cache;
    using Generic.Repository.Exceptions;
    using Generic.Repository.Validations.Extension.Validation;

    /// <summary>Static class to check if is needed to trow a Exception.</summary>
    public static class ThrowErrorIf
    {
        private static ICacheRepository CacheRepository;

        internal static void InitializeCache(ICacheRepository cacheRepository)
        {
            if (cacheRepository.IsNull())
            {
                throw new CacheNotInitializedException(nameof(ThrowErrorIf));
            }

            CacheRepository = cacheRepository;
        }

        public static void IsLessThanZero(int value)
        {
            if (value < 0)
            {
                throw new LessThanZeroException(value.ToString());
            }
        }

        public static void IsLessThanOrEqualsZero(int value)
        {
            if (value < 0)
            {
                throw new LessThanOrEqualsZeroException(value.ToString());
            }
        }

        public async static Task IsFieldNotEquals(object value, object field, string nameField)
        {
            var result = await AreEquals(value, field, nameField);
            if (result.Item1)
            {
                throw new NotEqualsFieldException(result.Item2.ToString(), field.ToString());
            }
        }

        public static async Task IsFieldNotEquals<TException>(object value, object field, string nameField)
        where TException : Exception, new()
        {
            var result = await AreEquals(value, field, nameField);
            if (result.Item1)
            {
                throw new TException();
            }
        }

        /// <summary>Throws the error if string null or empty value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void IsEmptyOrNullString(string obj, string nameParameter, string nameMethod)
        {
            var result = !obj.IsStringNotNullOrEmpty();
            if (result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Determines whether [is empty or null string] [the specified object].</summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="TException"></exception>
        public static void IsEmptyOrNullString<TException>(string obj)
        where TException : Exception, new()
        {
            var result = !obj.IsStringNotNullOrEmpty();
            if (result)
            {
                throw new TException();
            }
        }

        /// <summary>Throws the error null value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void IsNullValue(object obj, string nameParameter, string nameMethod)
        {
            var result = obj.IsNull();
            if (result)
            {
                throw new ArgumentNullException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Determines whether [is null value] [the specified object].</summary>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="TException"></exception>
        public static void IsNullValue<TException>(object obj)
        where TException : Exception, new()
        {
            var result = obj.IsNull();
            if (result)
            {
                throw new TException();
            }
        }

        /// <summary>Throws the error null or empty list.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">MethodName &gt; {methodName}</exception>
        public static void IsNullOrEmptyList<T>(IEnumerable<T> obj, string nameParameter, string nameMethod)
        {
            var result = obj.HasAny();
            if (!result)
            {
                throw new ListNullOrEmptyException($"{nameParameter} MethodName > {nameMethod}");
            }
        }

        /// <summary>Determines whether [is null or empty list] [the specified object].</summary>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="TException"></exception>
        public static void IsNullOrEmptyList<TList, TException>(IEnumerable<TList> obj)
        where TException : Exception, new()
        {
            var result = obj.HasAny();
            if (!result)
            {
                throw new TException();
            }
        }

        /// <summary>Throws the error if the type is not allowed.</summary>
        /// <typeparam name="T">Type not allowed.</typeparam>
        /// <param name="obj">The object.</param>
        /// <exception cref="InvalidTypeException"></exception>
        public static void TypeIsNotAllowed<T>(object obj)
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
        public static void IsTypeNotEquals<T>(object obj)
        {
            var isTypeValid = obj.IsType<T>();
            if (!isTypeValid)
            {
                throw new InvalidTypeException(obj.GetType().Name);
            }
        }

        /// <summary>Ares the equals.</summary>
        /// <param name="field1">The field1.</param>
        /// <param name="field2">The field2.</param>
        /// <param name="nameField">The name field.</param>
        /// <returns></returns>
        private async static Task<(bool, object)> AreEquals(object field1, object field2, string nameField)
        {
            var funcGet = await CacheRepository.GetMethodGet(field1.GetType().Name, nameField);
            var fieldComparer = funcGet(field1);

            return (fieldComparer == field2, fieldComparer);
        }
    }
}
