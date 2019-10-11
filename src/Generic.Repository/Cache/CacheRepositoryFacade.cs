using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Generic.Repository.Cache
{
    internal class CacheRepositoryFacade : ICacheRepositoryFacade
    {
        public Func<object, object> CreateFunction<TValue>(PropertyInfo property)
        {
            ThrowErrorIf.
                IsNullValue(property, nameof(property), nameof(CreateFunction));
            
            var getter = property.
                GetGetMethod(true);
            
            ThrowErrorIf.
                IsNullValue(getter, nameof(property), nameof(CreateFunction));

            return (Func<object, object>)ExtractMethod<TValue>(getter, property, "CreateFunctionGeneric");
        }

        public Func<object, object> CreateFunctionGeneric<TValue, TReturn>(MethodInfo getter)
        {
            var getterTypedDelegate = (Func<TValue, TReturn>)Delegate.
                CreateDelegate(typeof(Func<TValue, TReturn>), getter);
            
            object GetterDelegate(object instance) => 
                getterTypedDelegate((TValue) instance);
            
            return GetterDelegate;
        }

        public Action<object, object> CreateAction<TValue>(PropertyInfo property)
        {
            ThrowErrorIf.
                IsNullValue(property, nameof(property), nameof(ExtractMethod));
            
            var setter = property.
                GetSetMethod(true);
            
            ThrowErrorIf.
                IsNullValue(setter, nameof(setter), nameof(ExtractMethod)); ;

            var result = (Action<object, object>) ExtractMethod<TValue>(setter, property, "CreateActionGeneric");

            return result;
        }

        public Action<object, object> CreateActionGeneric<TValue, TInput>(MethodInfo setter)
        {
            var setterTypedDelegate = (Action<TValue, TInput>)Delegate.
                CreateDelegate(typeof(Action<TValue, TInput>), setter);
            
            void SetterDelegate(object instance, object value) => 
                setterTypedDelegate((TValue) instance, (TInput) value);
            
            return SetterDelegate;
        }

        private object ExtractMethod<TValue>(MethodInfo method, PropertyInfo property, string nameMethod)
        {
            ThrowErrorIf.
                IsNullValue(method, nameof(method), nameof(ExtractMethod));
            
            var type = typeof(ICacheRepositoryFacade);
            var genericMethod = type.GetMethod(nameMethod);
            var genericHelper = genericMethod?.
                MakeGenericMethod(typeof(TValue), property.PropertyType);

            return genericHelper?.
                Invoke(this, new object[] { method });
        }

        public R GetData<R>(IDictionary<string, R> dictionary, string key)
        {
            ThrowErrorIf.
                IsEmptyOrNullString(key, nameof(key), nameof(GetData));
            
            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }
            throw new KeyNotFoundException($"FIELD> {nameof(key)} VALUE> {key}");
        }
    }
}
