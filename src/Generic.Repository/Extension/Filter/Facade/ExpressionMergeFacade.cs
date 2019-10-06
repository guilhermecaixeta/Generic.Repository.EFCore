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
            var predicate = expressionB.CreateExpression<TValue>(parameter);

            if (expressionA.IsNull())
            {
                return predicate;
            }
            var expressionsJoined = JoinExpressions(expressionA, predicate, parameter, typeMerge);

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
            Expression<Func<TValue, bool>> predicateA,
            Expression<Func<TValue, bool>> predicateB,
            ParameterExpression parameter,
            LambdaMerge typeMerge) where TValue : class
        {
            switch (typeMerge)
            {
                case LambdaMerge.And:
                    var expression = AndAlso(predicateA, predicateB, parameter);
                    return expression.CreateExpression<TValue>(parameter);

                case LambdaMerge.Or:
                    expression = OrElse(predicateA, predicateB, parameter);
                    return expression.CreateExpression<TValue>(parameter);
            }
            return null;
        }

        private static InvocationExpression Invoke<TValue>(
            Expression<Func<TValue, bool>> predicate,
            ParameterExpression parameter) =>
                Expression.Invoke(predicate, parameter);
    }
}
