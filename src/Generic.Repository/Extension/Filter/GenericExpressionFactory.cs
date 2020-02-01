using Generic.Repository.Cache;
using Generic.Repository.Enums;
using Generic.Repository.Extension.Filter.Facade;
using Generic.Repository.Extension.Validation;
using Generic.Repository.Models.Filter;
using Generic.Repository.Models.PageAggregation.PageConfig;
using Generic.Repository.ThrowError;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Extension.Filter
{
    /// <summary>
    /// Extension Filter to generate lambda.
    /// </summary>
    internal static class GenericExpressionFactory
    {
        private const string MergeOption = nameof(MergeOption);
        private const string MethodOption = nameof(MethodOption);
        private const string NameProperty = nameof(NameProperty);
        private static readonly ExpressionTypeFacade FilterFacade = new ExpressionTypeFacade();

        /// <summary>Generates the predicate.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TFilter">The type of the filter.</typeparam>
        /// <param name="filter">The filter.</param>
        /// <param name="cacheRepository">The cache repository.</param>
        /// <returns></returns>
        public static async Task<Expression<Func<TValue, bool>>> CreateGenericFilter<TValue, TFilter>(
        this TFilter filter,
        ICacheRepository cacheRepository,
        CancellationToken token)
        where TValue : class
        where TFilter : class, IFilter
        {
            var predicate = (Expression<Func<TValue, bool>>)null;
            var parameter = Expression.Parameter(typeof(TValue));
            var filterName = typeof(TFilter).Name;
            var mergeOption = LambdaMerge.And;

            var dictionaryMethodGet = await cacheRepository.GetDictionaryMethodGet(filterName, token);

            foreach (var getFilter in dictionaryMethodGet)
            {
                var key = getFilter.Key;
                var value = getFilter.Value(filter);

                if (!IsValidValue(value))
                {
                    continue;
                }

                var attributeCached = await cacheRepository.GetDictionaryAttribute(filterName, token);

                if (!attributeCached.TryGetValue(key, out var attributes))
                {
                    continue;
                }

                attributes.TryGetValue(MethodOption, out var attributeMethod);

                ThrowErrorIf.IsNullValue(attributeMethod, nameof(attributeMethod), nameof(CreateGenericFilter));

                var methodOption = (LambdaMethod)attributeMethod.Value;

                attributes.TryGetValue(NameProperty, out var attributeName);

                var property = await cacheRepository.GetProperty(typeof(TValue).Name, (string)attributeName.Value ?? key, token);

                var expression = methodOption.CreateExpressionPerType(parameter, property, value);

                ThrowErrorIf.IsNullValue(expression, nameof(expression), nameof(CreateGenericFilter));

                predicate = ExpressionMergeFacade.CreateExpression(predicate, expression, parameter, mergeOption);

                attributes.TryGetValue(MergeOption, out var attributeMerge);

                mergeOption = !attributeMerge.Value.IsNull() ? (LambdaMerge)attributeMerge.Value : LambdaMerge.And;
            }
            return predicate;
        }

        public static async Task<Expression<Func<TValue, object>>> CreateGenericOrderBy<TValue, TFilter>(
            this IPageConfig pageConfig,
            ICacheRepository cacheRepository,
            CancellationToken token)
        {
            var key = typeof(TFilter).Name;

            var dictionary = await cacheRepository.GetDictionaryAttribute(key, pageConfig.Order, token);

            dictionary.TryGetValue(NameProperty, out var nameField);

            var order = (string)nameField.Value ?? pageConfig.Order;

            return await GetGenericExpression<TValue>(order, cacheRepository, token);
        }

        public static async Task<Expression<Func<TValue, object>>> CreateGenericOrderBy<TValue>(
        this IPageConfig pageConfig,
        ICacheRepository cacheRepository,
        CancellationToken token)
        {
            var order = pageConfig.Order;

            return await GetGenericExpression<TValue>(order, cacheRepository, token);
        }

        private static Expression<Func<TValue, object>> CreateExpression<TValue>(
            ParameterExpression parameter,
            PropertyInfo propertyInfo)
        {
            ThrowErrorIf.IsNullValue(parameter, nameof(parameter), nameof(CreateExpression));
            ThrowErrorIf.IsNullValue(propertyInfo, nameof(propertyInfo), nameof(CreateExpression));

            var memberExpression = Expression.PropertyOrField(parameter, propertyInfo.Name);

            var conversion = Expression.Convert(memberExpression, typeof(object));

            return Expression.Lambda<Func<TValue, object>>(conversion, parameter);
        }

        /// <summary>Creates the type of the expression per.</summary>
        /// <param name="type">The type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static Expression CreateExpressionPerType(
            this LambdaMethod type,
            ParameterExpression parameter,
            PropertyInfo propertyInfo,
            object value)
        {
            ThrowErrorIf.IsNullValue(parameter, nameof(parameter), nameof(CreateExpressionPerType));
            ThrowErrorIf.IsNullValue(propertyInfo, nameof(propertyInfo), nameof(CreateExpressionPerType));

            var memberExpression = Expression.Property(parameter, propertyInfo);
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

        private static async Task<Expression<Func<TValue, object>>> GetGenericExpression<TValue>(
            string order,
            ICacheRepository cacheRepository,
            CancellationToken token)
        {
            var key = typeof(TValue).Name;

            var parameter = Expression.Parameter(typeof(TValue));

            var propertyInfo = await cacheRepository.GetProperty(key, order, token);

            return CreateExpression<TValue>(parameter, propertyInfo);
        }

        /// <summary>Determines whether [is valid value] [the specified value].</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is valid value] [the specified value]; otherwise, <c>false</c>.</returns>
        private static bool IsValidValue(object value) =>
            value.IsNotEqualDateTimeMaxMinValue() || value.IsStringNotNullOrEmpty();
    }
}