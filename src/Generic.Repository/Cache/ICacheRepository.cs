using Generic.Repository.Extension.Filter.Facade;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public interface ICacheRepository
    {
        ExpressionUpdatingFacade GetExpression(string objectKey, string propertyKey);

        Func<object, object> GetMethodGet(string objectKey, string propertyKey);

        IDictionary<string, Func<object, object>> GetDictionaryMethodGet(string objectKey);

        Action<object, object> GetMethodSet(string objectKey, string propertyKey);

        IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey);

        PropertyInfo GetProperty(string objectKey, string propertyKey);

        IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey);

        CustomAttributeTypedArgument GetAttribute(string objectKey, string propertyKey, string customAttributeKey);

        IDictionary<string, CustomAttributeTypedArgument> GetDictionaryAttribute(string objectKey, string propertyKey);

        IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey);

        void Add<TValue>();

        void Add<TValue>(bool saveAttribute);

        void Add<TValue>(bool saveAttribute, bool saveGet);

        void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet);

        void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet, bool saveProperties);

        void AddExpression(string key, string propertieKey, ExpressionUpdatingFacade expressionUpdating);

        bool HasMethodSet();

        bool HasMethodGet();

        bool HasProperty();

        bool HasAttribute();

        void ClearCache();

    }
}
