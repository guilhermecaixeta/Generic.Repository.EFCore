using System;
using System.Collections;
using Microsoft.EntityFrameworkCore.Internal;

namespace Generic.Repository.Extension.Validation
{
    internal static class Validation
    {
        internal static bool IsNull(this object obj) => obj == null;

        internal static bool IsStringNonNullOrEmpty(this object obj) =>
            obj.IsType<string>() && !string.IsNullOrEmpty((string)obj);

        internal static bool IsDateTimeDiffMaxMinValue(this object obj) =>
            obj.IsType<DateTime>() && ((DateTime)obj).Date > DateTime.MinValue && ((DateTime)obj).Date < DateTime.MaxValue;

        internal static bool IsType<T>(this object obj) => obj is T;

        internal static bool HasAny(this object obj) => obj is IEnumerable ? ((IEnumerable)obj).Any() : false;
    }
}
