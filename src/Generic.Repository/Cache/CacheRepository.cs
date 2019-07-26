using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private IDictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> CacheAttribute { get; set; }
        private IDictionary<string, Dictionary<string, Func<object, object>>> CacheGet { get; set; }
        private IDictionary<string, Dictionary<string, Action<object, object>>> CacheSet { get; set; }
        private IDictionary<string, Dictionary<string, PropertyInfo>> CacheProperties { get; set; }
        private IDictionary<Type, MethodInfo> CacheMethod { get; set; }
        private ICacheRepositoryFacade CacheFacade { get; }

        public CacheRepository(ICacheRepositoryFacade cacheFacade)
        {
            CacheFacade = cacheFacade;
            CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>();
            CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>();
            CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>();
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>();
        }

        public CacheRepository(ICacheRepositoryFacade cacheFacade, string AssemblyName, string Namespace)
        {
            CacheFacade = cacheFacade;
            if (!string.IsNullOrEmpty(AssemblyName) && !string.IsNullOrEmpty(Namespace))
            {
                var size = Assembly.Load(AssemblyName).GetTypes().Where(x => Namespace.Split(';').Contains(x.Namespace)).Count();
                if (size == 0)
                {
                    throw new NullReferenceException(nameof(Cache));
                }
                CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>(size);
                CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>(size);
                CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>(size);
                CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>(size);
            }
        }

        public Func<object, object> GetMethodGet(string objectKey, string propertieKey)
        {
            var dicResult = GetData<Dictionary<string, Func<object, object>>, string>(CacheGet, objectKey);
            var result = GetData<Func<object, object>, string>(dicResult, propertieKey);
            return result;
        }

        public IDictionary<string, Func<object, object>> GetDictionaryMethodGet(string objectKey)
        {
            var dictionary = GetData<Dictionary<string, Func<object, object>>, string>(CacheGet, objectKey);
            return dictionary;
        }

        public Action<object, object> GetMethodSet(string objectKey, string propertieKey)
        {
            var dicResult = GetData<Dictionary<string, Action<object, object>>, string>(CacheSet, objectKey);
            var result = GetData<Action<object, object>, string>(dicResult, propertieKey);
            return result;
        }

        public IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey)
        {
            var dictionary = GetData<Dictionary<string, Action<object, object>>, string>(CacheSet, objectKey);
            return dictionary;
        }

        public CustomAttributeTypedArgument GetAttribute(string objectKey, string propertieKey, string customAttirbuteKey)
        {
            var dictionaryI = GetData<Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>, string>(CacheAttribute, objectKey);
            var dictionaryII = GetData<Dictionary<string, CustomAttributeTypedArgument>, string>(dictionaryI, propertieKey);
            var result = GetData<CustomAttributeTypedArgument, string>(dictionaryII, customAttirbuteKey);
            return result;
        }

        public IDictionary<string, CustomAttributeTypedArgument> GetDictionaryAttribute(string objectKey, string propertieKey)
        {
            var dictionaryI = GetData<Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>, string>(CacheAttribute, objectKey);
            var result = GetData<Dictionary<string, CustomAttributeTypedArgument>, string>(dictionaryI, propertieKey);
            return result;
        }

        public IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey)
        {
            var result = GetData<Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>, string>(CacheAttribute, objectKey);
            return result;
        }

        public PropertyInfo GetProperty(string objectKey, string propertieKey)
        {
            var dictionary = GetData<Dictionary<string, PropertyInfo>, string>(CacheProperties, objectKey);
            var result = GetData<PropertyInfo, string>(dictionary, propertieKey);
            return result;
        }

        public IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey)
        {
            var result = GetData<Dictionary<string, PropertyInfo>, string>(CacheProperties, objectKey);
            return result;
        }

        public MethodInfo GetMethod(Type key)
        {
            var result = GetData<MethodInfo, Type>(CacheMethod, key);
            return result;
        }

        public IDictionary<Type, MethodInfo> GetDictionaryMethod()
        {
            return CacheMethod;
        }

        public void SaveOnCacheIfNonExists<TValue>() =>
            SaveOnCacheIfNonExists<TValue>(true, true, true, true);

        public void SaveOnCacheIfNonExists<TValue>(bool saveAttribute) =>
            SaveOnCacheIfNonExists<TValue>(saveAttribute, true, true, true);

        public void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet) =>
            SaveOnCacheIfNonExists<TValue>(saveAttribute, saveGet, true, true);

        public void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet, bool saveSet) =>
            SaveOnCacheIfNonExists<TValue>(saveAttribute, saveGet, saveSet, true);

        public void SaveOnCacheIfNonExists<TValue>(bool saveAttribute, bool saveGet, bool saveSet, bool saveProperties)
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

        private void CachingAttribute(PropertyInfo propertyInfo, string typeName)
        {
            string propetyName = propertyInfo.Name;
            CacheAttribute[typeName].Add(propetyName, propertyInfo.GetCustomAttributesData().SelectMany(x => x.NamedArguments).ToDictionary(x => x.MemberName, x => x.TypedValue));
        }

        private R GetData<R, T>(IDictionary<T, R> dictionary, T key)
        {
            if (dictionary.TryGetValue(key, out var result))
            {
                return result;
            }
            else throw new ArgumentNullException($"FIELD> {nameof(key)} VALUE>{key}");
        }
    }
}
