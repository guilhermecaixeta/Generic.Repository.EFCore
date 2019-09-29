using Generic.Repository.Extension.Filter.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private int Size { get; set; }

        private IDictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> CacheAttribute { get; set; }

        private IDictionary<string, Dictionary<string, ExpressionUpdatingFacade>> CacheExpression { get; set; }

        private IDictionary<string, Dictionary<string, Func<object, object>>> CacheGet { get; set; }

        private IDictionary<string, Dictionary<string, Action<object, object>>> CacheSet { get; set; }

        private IDictionary<string, Dictionary<string, PropertyInfo>> CacheProperties { get; set; }

        private ICacheRepositoryFacade CacheFacade { get; }

        public CacheRepository()
        {
            CacheFacade = new CacheRepositoryFacade();
            InitCache();
        }

        public CacheRepository(
            string AssemblyName,
            string Namespace)
        {
            CacheFacade = new CacheRepositoryFacade();
            if (!string.IsNullOrEmpty(AssemblyName) && !string.IsNullOrEmpty(Namespace))
            {
                Size = Assembly.
                Load(AssemblyName).
                GetTypes().
                Where(x => Namespace.Split(';').Contains(x.Namespace)).
                Count();
                if (Size > 0)
                {
                    InitCache(Size);
                }
                InitCache();
            }
        }

        public ExpressionUpdatingFacade GetExpression(string objectKey, string propertieKey)
        {
            if (!CacheExpression.Any())
            {
                return null;
            }
            var dictionary = CacheFacade.GetData(CacheExpression, objectKey);
            var result = CacheFacade.GetData(dictionary, propertieKey);
            return result;
        }

        public Func<object, object> GetMethodGet(
            string objectKey,
            string propertieKey)
        {
            var dicResult = CacheFacade.
            GetData(CacheGet, objectKey);

            var result = CacheFacade.
            GetData(dicResult, propertieKey);

            return result;
        }

        public IDictionary<string, Func<object, object>> GetDictionaryMethodGet(
            string objectKey)
        {
            var dictionary = CacheFacade.
            GetData(CacheGet, objectKey);
            return dictionary;
        }

        public Action<object, object> GetMethodSet(
            string objectKey,
            string propertyKey)
        {
            var dicResult = CacheFacade.
            GetData<Dictionary<string, Action<object, object>>>(CacheSet, objectKey);

            var result = CacheFacade.
            GetData<Action<object, object>>(dicResult, propertyKey);
            return result;
        }

        public IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey)
        {
            var dictionary = CacheFacade.GetData<Dictionary<string, Action<object, object>>>(CacheSet, objectKey);
            return dictionary;
        }

        public CustomAttributeTypedArgument GetAttribute(
            string objectKey,
            string propertyKey,
            string customAttributeKey)
        {
            var dictionaryI = CacheFacade
            .GetData<Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>(
                CacheAttribute,
                objectKey);

            var dictionaryII = CacheFacade
            .GetData<Dictionary<string, CustomAttributeTypedArgument>>(
                dictionaryI,
                propertyKey);

            var result = CacheFacade
            .GetData<CustomAttributeTypedArgument>(
                dictionaryII,
                customAttributeKey);

            return result;
        }

        public IDictionary<string, CustomAttributeTypedArgument> GetDictionaryAttribute(string objectKey, string propertyKey)
        {
            var dictionaryI = CacheFacade.GetData(CacheAttribute, objectKey);
            var result = CacheFacade.GetData(dictionaryI, propertyKey);
            return result;
        }

        public IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey)
        {
            var result = CacheFacade.GetData(CacheAttribute, objectKey);
            return result;
        }

        public PropertyInfo GetProperty(string objectKey, string propertyKey)
        {
            var dictionary = CacheFacade.GetData(CacheProperties, objectKey);
            var result = CacheFacade.GetData(dictionary, propertyKey);
            return result;
        }

        public IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey)
        {
            var result = CacheFacade.GetData(CacheProperties, objectKey);
            return result;
        }

        public void Add<TValue>() =>
            Add<TValue>(true, true, true, true);

        public void Add<TValue>(bool saveAttribute) =>
            Add<TValue>(saveAttribute, true, true, true);

        public void Add<TValue>(bool saveAttribute, bool saveGet) =>
            Add<TValue>(saveAttribute, saveGet, true, true);

        public void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet) =>
            Add<TValue>(saveAttribute, saveGet, saveSet, true);

        public void Add<TValue>(bool saveAttribute, bool saveGet, bool saveSet, bool saveProperties)
        {
            string typeName = typeof(TValue).Name;
            PropertyInfo[] properties = typeof(TValue).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            if (saveGet && !CacheGet.ContainsKey(typeName))
            {
                CacheGet.Add(typeName, properties.ToDictionary(g => g.Name, m => CacheFacade.CreateFunction<TValue>(m)));
            }
            if (saveSet && !CacheSet.ContainsKey(typeName))
            {
                CacheSet.Add(typeName, properties.ToDictionary(s => s.Name, m => CacheFacade.CreateAction<TValue>(m)));
            }
            if (saveProperties && !CacheProperties.ContainsKey(typeName))
            {
                CacheProperties.Add(typeName, properties.ToDictionary(p => p.Name, p => p));
            }
            if (saveAttribute && !CacheAttribute.ContainsKey(typeName))
            {
                CacheAttribute.Add(typeName, new Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>(properties.Length));
                foreach (var property in properties)
                {
                    CachingAttribute(property, typeName);
                }
            }
        }

        public void AddExpression(string key, string propertieKey, ExpressionUpdatingFacade expressionUpdating)
        {
            var dictionary = new Dictionary<string, ExpressionUpdatingFacade> { { propertieKey, expressionUpdating } };

            CacheExpression.Add(key, dictionary);
        }

        public bool HasMethodSet() => CacheSet.Any();

        public bool HasMethodGet() => CacheGet.Any();

        public bool HasProperty() => CacheProperties.Any();

        public bool HasAttribute() => CacheAttribute.Any();

        public void ClearCache()
        {
            InitCache();
        }

        private void CachingAttribute(PropertyInfo propertyInfo, string typeName)
        {
            var propertyName = propertyInfo.Name;
            CacheAttribute[typeName].Add(propertyName, propertyInfo.
                GetCustomAttributesData().
                SelectMany(x => x.NamedArguments).
                ToDictionary(x => x.MemberName, x => x.TypedValue));
        }

        private void InitCache()
        {
            CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>();
            CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>();
            CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>();
            CacheExpression = new Dictionary<string, Dictionary<string, ExpressionUpdatingFacade>>();
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>();
        }

        private void InitCache(int size)
        {
            CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>(size);
            CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>(size);
            CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>(size);
            CacheExpression = new Dictionary<string, Dictionary<string, ExpressionUpdatingFacade>>();
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>(size);
        }
    }
}
