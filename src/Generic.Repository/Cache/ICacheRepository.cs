using Generic.Repository.Extension.Filter.Facade;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public interface ICacheRepository
    {
        Func<object, object> GetMethodGet(string objectKey, string propertyKey);

        IDictionary<string, Func<object, object>> GetDictionaryMethodGet(string objectKey);

        Action<object, object> GetMethodSet(string objectKey, string propertyKey);

        IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey);

        PropertyInfo GetProperty(string objectKey, string propertyKey);

        IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey);

        CustomAttributeTypedArgument GetAttribute(string objectKey, string propertyKey, string customAttributeKey);

        IDictionary<string, CustomAttributeTypedArgument> GetDictionaryAttribute(string objectKey, string propertyKey);

        IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey);

        void AddGet<TValue>();

        void AddSet<TValue>();

        void AddProperty<TValue>();

        void AddAttribute<TValue>();

        bool HasMethodSet();

        bool HasMethodGet();

        bool HasProperty();

        bool HasAttribute();

        void ClearCache();

    }
}
