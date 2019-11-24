using Generic.Repository.Attributes;
using Generic.Repository.Validations.ThrowError;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private IDictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> CacheAttribute { get; set; }

        private IDictionary<string, Dictionary<string, Func<object, object>>> CacheGet { get; set; }

        private IDictionary<string, Dictionary<string, Action<object, object>>> CacheSet { get; set; }

        private IDictionary<string, Dictionary<string, PropertyInfo>> CacheProperties { get; set; }

        private ICacheRepositoryFacade CacheFacade { get; } = new CacheRepositoryFacade();

        public CacheRepository()
        {
            InitCache();
        }

        public async Task<Func<object, object>> GetMethodGet(
            string objectKey,
            string propertyKey,
            CancellationToken token)
        {
            var dicResult = await CacheFacade.
                GetData(CacheGet, objectKey, token);

            var result = await CacheFacade.
                GetData(dicResult, propertyKey, token);

            return result;
        }

        public async Task<IDictionary<string, Func<object, object>>> GetDictionaryMethodGet(
            string objectKey,
            CancellationToken token)
        {
            var dictionary = await CacheFacade.
                GetData(CacheGet, objectKey, token);

            return dictionary;
        }

        public async Task<Action<object, object>> GetMethodSet(
            string objectKey,
            string propertyKey,
            CancellationToken token)
        {
            var dicResult = await CacheFacade.
                GetData(CacheSet, objectKey, token);

            var result = await CacheFacade.
                GetData(dicResult, propertyKey, token);

            return result;
        }

        public async Task<IDictionary<string, Action<object, object>>> GetDictionaryMethodSet(
            string objectKey,
            CancellationToken token)
        {
            var dictionary = await CacheFacade.GetData(CacheSet, objectKey, token);
            return dictionary;
        }

        public async Task<CustomAttributeTypedArgument> GetAttribute(
            string objectKey,
            string propertyKey,
            string customAttributeKey,
            CancellationToken token)
        {
            var dictionaryI = await CacheFacade
            .GetData(
                CacheAttribute,
                objectKey, token);

            var dictionaryII = await CacheFacade
            .GetData(
                dictionaryI,
                propertyKey, token);

            var result = await CacheFacade
            .GetData(
                dictionaryII,
                customAttributeKey, token);

            return result;
        }

        public async Task<IDictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(
            string objectKey,
            string propertyKey,
            CancellationToken token)
        {
            var dictionaryI = await CacheFacade.GetData(CacheAttribute, objectKey, token);

            var result = await CacheFacade.GetData(dictionaryI, propertyKey, token);

            return result;
        }

        public async Task<IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> GetDictionaryAttribute(
            string objectKey,
            CancellationToken token)
        {
            var result = await CacheFacade.GetData(CacheAttribute, objectKey, token);

            return result;
        }

        public async Task<PropertyInfo> GetProperty(
            string objectKey,
            string propertyKey,
            CancellationToken token)
        {
            var dictionary = await CacheFacade.
                GetData(CacheProperties, objectKey, token);

            var result = await CacheFacade.
                GetData(dictionary, propertyKey, token);

            return result;
        }

        public async Task<IDictionary<string, PropertyInfo>> GetDictionaryProperties(
            string objectKey,
            CancellationToken token)
        {
            var result = await CacheFacade.
                GetData(CacheProperties, objectKey, token);

            return result;
        }

        public async Task AddGet<TValue>(
            CancellationToken token)
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();

                var valid = CacheGet.ContainsKey(values.typeName);

                if (valid || !values.cacheable)
                {
                    return;
                }

                var dictionary = values.properties.
                       ToDictionary(g => g.Name, m => CacheFacade.CreateFunction<TValue>(m));
                CacheGet.Add(values.typeName, dictionary);
            }

            await CacheFacade.ProcessSemaphore(ActionAdd, token);
        }

        public async Task AddSet<TValue>(
            CancellationToken token)
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();

                var valid = CacheSet.ContainsKey(values.typeName);

                if (valid || !values.cacheable)
                {
                    return;
                }

                var dictionary = values.properties.
                                        ToDictionary(s => s.Name, m => CacheFacade.CreateAction<TValue>(m));
                CacheSet.Add(values.typeName, dictionary);
            }

            await CacheFacade.ProcessSemaphore(ActionAdd, token);
        }

        public async Task AddProperty<TValue>(
            CancellationToken token)
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();
                
                var valid = CacheProperties.ContainsKey(values.typeName);

                if (valid || !values.cacheable)
                {
                    return;
                }

                CacheProperties.Add(values.typeName, values.properties.ToDictionary(p => p.Name, p => p));
            }

            await CacheFacade.ProcessSemaphore(ActionAdd, token);
        }

        public async Task AddAttribute<TValue>(
            CancellationToken token)
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();
                
                var valid = CacheAttribute.ContainsKey(values.typeName);

                if (valid || !values.cacheable)
                {
                    return;
                }

                var dictionary = new Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>(values.properties.Count());

                CacheAttribute.Add(values.typeName, dictionary);

                foreach (var property in values.properties)
                {
                    SetCacheAttributes(property, values.typeName);
                }
            }

            await CacheFacade.ProcessSemaphore(ActionAdd, token);
        }

        public async Task<bool> HasMethodSet(
            CancellationToken token) =>
            await CacheFacade.ProcessSemaphore(() => CacheSet.Any(), token);

        public async Task<bool> HasMethodGet(
            CancellationToken token) =>
            await CacheFacade.ProcessSemaphore(() => CacheGet.Any(), token);

        public async Task<bool> HasProperty(
            CancellationToken token) =>
            await CacheFacade.ProcessSemaphore(() => CacheProperties.Any(), token);

        public async Task<bool> HasAttribute(
            CancellationToken token) =>
            await CacheFacade.ProcessSemaphore(() => CacheAttribute.Any(), token);

        public void ClearCache()
        {
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

        /// <summary>Gets the values from TValue</summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns></returns>
        private (string typeName, IEnumerable<PropertyInfo> properties, bool cacheable) GetValues<TValue>()
        {
            var typeName = typeof(TValue).Name;

            var cacheable = typeof(TValue).GetCustomAttribute<NoCacheableAttribute>() == null;

            PropertyInfo[] properties = typeof(TValue).
                GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            ThrowErrorIf.
                IsNullOrEmptyList(properties, nameof(properties), string.Empty);

            var propertiesList = ValidateProperty(properties);

            return (typeName, propertiesList, cacheable);
        }

        /// <summary>Validates the property.</summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> ValidateProperty(PropertyInfo[] properties)
        {
            foreach (var property in properties)
            {
                var isCacheable = property.GetCustomAttribute<NoCacheableAttribute>() != null;

                var type = property.PropertyType;

                var isPrimitive = type.IsSubclassOf(typeof(ValueType)) ||
                    type.IsArray ||
                    type.Equals(typeof(string)) ||
                    type.Equals(typeof(StringBuilder)) ||
                    type.Equals(typeof(StringDictionary));
                ;

                if (!isPrimitive ||
                    isCacheable)
                {
                    continue;
                }
                yield return property;
            }
        }
    }
}
