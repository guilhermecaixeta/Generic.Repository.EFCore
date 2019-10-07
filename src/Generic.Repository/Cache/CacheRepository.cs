using Generic.Repository.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generic.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private static readonly IsError IsError = new IsError();

        private int Size { get; set; }

        private IDictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> CacheAttribute { get; set; }

        private IDictionary<string, Dictionary<string, Func<object, object>>> CacheGet { get; set; }

        private IDictionary<string, Dictionary<string, Action<object, object>>> CacheSet { get; set; }

        private IDictionary<string, Dictionary<string, PropertyInfo>> CacheProperties { get; set; }

        private ICacheRepositoryFacade CacheFacade { get; } = new CacheRepositoryFacade(IsError);

        public CacheRepository()
        {
            InitCache();
        }

        /// <summary>Initializes a new instance of the <see cref="CacheRepository"/> class.</summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="nameSpace">The name spaces splited by ";".</param>
        public CacheRepository(
            string assemblyName,
            string nameSpace)
        {
            IsError.
                IsThrowErrorEmptyOrNullString(assemblyName, nameof(assemblyName), nameof(CacheRepository));

            IsError.
                IsThrowErrorEmptyOrNullString(nameSpace, nameof(nameSpace), nameof(CacheRepository));

            Size = Assembly.
                Load(assemblyName).
                GetTypes().
                Count(x => nameSpace.Split(';').Contains(x.Namespace));

            if (Size > 0)
            {
                InitCache(Size);
            }
        }

        public Func<object, object> GetMethodGet(
            string objectKey,
            string propertyKey)
        {
            var dicResult = CacheFacade.
            GetData(CacheGet, objectKey);

            var result = CacheFacade.
            GetData(dicResult, propertyKey);

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
            GetData(CacheSet, objectKey);

            var result = CacheFacade.
            GetData(dicResult, propertyKey);
            return result;
        }

        public IDictionary<string, Action<object, object>> GetDictionaryMethodSet(string objectKey)
        {
            var dictionary = CacheFacade.GetData(CacheSet, objectKey);
            return dictionary;
        }

        public CustomAttributeTypedArgument GetAttribute(
            string objectKey,
            string propertyKey,
            string customAttributeKey)
        {
            var dictionaryI = CacheFacade
            .GetData(
                CacheAttribute,
                objectKey);

            var dictionaryII = CacheFacade
            .GetData(
                dictionaryI,
                propertyKey);

            var result = CacheFacade
            .GetData(
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
            var dictionary = CacheFacade.
                GetData(CacheProperties, objectKey);

            var result = CacheFacade.
                GetData(dictionary, propertyKey);

            return result;
        }

        public IDictionary<string, PropertyInfo> GetDictionaryProperties(string objectKey)
        {
            var result = CacheFacade.
                GetData(CacheProperties, objectKey);

            return result;
        }

        public void AddGet<TValue>()
        {
            var values = GetValues<TValue>();

            if (!CacheGet.ContainsKey(values.typeName))
            {
                var dictionary = values.properties.
                    ToDictionary(g => g.Name, m => CacheFacade.CreateFunction<TValue>(m));
                CacheGet.Add(values.typeName, dictionary);
            }
        }

        public void AddSet<TValue>()
        {
            var values = GetValues<TValue>();

            if (!CacheSet.ContainsKey(values.typeName))
            {
                var dictionary = values.properties.
                    ToDictionary(s => s.Name, m => CacheFacade.CreateAction<TValue>(m));
                CacheSet.Add(values.typeName, dictionary);
            }
        }

        public void AddProperty<TValue>()
        {
            var values = GetValues<TValue>();

            if (!CacheProperties.ContainsKey(values.typeName))
            {
                CacheProperties.Add(values.typeName, values.properties.ToDictionary(p => p.Name, p => p));
            }
        }

        public void AddAttribute<TValue>()
        {
            var values = GetValues<TValue>();

            if (!CacheAttribute.ContainsKey(values.typeName))
            {
                var dictionary = new Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>(values.properties.Length);

                CacheAttribute.Add(values.typeName, dictionary);

                foreach (var property in values.properties)
                {
                    SetCacheAttributes(property, values.typeName);
                }
            }
        }

        public bool HasMethodSet() => CacheSet.Any();

        public bool HasMethodGet() => CacheGet.Any();

        public bool HasProperty() => CacheProperties.Any();

        public bool HasAttribute() => CacheAttribute.Any();

        public void ClearCache()
        {
            if (Size > 0)
            {
                InitCache(Size);
            }
            InitCache();
        }

        private void SetCacheAttributes(MemberInfo propertyInfo, string typeName)
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
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>();
        }

        private void InitCache(int size)
        {
            CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>(size);
            CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>(size);
            CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>(size);
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>(size);
        }

        private (string typeName, PropertyInfo[] properties) GetValues<TValue>()
        {
            var typeName = typeof(TValue).Name;

            PropertyInfo[] properties = typeof(TValue).
                GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            IsError.
                IsThrowErrorNullOrEmptyList(properties, nameof(properties), string.Empty);

            return (typeName, properties);
        }
    }
}
