using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Generic.Repository.Entity.IFilter;

namespace Generic.Repository.Extension.Repository
{
    public static class Repository
    {
        public static Expression<Func<E, bool>> GenerateLambda<E, F>(this F filter)
        where E : class
        where F : IBaseFilter
        {

            ParameterExpression param = Expression.Parameter(typeof(E));
            Expression<Func<E, bool>> predicate = null;
            string mergeExpressionType = "";
            string typeExpression = "";
            string nameProp = "";
            filter.GetType().GetProperties().ToList().ForEach(prop =>
            {
                var propValue = prop.GetValue(filter, null);
                if (propValue != null && !propValue.ToString().Equals("0") && (!prop.ToString().Equals($"{DateTime.MinValue}") || !prop.ToString().Equals($"{DateTime.MaxValue}")))
                {
                    nameProp = Regex.Replace(prop.Name, @"(Equal|Contains|GreaterThan|LessThan|GreaterThanOrEquals|LessThanOrEquals|And|Or)", string.Empty);
                    prop.Name.ReturnStringTypeExp(out typeExpression);
                    var paramProp = typeof(E).GetProperty(nameProp);
                    Expression lambda = null;

                    lambda = typeExpression.SetExpressionType(param, paramProp, propValue);
                    if (predicate == null)
                        predicate = lambda.MergeExpressions<E>(param);
                    else
                        predicate = predicate.MergeExpressions<E>(mergeExpressionType, param, lambda.MergeExpressions<E>(param));
                    prop.Name.ReturnStringTypeMethod(out mergeExpressionType);
                }
            });
            return predicate;
        }

        private static Expression SetExpressionType(this string type, ParameterExpression parameter, PropertyInfo prop, object value)
        {
            Expression lambda = null;
            switch (type)
            {
                case "Equals":
                    return Expression.Equal(Expression.Property(parameter, prop), Expression.Constant(value));
                case "Contains":
                    if (prop.PropertyType == typeof(string))
                    {
                        MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        lambda = Expression.Call(Expression.Property(parameter, prop), method, Expression.Constant(value));
                    }
                    else
                        throw new NotSupportedException($"ERROR> ClassName: {nameof(SetExpressionType)} - {prop.Name} type is not string. This method only can be used by string type parameter.");
                    break;
                case "GreaterThan":
                    if (prop.ValidateTypeIsNotString("GreaterThan"))
                        lambda = Expression.GreaterThan(Expression.Property(parameter, prop), Expression.Constant(value));
                    break;
                case "LessThan":
                    if (prop.ValidateTypeIsNotString("LessThan"))
                        lambda = Expression.LessThan(Expression.Property(parameter, prop), Expression.Constant(value));
                    break;
                case "GreaterThanOrEqual":
                    if (prop.ValidateTypeIsNotString("GreaterThanOrEqual"))
                        lambda = Expression.GreaterThanOrEqual(Expression.Property(parameter, prop), Expression.Constant(value));
                    break;
                case "LessThanOrEqual":
                    if (prop.ValidateTypeIsNotString("LessThanOrEqual"))
                        lambda = Expression.LessThanOrEqual(Expression.Property(parameter, prop), Expression.Constant(value));
                    break;
                default:
                    lambda = Expression.Equal(Expression.Property(parameter, prop), Expression.Constant(value));
                    break;
            }
            return lambda;
        }

        private static bool ValidateTypeIsNotString(this PropertyInfo prop, string typeMethod)
        {
            if (prop.PropertyType != typeof(String))
                return true;
            else throw new NotSupportedException($"ERROR> ClassName: {nameof(SetExpressionType)} - {prop.Name} type is string. {typeMethod} method doesn't support this type. Please inform Contains or Equal.");
        }

        private static Expression<Func<E, bool>> MergeExpressions<E>(this Expression lambda, ParameterExpression parameter)
        where E : class => Expression.Lambda<Func<E, bool>>(lambda, parameter);

        private static Expression<Func<E, bool>> MergeExpressions<E>(this Expression<Func<E, bool>> predicate, string typeMerge, ParameterExpression parameter, Expression<Func<E, bool>> predicateMerge)
        where E : class
        {
            Expression lambda = null;
            switch (typeMerge)
            {
                case "And":
                    lambda = Expression.AndAlso(
                        Expression.Invoke(predicate, parameter),
                        Expression.Invoke(predicateMerge, parameter));
                    break;
                case "Or":
                    lambda = Expression.OrElse(
                        Expression.Invoke(predicate, parameter),
                        Expression.Invoke(predicateMerge, parameter));
                    break;
                default:
                    lambda = Expression.AndAlso(
                        Expression.Invoke(predicate, parameter),
                        Expression.Invoke(predicateMerge, parameter));
                    break;
            }
            return Expression.Lambda<Func<E, bool>>(lambda, parameter);
        }

        private static void ReturnStringTypeExp(this string value, out string output)
        {
            string returnString = "";
            if (!Regex.Match(value, @"(LessThan)").Success && !Regex.Match(value, @"(GreaterThan)").Success && Regex.Match(value, @"(Equal)").Success)
                returnString = "Equals";
            else if (Regex.Match(value, @"(Contains)").Success)
                returnString = "Contains";
            else if (!Regex.Match(value, @"(Equal)").Success && Regex.Match(value, @"(GreaterThan)").Success)
                returnString = "GreaterThan";
            else if (!Regex.Match(value, @"(Equal)").Success && Regex.Match(value, @"(LessThan)").Success)
                returnString = "LessThan";
            else if (Regex.Match(value, @"(GreaterThanOrEqual)").Success)
                returnString = "GreaterThanOrEqual";
            else if (Regex.Match(value, @"(LessThanOrEqual)").Success)
                returnString = "LessThanOrEqual";
            else returnString = "Equals";
            output = returnString;
        }

        private static void ReturnStringTypeMethod(this string value, out string output)
        {
            string returnString = "";
            if (Regex.Match(value, @"(And)").Success)
                returnString = "And";
            else if (Regex.Match(value, @"(Or)").Success)
                returnString = "Or";
            else returnString = "And";
            output = returnString;
        }
    }
}