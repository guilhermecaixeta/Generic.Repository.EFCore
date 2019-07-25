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
        MethodInfo GetMethod(Type key);
        IDictionary<Type, MethodInfo> GetDictionaryMethod();

        void SaveOnCacheIfNonExists<TValue>();

        void SaveOnCacheIfNonExists<TValue>(bool saveAttribute);

        void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet);

        void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet, bool saveSet);

        void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet, bool saveSet, bool saveProperties);

    }
}
