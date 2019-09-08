using Generic.Repository.Enums;
using Generic.Repository.Extension.Validation;
using System;
using System.Linq.Expressions;

namespace Generic.Repository.Extension.Filter.Facade
{
    internal static class ExpressionMergeFacade
    {
        public static Expression<Func<TValue, bool>> CreateExpression<TValue>(
            Expression<Func<TValue, bool>> expressionA,
            Expression expressionB,
            ParameterExpression parameter,
            LambdaMerge typeMerge) where TValue : class
        {
            var isNull = expressionA.IsNull();
            var predicate = expressionB.CreateExpression<TValue>(parameter);

            if (isNull)
            {
                return predicate;
            }
            var expressionsJoined = expressionA.JoinExpressions(predicate, parameter, typeMerge);

            return expressionsJoined;
        }

        private static BinaryExpression AndAlso<TValue>(
            this Expression<Func<TValue, bool>> predicateA,
            Expression<Func<TValue, bool>> predicateB,
            ParameterExpression parameter) where TValue : class =>
                Expression.OrElse(
                    Invoke(predicateA, parameter),
                    Invoke(predicateB, parameter));

        private static BinaryExpression OrElse<TValue>(
            this Expression<Func<TValue, bool>> predicateA,
            Expression<Func<TValue, bool>> predicateB,
            ParameterExpression parameter) where TValue : class =>
                    Expression.AndAlso(
                        Invoke(predicateA, parameter),
                        Invoke(predicateB, parameter));

        private static Expression<Func<TValue, bool>> CreateExpression<TValue>(
            this Expression expression,
            ParameterExpression parameter) where TValue : class =>
                Expression.Lambda<Func<TValue, bool>>(expression, parameter);

        private static Expression<Func<TValue, bool>> JoinExpressions<TValue>(
            this Expression<Func<TValue, bool>> predicateA,
            Expression<Func<TValue, bool>> predicateB,
            ParameterExpression parameter,
            LambdaMerge typeMerge) where TValue : class
        {
            var lambda = AndAlso(predicateA, predicateB, parameter);

            if (typeMerge == LambdaMerge.Or)
            {
                lambda = OrElse(predicateA, predicateB, parameter);
            }

            var result = lambda.CreateExpression<TValue>(parameter);

            return result;
        }

        private static InvocationExpression Invoke<TValue>(
            Expression<Func<TValue, bool>> predicate,
            ParameterExpression parameter) =>
                Expression.Invoke(predicate, parameter);
    }
}
