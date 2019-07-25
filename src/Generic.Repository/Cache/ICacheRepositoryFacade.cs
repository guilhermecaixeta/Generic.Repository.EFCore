using System;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public interface ICacheRepositoryFacade
    {
        Func<object, object> CreateFunction<TValue>(PropertyInfo property);

        Func<object, object> CreateFunctionGeneric<TValue, TReturn>(MethodInfo getter);

        Action<object, object> CreateAction<TValue>(PropertyInfo property);

        Action<object, object> CreateActionGeneric<TValue, TInput>(MethodInfo setter);
    }
}
