using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    public interface ICacheRepository
    {
        Task<Func<object, object>> GetMethodGet(string objectKey, string propertyKey);

        Task<IDictionary<string, Func<object, object>>> GetDictionaryMethodGet(string objectKey);

        Task<Action<object, object>> GetMethodSet(string objectKey, string propertyKey);

        Task<IDictionary<string, Action<object, object>>> GetDictionaryMethodSet(string objectKey);

        Task<PropertyInfo> GetProperty(string objectKey, string propertyKey);

        Task<IDictionary<string, PropertyInfo>> GetDictionaryProperties(string objectKey);

        Task<CustomAttributeTypedArgument> GetAttribute(string objectKey, string propertyKey, string customAttributeKey);

        Task<IDictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey, string propertyKey);

        Task<IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> GetDictionaryAttribute(string objectKey);

        Task AddGet<TValue>();

        Task AddSet<TValue>();

        Task AddProperty<TValue>();

        Task AddAttribute<TValue>();

        bool HasMethodSet();

        bool HasMethodGet();

        bool HasProperty();

        bool HasAttribute();

        void ClearCache();

    }
}
