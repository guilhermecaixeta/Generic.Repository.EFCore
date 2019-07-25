using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Generic.Repository.Cache;
using Generic.Repository.Enums;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;

namespace Generic.Repository.Extension.Filter
{
    /// <summary>
    /// Extension Filter to generate lambda.
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// Generate lambda method
        /// </summary>
        /// <param name="filter">Object filter</param>
        /// <typeparam name="TValue">Type Entity</typeparam>
        /// <typeparam name="F">Type Filter</typeparam>
        /// <returns>Predicate generated</returns>
        public static Expression<Func<TValue, bool>> GenerateLambda<TValue, TFilter>(
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
            SaveOnCacheIfNonExists<TFilter>(
                true,
                true,
                false,
                false);
            cacheRepository.
            GetDictionaryMethodGet(typeNameTFilter).
            ToList().
            ForEach(propertyTFilter =>
            {
                Expression lambda = null;
                var namePropertyOnE = string.Empty;
                var namePropertyOnTFilter = propertyTFilter.Key;
                var propertyValueTFilter = propertyTFilter.Value(filter);

                if (
                    propertyValueTFilter != null &&
                    (!propertyValueTFilter.ToString().Equals("0") ||
                    (propertyValueTFilter.GetType() == typeof(DateTime) &&
                    ((DateTime)propertyValueTFilter != DateTime.MinValue ||
                    (DateTime)propertyValueTFilter != DateTime.MaxValue))))
                {
                    var customAttributes = cacheRepository.GetDictionaryAttribute(typeNameTFilter);
                    if (customAttributes.TryGetValue(namePropertyOnTFilter, out Dictionary<string, CustomAttributeTypedArgument> attributes))
                    {
                        if (attributes.TryGetValue("EntityPropertyName", out CustomAttributeTypedArgument attribute))
                        {
                            namePropertyOnE = attribute.Value.ToString();
                        }
                        if (attributes.TryGetValue("MethodOption", out attribute))
                        {
                            var property = cacheRepository.GetProperty(typeNameTValue, namePropertyOnE ?? propertyTFilter.Key.ToString());
                            methodOption = (LambdaMethod)attribute.Value;
                            lambda = methodOption.SetExpressionType(param, property, propertyValueTFilter);
                        }
                        if (!lambda.IsNull(nameof(GenerateLambda), nameof(lambda)))
                        {
                            predicate = predicate == null ?
                                lambda.MergeExpressions<TValue>(param) :
                                predicate.MergeExpressions(mergeOption, param, lambda.MergeExpressions<TValue>(param));

                            if (attributes.TryGetValue("MergeOption", out attribute))
                            {
                                mergeOption = (LambdaMerge)attribute.Value;
                            }
                            else
                            {
                                mergeOption = LambdaMerge.And;
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
        private static Expression SetExpressionType(this LambdaMethod type, ParameterExpression parameter, PropertyInfo prop, object value)
        {
            Expression lambda = null;
            switch (type)
            {
                case LambdaMethod.Contains:
                    if (prop.PropertyType.IsString(nameof(SetExpressionType), prop.Name))
                    {
                        MethodInfo method = typeof(string).GetMethod(LambdaMethod.Contains.ToString(), new[] { typeof(string) });
                        lambda = Expression.Call(Expression.Property(parameter, prop), method, Expression.Constant(value));
                    }
                    break;
                case LambdaMethod.GreaterThan:
                    if (prop.GetType().IsNotString(nameof(SetExpressionType), prop.Name, LambdaMethod.GreaterThan.ToString()))
                    {
                        lambda = Expression.GreaterThan(Expression.Property(parameter, prop), Expression.Constant(value));
                    }
                    break;
                case LambdaMethod.LessThan:
                    if (prop.GetType().IsNotString(nameof(SetExpressionType), prop.Name, LambdaMethod.LessThan.ToString()))
                    {
                        lambda = Expression.LessThan(Expression.Property(parameter, prop), Expression.Constant(value));
                    }
                    break;
                case LambdaMethod.GreaterThanOrEqual:
                    if (prop.GetType().IsNotString(nameof(SetExpressionType), prop.Name, LambdaMethod.GreaterThanOrEqual.ToString()))
                    {
                        lambda = Expression.GreaterThanOrEqual(Expression.Property(parameter, prop), Expression.Constant(value));
                    }
                    break;
                case LambdaMethod.LessThanOrEqual:
                    if (prop.GetType().IsNotString(nameof(SetExpressionType), prop.Name, LambdaMethod.GreaterThanOrEqual.ToString()))
                    {
                        lambda = Expression.LessThanOrEqual(Expression.Property(parameter, prop), Expression.Constant(value));
                    }
                    break;
                default: return Expression.Equal(Expression.Property(parameter, prop), Expression.Constant(value));
            }
            return lambda;
        }

        private static Expression<Func<TValue, bool>> MergeExpressions<TValue>(this Expression lambda, ParameterExpression parameter)
         where TValue : class => Expression.Lambda<Func<TValue, bool>>(lambda, parameter);

        private static Expression<Func<TValue, bool>> MergeExpressions<TValue>(this Expression<Func<TValue, bool>> predicate, LambdaMerge typeMerge, ParameterExpression parameter, Expression<Func<TValue, bool>> predicateMerge)
        where TValue : class
        {
            Expression lambda = null;
            if (typeMerge == LambdaMerge.And)
            {
                lambda = Expression.AndAlso(
                    Expression.Invoke(predicate, parameter),
                    Expression.Invoke(predicateMerge, parameter));
            }
            else
            {
                lambda = Expression.OrElse(
                Expression.Invoke(predicate, parameter),
                Expression.Invoke(predicateMerge, parameter));
            }
            return Expression.Lambda<Func<TValue, bool>>(lambda, parameter);
        }
    }
}
