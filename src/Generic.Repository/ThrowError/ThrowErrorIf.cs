using Generic.Repository.Cache;
using Generic.Repository.Exceptions;
using Generic.Repository.Extension.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.ThrowError
{
    /// <summary>Throw an error if the condition is attempt.</summary>
    public static class ThrowErrorIf
    {
        private static ICacheRepository CacheRepository;

        /// <summary>Determines whether [is field not equals] [the specified value].</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="field">The field.</param>
        /// <param name="nameField">The name field.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="NotEqualsFieldException"></exception>
        public static async Task FieldIsNotEquals<TEntity>(
            TEntity value,
            object field,
            string nameField,
            CancellationToken token)
            where TEntity : class
        {
            var result = await AreEquals(value, field, nameField, token).
                        ConfigureAwait(false);

            if (result.Item1)
            {
                throw new NotEqualsFieldException(result.Item2.ToString(), field.ToString());
            }
        }

        /// <summary>Determines whether [is field not equals] [the specified value].</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TException">The type of the exception.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="field">The field.</param>
        /// <param name="nameField">The name field.</param>
        /// <param name="token">The token.</param>
        /// <exception cref="TException"></exception>
        public static async Task FieldIsNotEquals<TEntity, TException>(
            TEntity value,
            object field,
            string nameField,
            CancellationToken token)
        where TException : Exception, new()
        where TEntity : class
        {
            var result = await AreEquals(value, field, nameField, token).
                        ConfigureAwait(false);

            if (result.Item1)
            {
                throw new TException();
            }
        }

        /// <summary>Initializes the cache.</summary>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <exception cref="CacheNotInitializedException">ThrowErrorIf</exception>
        public static void InitializeCache(ICacheRepository cacheRepository)
        {
            HasCache(cacheRepository);

            CacheRepository = cacheRepository;
        }

        /// <summary>Throws the error if string null or empty value.</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">Name of the Parameter.</param>
        /// <param name="nameMethod">Name of the method.</param>
        /// <exception cref="ArgumentNullException">Attribute&gt; {attributeName} MethodName&gt; {methodName}</exception>
        public static void IsEmptyOrNullString(
            string obj,
            string nameParameter,
            string nameMethod)
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

        /// <summary>Determines whether [is less than or equals zero] [the specified value].</summary>
        /// <param name="value">The value.</param>
        /// <exception cref="LessThanOrEqualsZeroException">val &lt;= 0</exception>
        public static void IsLessThanOrEqualsZero(int value)
        {
            if (value < 0)
            {
                throw new LessThanOrEqualsZeroException(value.ToString());
            }
        }

        /// <summary>Determines whether [is less than zero] [the specified value].</summary>
        /// <param name="value">The value.</param>
        /// <exception cref="LessThanZeroException"></exception>
        public static void IsLessThanZero(int value)
        {
            if (value < 0)
            {
                throw new LessThanZeroException(value.ToString());
            }
        }

        /// <summary>Determines whether [is null or empty list] [the specified object].</summary>
        /// <typeparam name="TList">The type of the list.</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">The name parameter.</param>
        /// <param name="nameMethod">The name method.</param>
        /// <exception cref="ListNullOrEmptyException"></exception>
        public static void IsNullOrEmptyList<TList>(
            IEnumerable<TList> obj,
            string nameParameter,
            string nameMethod)
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

        /// <summary>Determines whether [is null value] [the specified object].</summary>
        /// <param name="obj">The object.</param>
        /// <param name="nameParameter">The name parameter.</param>
        /// <param name="nameMethod">The name method.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void IsNullValue(
            object obj,
            string nameParameter,
            string nameMethod)
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

        /// <summary>Ares the equals.</summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="param">The parameter.</param>
        /// <param name="nameFieldObject">The name field.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        private static async Task<(bool, object)> AreEquals<TEntity>(
            TEntity @object,
            object @param,
            string nameFieldObject,
            CancellationToken token)
            where TEntity : class
        {
            HasCache(CacheRepository);

            var funcGet = await CacheRepository.
                GetMethodGet(@object.GetType().Name, nameFieldObject, token).
                ConfigureAwait(false);

            var value = funcGet(@param);

            var isEquals = value == @param;

            return (isEquals, value);
        }

        /// <summary>Check if cache was initialized.</summary>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <exception cref="CacheNotInitializedException">ThrowErrorIf</exception>
        private static void HasCache(ICacheRepository cacheRepository)
        {
            if (cacheRepository.IsNull())
            {
                throw new CacheNotInitializedException(nameof(ThrowErrorIf));
            }
        }
    }
}