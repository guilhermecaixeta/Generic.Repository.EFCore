using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    /// <summary>
    /// Cache repository facade.
    /// </summary>
    internal interface ICacheRepositoryFacade
    {
        /// <summary>Creates the action.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        Action<object, object> CreateAction<TValue>(PropertyInfo property);

        /// <summary>Creates the action generic.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="setter">The setter.</param>
        /// <returns></returns>
        Action<object, object> CreateActionGeneric<TValue, TInput>(MethodInfo setter);

        /// <summary>Creates the function.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        Func<object, object> CreateFunction<TValue>(PropertyInfo property);

        /// <summary>Creates the function generic.</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="getter">The getter.</param>
        /// <returns></returns>
        Func<object, object> CreateFunctionGeneric<TValue, TReturn>(MethodInfo getter);

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<R> GetData<R>(IDictionary<string, R> dictionary, string key, CancellationToken token);

        /// <summary>
        /// Processes the action with lock.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task ProcessActionWithLock(Action @delegate, CancellationToken token);
    }
}