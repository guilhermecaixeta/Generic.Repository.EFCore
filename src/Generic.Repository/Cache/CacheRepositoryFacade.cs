using System;
using System.Collections.Generic;
using System.Reflection;
using Generic.Repository.Extension.Validation;

namespace Generic.Repository.Cache
{
    internal class CacheRepositoryFacade : ICacheRepositoryFacade
    {
        public Func<object, object> CreateFunction<TValue>(PropertyInfo property)
        {
            property.IsNull(property.Name, nameof(CreateFunction));
            var getter = property.GetGetMethod(true);
            getter.IsNull($"ClassName: {nameof(CreateFunction)} {Environment.NewLine}Message: The property {property.Name} does not have a public accessor.");

            return (Func<object, object>)ExtractMethod<TValue>(getter, property, "CreateFunctionGeneric");
        }

        public Func<object, object> CreateFunctionGeneric<TValue, TReturn>(MethodInfo getter)
        {
            Func<TValue, TReturn> getterTypedDelegate = (Func<TValue, TReturn>)Delegate.CreateDelegate(typeof(Func<TValue, TReturn>), getter);
            Func<object, object> getterDelegate = ((object instance) => getterTypedDelegate((TValue)instance));
            return getterDelegate;
        }

        public Action<object, object> CreateAction<TValue>(PropertyInfo property)
        {
            property.IsNull(property.Name, nameof(CreateAction));
            var setter = property.GetSetMethod(true);
            setter.IsNull($"ClassName: {nameof(CreateAction)} {Environment.NewLine}Message: The property {property.Name} does not have a public setter.");

            return (Action<object, object>)ExtractMethod<TValue>(setter, property, "CreateActionGeneric");
        }

        public Action<object, object> CreateActionGeneric<TValue, TInput>(MethodInfo setter)
        {
            Action<TValue, TInput> setterTypedDelegate = (Action<TValue, TInput>)Delegate.CreateDelegate(typeof(Action<TValue, TInput>), setter);
            Action<object, object> setterDelegate = (object instance, object value) => { setterTypedDelegate((TValue)instance, (TInput)value); };
            return setterDelegate;
        }

        private object ExtractMethod<TValue>(MethodInfo method, PropertyInfo property, string nameMethod)
        {
            method.IsNull(nameof(ExtractMethod), nameof(method));
            var type = typeof(ICacheRepositoryFacade);
            var genericMethod = type.GetMethod(nameMethod);
            var genericHelper = genericMethod.MakeGenericMethod(typeof(TValue), property.PropertyType);

            return genericHelper.Invoke(this, new object[] { method });
        }

        public R GetData<R>(IDictionary<string, R> dictionary, string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException($"FIELD> {nameof(key)} METHOD> {nameof(GetData)}");
            }
            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }
            else throw new KeyNotFoundException($"FIELD> {nameof(key)} VALUE> {key} METHOD> {nameof(GetData)}");
        }
    }
}
