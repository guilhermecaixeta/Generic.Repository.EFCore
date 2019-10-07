using System;
using System.Collections;
using Generic.Repository.Exceptions;
using Microsoft.EntityFrameworkCore.Internal;

namespace Generic.Repository.Extension.Validation
{
    internal static class Validation
    {
        internal static bool IsNull(this object obj) => 
            obj == null;

        internal static bool IsStringNotNullOrEmpty(this object obj) =>
            obj.IsType<string>() && !string.IsNullOrEmpty((string)obj);

        internal static bool IsNotEqualDateTimeMaxMinValue(this object obj) =>
            obj.IsType<DateTime>() && ((DateTime)obj).Date > DateTime.MinValue && ((DateTime)obj).Date < DateTime.MaxValue;

        internal static bool IsType<T>(this object obj) => 
            obj is T;

        internal static bool HasAny(this object obj) => 
            !obj.IsNull() && (obj.IsType<IEnumerable>() ? 
                ((IEnumerable)obj).Any() : throw new InvalidTypeException(obj.GetType().Name));
    }
}
