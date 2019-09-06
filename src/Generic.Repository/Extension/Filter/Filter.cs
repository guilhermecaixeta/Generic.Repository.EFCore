using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Generic.Repository.Cache;
using Generic.Repository.Enums;
using Generic.Repository.Extension.Filter.Facade;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;

namespace Generic.Repository.Extension.Filter
{
    /// <summary>
    /// Extension Filter to generate lambda.
    /// </summary>
    internal static class Filter
    {
        private static readonly ExpressionTypeFacade FilterFacade = new ExpressionTypeFacade();

        /// <summary>Generates the predicate.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <returns></returns>
        public static Expression<Func<TValue, bool>> GeneratePredicate<TValue, TFilter>(
        this TFilter filter,
        ICacheRepository cacheRepository)
        where TValue : class
        where TFilter : class, IFilter
        {
            var param = Expression.Parameter(typeof(TValue));
            var typeNameTFilter = typeof(TFilter).Name;
            var typeNameTValue = typeof(TValue).Name;
            var mergeOption = LambdaMerge.And;

            Expression<Func<TValue, bool>> predicate = null;
            LambdaMethod methodOption;

            cacheRepository.
            GetDictionaryMethodGet(typeNameTFilter).
            ToList().
            ForEach(propertyTFilter =>
            {
                Expression expression = null;
                string namePropertyOnE = null;
                var namePropertyOnTFilter = propertyTFilter.Key;
                var propertyValueTFilter = propertyTFilter.Value(filter);

                if (propertyValueTFilter.IsDateTimeDiffMaxMinValue() || propertyValueTFilter.IsStringNonNullOrEmpty())
                {
                    var customAttributes = cacheRepository.GetDictionaryAttribute(typeNameTFilter);
                    if (customAttributes.TryGetValue(namePropertyOnTFilter, out Dictionary<string, CustomAttributeTypedArgument> attributes))
                    {
                        if (attributes.TryGetValue("NameProperty", out CustomAttributeTypedArgument attribute))
                        {
                            namePropertyOnE = attribute.Value.ToString();
                        }
                        if (attributes.TryGetValue("MethodOption", out attribute))
                        {
                            var property = cacheRepository.GetProperty(typeNameTValue, namePropertyOnE ?? propertyTFilter.Key.ToString());
                            methodOption = (LambdaMethod)attribute.Value;
                            expression = methodOption.SetExpressionType(param, property, propertyValueTFilter);
                        }
                        if (!expression.IsNull(nameof(GeneratePredicate), nameof(expression)))
                        {
                            predicate = predicate == null ?
                                expression.MergeExpressions<TValue>(param) :
                                predicate.MergeExpressions(mergeOption, param, expression.MergeExpressions<TValue>(param));

                            mergeOption = LambdaMerge.And;

                            if (attributes.TryGetValue("MergeOption", out attribute))
                            {
                                mergeOption = (LambdaMerge)attribute.Value;
                            }
                        }
                    }
                }
            });
            return predicate;
        }

        /// <summary>
        /// Create an expression
        /// </summary>
        /// <param name="type">Type expression to create</param>
        /// <param name="parameter">Parameter to will be used to make an expression</param>
        /// <param name="prop">Property to will be used to make an expression</param>
        /// <param name="value">Value to will be used to make an expression</param>
        /// <returns></returns>
        private static Expression SetExpressionType(
            this LambdaMethod type,
            ParameterExpression parameter,
            PropertyInfo prop,
            object value)
        {
            var memberExpression = Expression.Property(parameter, prop);
            var constant = Expression.Constant(value);

            switch (type)
            {
                case LambdaMethod.Contains:
                    var result = FilterFacade.Contains(constant, memberExpression, value);
                    return result;
                case LambdaMethod.GreaterThan:
                    result = FilterFacade.GreaterThan(constant, memberExpression, value);
                    return result;
                case LambdaMethod.LessThan:
                    result = FilterFacade.LessThan(constant, memberExpression, value);
                    return result;
                case LambdaMethod.GreaterThanOrEqual:
                    result = FilterFacade.GreaterThanOrEqual(constant, memberExpression, value);
                    return result;
                case LambdaMethod.LessThanOrEqual:
                    result = FilterFacade.LessThanOrEqual(constant, memberExpression, value);
                    return result;
                default:
                    result = FilterFacade.Equal(constant, memberExpression, value);
                    return result;
            }
        }

        private static Expression<Func<TValue, bool>> MergeExpressions<TValue>(
            this Expression lambda,
            ParameterExpression parameter)
            where TValue : class =>
                Expression.Lambda<Func<TValue, bool>>(lambda, parameter);

        private static Expression<Func<TValue, bool>> MergeExpressions<TValue>(
            this Expression<Func<TValue, bool>> predicate,
            LambdaMerge typeMerge,
            ParameterExpression parameter,
            Expression<Func<TValue, bool>> predicateMerge)
        where TValue : class
        {
            var lambda = Expression.OrElse(
                Expression.Invoke(predicate, parameter),
                Expression.Invoke(predicateMerge, parameter));

            if (typeMerge == LambdaMerge.And)
            {
                lambda = Expression.AndAlso(
                    Expression.Invoke(predicate, parameter),
                    Expression.Invoke(predicateMerge, parameter));
            }

            return Expression.Lambda<Func<TValue, bool>>(lambda, parameter);
        }

    }
}
