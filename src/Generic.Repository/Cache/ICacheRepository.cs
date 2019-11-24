using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    public interface ICacheRepository
    {
        Task<Func<object, object>> GetMethodGet(string objectKey, string propertyKey, CancellationToken token);

        Task<IDictionary<string, Func<object, object>>> GetDictionaryMethodGet(string objectKey, CancellationToken token);

        Task<Action<object, object>> GetMethodSet(string objectKey, string propertyKey, CancellationToken token);

        Task<IDictionary<string, Action<object, object>>> GetDictionaryMethodSet(string objectKey, CancellationToken token);

        Task<PropertyInfo> GetProperty(string objectKey, string propertyKey, CancellationToken token);

        Task<IDictionary<string, PropertyInfo>> GetDictionaryProperties(string objectKey, CancellationToken token);

        Task<CustomAttributeTypedArgument> GetAttribute(string objectKey, string propertyKey, string customAttributeKey, CancellationToken token);

        Task<IDictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey, string propertyKey, CancellationToken token);

        Task<IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> GetDictionaryAttribute(string objectKey, CancellationToken token);

        Task AddGet<TValue>(CancellationToken token);

        Task AddSet<TValue>(CancellationToken token);

        Task AddProperty<TValue>(CancellationToken token);

        Task AddAttribute<TValue>(CancellationToken token);

        Task<bool> HasMethodSet(CancellationToken token);

        Task<bool> HasMethodGet(CancellationToken token);

        Task<bool> HasProperty(CancellationToken token);

        Task<bool> HasAttribute(CancellationToken token);

        void ClearCache();

    }
}
