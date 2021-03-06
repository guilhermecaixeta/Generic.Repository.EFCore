using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    internal class CacheRepositoryFacade : ICacheRepositoryFacade
    {
        private readonly object _delegateLock = new object();

        public Action<object, object> CreateAction<TValue>(PropertyInfo property)
        {
            ThrowErrorIf.
                IsNullValue(property, nameof(property), nameof(CreateAction));

            var setter = property.
                GetSetMethod(true);

            ThrowErrorIf.
                IsNullValue(setter, nameof(setter), nameof(CreateAction));

            var result = ExtractMethod<TValue, Action<object, object>>(setter, property, nameof(CreateActionGeneric));

            return result;
        }

        public Action<object, object> CreateActionGeneric<TValue, TInput>(MethodInfo setter)
        {
            var setterTypedDelegate = (Action<TValue, TInput>)Delegate.
                CreateDelegate(typeof(Action<TValue, TInput>), setter);

            void SetterDelegate(object instance, object value) =>
                setterTypedDelegate((TValue)instance, (TInput)value);

            return SetterDelegate;
        }

        public Func<object, object> CreateFunction<TValue>(PropertyInfo property)
        {
            ThrowErrorIf.
                IsNullValue(property, nameof(property), nameof(CreateFunction));

            var getter = property.
                GetGetMethod(true);

            ThrowErrorIf.
                IsNullValue(getter, nameof(property), nameof(CreateFunction));

            return ExtractMethod<TValue, Func<object, object>>(getter, property, nameof(CreateFunctionGeneric));
        }

        public Func<object, object> CreateFunctionGeneric<TValue, TReturn>(MethodInfo getter)
        {
            var getterTypedDelegate = (Func<TValue, TReturn>)Delegate.
                CreateDelegate(typeof(Func<TValue, TReturn>), getter);

            object GetterDelegate(object instance) =>
                getterTypedDelegate((TValue)instance);

            return GetterDelegate;
        }

        public async Task<TReturn> GetData<TReturn>(IDictionary<string, TReturn> dictionary, string key, CancellationToken token)
        {
            return await Task.Run(() =>
            {
                ThrowErrorIf.
                    IsEmptyOrNullString(key, nameof(key), nameof(GetData));

                dictionary.TryGetValue(key, out var result);

                return result;
            }, token).
            ConfigureAwait(false);
        }

        public async Task RunActionInSemaphore(Action @delegate, CancellationToken token)
        {
            await Task.Run(() =>
            {
                lock (_delegateLock)
                {
                    @delegate();
                }
            }, token).ConfigureAwait(false);
        }

        private TReturn ExtractMethod<TValue, TReturn>(MethodInfo method, PropertyInfo property, string nameMethod)
        {
            ThrowErrorIf.
                IsNullValue(method, nameof(method), nameof(ExtractMethod));

            var type = typeof(ICacheRepositoryFacade);
            var genericMethod = type.GetMethod(nameMethod);
            var genericHelper = genericMethod?.
                MakeGenericMethod(typeof(TValue), property.PropertyType);

            var extractedMethod = (TReturn)genericHelper?.
                Invoke(this, new object[] { method });

            return extractedMethod;
        }
    }
}