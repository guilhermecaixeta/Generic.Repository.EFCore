namespace Generic.Repository.Validations.Extension.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Validation
    {
        /// <summary>Determines whether this instance is null.</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is null; otherwise, <c>false</c>.</returns>
        public static bool IsNull(this object obj) =>
            obj is null;

        /// <summary>Determines whether [is string not null or empty].</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if [is string not null or empty] [the specified object]; otherwise, <c>false</c>.</returns>
        public static bool IsStringNotNullOrEmpty(this object obj) =>
            obj.IsType<string>() && !string.IsNullOrEmpty((string)obj);

        /// <summary>Determines whether [is not equal date time maximum minimum value].</summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if [is not equal date time maximum minimum value] [the specified object]; otherwise, <c>false</c>.</returns>
        public static bool IsNotEqualDateTimeMaxMinValue(this object obj) =>
            obj.IsType<DateTime>() && ((DateTime)obj).Date > DateTime.MinValue && ((DateTime)obj).Date < DateTime.MaxValue;

        /// <summary>Determines whether this instance is type.</summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is type; otherwise, <c>false</c>.</returns>
        public static bool IsType<T>(this object obj) =>
            obj is T;

        /// <summary>Determines whether this instance has any.</summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns>
        ///   <c>true</c> if the specified object has any; otherwise, <c>false</c>.</returns>
        public static bool HasAny<T>(this IEnumerable<T> obj) =>
            !obj.IsNull() && obj.Any();
    }
}
