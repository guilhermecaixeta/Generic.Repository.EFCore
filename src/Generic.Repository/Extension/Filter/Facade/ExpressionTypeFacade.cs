using Generic.Repository.Enums;
using Generic.Repository.ThrowError;
using System;
using System.Linq.Expressions;

namespace Generic.Repository.Extension.Filter.Facade
{
    internal class ExpressionTypeFacade
    {
        private readonly ThrowErrors ThrowError = new ThrowErrors();

        /// <summary>Determines whether this instance contains the object.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Type of argument is not valid to method. &gt; {nameof(Contains)}</exception>
        /// <exception cref="InvalidOperationException">Error to get method contains. &gt; {nameof(Contains)}</exception>
        public Expression Contains(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            ThrowError.ThrowErrorTypeIsNotEqual<string>(value);

            var method = typeof(string).
                GetMethod(LambdaMethod.Contains.ToString(), new[] { typeof(string) });

            var result = Expression.
                Call(memberExpression,
                    method ?? throw new InvalidOperationException($"Error to get method > {nameof(Contains)}"),
                    constant);
            return result;
        }

        /// <summary>Greaters the than.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Expression GreaterThan(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsNotString(value);
            var result = Expression.GreaterThan(memberExpression, constant);
            return result;
        }

        /// <summary>Lesses the than.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Expression LessThan(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsNotString(value);
            var result = Expression.LessThan(memberExpression, constant);
            return result;
        }

        /// <summary>Equals the specified constant.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Expression Equal(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsNotString(value);
            var result = Expression.Equal(memberExpression, constant);
            return result;
        }

        /// <summary>Greaters the than or equal.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns>Expression containing the predicate Greater than or equal.</returns>
        public Expression GreaterThanOrEqual(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsNotString(value);
            var result = Expression.GreaterThanOrEqual(memberExpression, constant);
            return result;
        }

        /// <summary>Lesses the than or equal.</summary>
        /// <param name="constant">The constant.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="value">The value.</param>
        /// <returns>Expression containing predicate less than or equal.</returns>
        public Expression LessThanOrEqual(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsNotString(value);
            var result = Expression.LessThanOrEqual(memberExpression, constant);
            return result;
        }

        /// <summary>Determines whether [is not string] [the specified object].</summary>
        /// <param name="obj">The object.</param>
        private void IsNotString(object obj)
        {
            ThrowError.ThrowErrorTypeIsNotAllowed<string>(obj);
        }

    }
}
