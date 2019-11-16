using Generic.Repository.Validations.ThrowError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Generic.Repository.Cache
{
    public class CacheRepository : ICacheRepository
    {
        private int Size { get; set; }

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
            string propertyKey)
        {
            var dicResult = await CacheFacade.
            GetData(CacheGet, objectKey);

            var result = await CacheFacade.
            GetData(dicResult, propertyKey);

            return result;
        }

        public async Task<IDictionary<string, Func<object, object>>> GetDictionaryMethodGet(
            string objectKey)
        {
            var dictionary = await CacheFacade.
            GetData(CacheGet, objectKey);
            return dictionary;
        }

        public async Task<Action<object, object>> GetMethodSet(
            string objectKey,
            string propertyKey)
        {
            var dicResult = await CacheFacade.
            GetData(CacheSet, objectKey);

            var result = await CacheFacade.
            GetData(dicResult, propertyKey);
            return result;
        }

        public async Task<IDictionary<string, Action<object, object>>> GetDictionaryMethodSet(string objectKey)
        {
            var dictionary = await CacheFacade.GetData(CacheSet, objectKey);
            return dictionary;
        }

        public async Task<CustomAttributeTypedArgument> GetAttribute(
            string objectKey,
            string propertyKey,
            string customAttributeKey)
        {
            var dictionaryI = await CacheFacade
            .GetData(
                CacheAttribute,
                objectKey);

            var dictionaryII = await CacheFacade
            .GetData(
                dictionaryI,
                propertyKey);

            var result = await CacheFacade
            .GetData(
                dictionaryII,
                customAttributeKey);

            return result;
        }

        public async Task<IDictionary<string, CustomAttributeTypedArgument>> GetDictionaryAttribute(string objectKey, string propertyKey)
        {
            var dictionaryI = await CacheFacade.GetData(CacheAttribute, objectKey);
            var result = await CacheFacade.GetData(dictionaryI, propertyKey);
            return result;
        }

        public async Task<IDictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> GetDictionaryAttribute(string objectKey)
        {
            var result = await CacheFacade.GetData(CacheAttribute, objectKey);
            return result;
        }

        public async Task<PropertyInfo> GetProperty(string objectKey, string propertyKey)
        {
            var dictionary = await CacheFacade.
                GetData(CacheProperties, objectKey);

            var result = await CacheFacade.
                GetData(dictionary, propertyKey);

            return result;
        }

        public async Task<IDictionary<string, PropertyInfo>> GetDictionaryProperties(string objectKey)
        {
            var result = await CacheFacade.
                GetData(CacheProperties, objectKey);

            return result;
        }

        public async Task AddGet<TValue>()
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();

                if (CacheGet.ContainsKey(values.typeName))
                {
                    return;
                }

                var dictionary = values.properties.
                       ToDictionary(g => g.Name, m => CacheFacade.CreateFunction<TValue>(m));
                CacheGet.Add(values.typeName, dictionary);
            }

            await CacheFacade.ProcessSemaphore(ActionAdd);
        }

        public async Task AddSet<TValue>()
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();
                if (CacheSet.ContainsKey(values.typeName))
                {
                    return;
                }
                var dictionary = values.properties.
                                        ToDictionary(s => s.Name, m => CacheFacade.CreateAction<TValue>(m));
                CacheSet.Add(values.typeName, dictionary);
            }

            await CacheFacade.ProcessSemaphore(ActionAdd);
        }

        public async Task AddProperty<TValue>()
        {
            void ActionAdd()
            {
                var values = GetValues<TValue>();

                if (CacheProperties.ContainsKey(values.typeName))
                {
                    return;
                }
                CacheProperties.Add(values.typeName, values.properties.ToDictionary(p => p.Name, p => p));
            }

            await CacheFacade.ProcessSemaphore(ActionAdd);
        }

        public async Task AddAttribute<TValue>()
        {
            void ActionAdd()
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

            await CacheFacade.ProcessSemaphore(ActionAdd);
        }

        public bool HasMethodSet() => CacheSet.Any();

        public bool HasMethodGet() => CacheGet.Any();

        public bool HasProperty() => CacheProperties.Any();

        public bool HasAttribute() => CacheAttribute.Any();

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

        private (string typeName, PropertyInfo[] properties) GetValues<TValue>()
        {
            var typeName = typeof(TValue).Name;

            PropertyInfo[] properties = typeof(TValue).
                GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            ThrowErrorIf.
                IsNullOrEmptyList(properties, nameof(properties), string.Empty);

            return (typeName, properties);
        }
    }
}
