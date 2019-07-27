using System;
using System.Collections.Generic;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public interface ICacheRepository
    {
        Func<object, object> GetMethodGet(string objectKey, string propertieKey);
        IDictionary<string, Func<object, object>> GetDictionaryMethodGet(string objectKey);
        Action<object, object> GetMethodSet(string objectKey, string propertieKey);
        IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey);
        PropertyInfo GetProperty(string objectKey, string propertieKey);
        IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey);
        CustomAttributeTypedArgument GetAttribute(string objectKey, string propertieKey, string customAttirbuteKey);
        IDictionary<string, CustomAttributeTypedArgument> GetDictionaryAttribute(string objectKey, string propertieKey);
        IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey);

        void Add<TValue>();

        void Add<TValue>(bool saveAttribute);

        void Add<TValue>(bool saveAttribute, bool saveGet);

        void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet);

        void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet, bool saveProperties);

        bool HasMethodSet();
        
        bool HasMethodGet();
        
        bool HasProperty();
        
        bool HasAttribute();

        void ClearCache();

    }
}
