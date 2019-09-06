using Generic.Repository.Enums;
using Generic.Repository.Extension.Validation;
using System;
using System.Linq.Expressions;

namespace Generic.Repository.Extension.Filter.Facade
{
    internal class ExpressionTypeFacade
    {
        private static void IsStringValidate(object value, string nameMethod)
        {
            var result = value.IsType<string>();
            if (result)
            {
                throw new ArgumentException($"Type of argument is not valid to method. > {nameMethod}");
            }
        }

        public Expression Contains(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            var isString = value.IsType<string>();
            if (!isString)
            {
                throw new ArgumentException($"Type of argument is not valid to method. > {nameof(Contains)}");
            }

            var method = typeof(string).
                GetMethod(LambdaMethod.Contains.ToString(), new[] { typeof(string) });

            var result = Expression.
                Call(memberExpression, method ?? throw new InvalidOperationException($"Error to get method contains. > {nameof(Contains)}"), constant);
            return result;
        }

        public Expression GreaterThan(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsStringValidate(value, nameof(GreaterThan));
            var result = Expression.GreaterThan(memberExpression, constant);
            return result;
        }

        public Expression LessThan(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsStringValidate(value, nameof(LessThan));
            var result = Expression.LessThan(memberExpression, constant);
            return result;
        }

        public Expression Equal(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsStringValidate(value, nameof(LessThan));
            var result = Expression.Equal(memberExpression, constant);
            return result;
        }

        public Expression GreaterThanOrEqual(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsStringValidate(value, nameof(LessThan));
            var result = Expression.GreaterThanOrEqual(memberExpression, constant);
            return result;
        }

        public Expression LessThanOrEqual(
            ConstantExpression constant,
            MemberExpression memberExpression,
            object value)
        {
            IsStringValidate(value, nameof(LessThan));
            var result = Expression.LessThanOrEqual(memberExpression, constant);
            return result;
        }
    }
}
