using System;
using System.Collections.Generic;
using System.Text;
using Generic.Repository.Extension.Validation;

namespace Generic.Repository.Repository
{
    internal static class BaseRepositoryValidator
    {
        public static void IsThrowNullError(this object obj, string nameMethod)
        {
            var result = obj.IsNull();
            if (result)
            {
                throw new ArgumentNullException(nameMethod);
            }
        }

        public static void IsNullOrEmptyListThrowError(object obj, string nameMethod)
        {
            var result = obj.HasAny();
            if (result)
            {
                throw new ArgumentNullException(nameMethod);
            }
        }
    }
}
