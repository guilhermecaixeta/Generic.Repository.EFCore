using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Generic.Service.Extensions.Commom
{
public static class Commom
    {
        internal static int totalTypesInAssemblyModel { get; set; }
        public static Dictionary<string, Dictionary<string, Func<object, object>>> CacheGet { get; private set; }
        public static Dictionary<string, Dictionary<string, Action<object, object>>> CacheSet { get; private set; }
        public static Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>> CacheAttribute { get; private set; }
        public static Dictionary<string, Dictionary<string, PropertyInfo>> CacheProperties { get; private set; }
        public static Dictionary<Type, MethodInfo> CacheMethod { get; } = new Dictionary<Type, MethodInfo>();

        /// <summary>
        /// Set space on dictionary to improve perfomacing
        /// </summary>
        /// <param name="AssemblyName">Assembly name of project which models alread exist</param>
        /// <param name="Namespace">Namespace name of models alread exist</param>
        public static void SetSizeByLengthProperties(string AssemblyName, string Namespace)
        {
            totalTypesInAssemblyModel = Assembly.Load(AssemblyName).GetTypes().Where(x => Namespace.Split(";").Contains(x.Namespace)).Count();
            CacheProperties = new Dictionary<string, Dictionary<string, PropertyInfo>>(totalTypesInAssemblyModel);
            CacheGet = new Dictionary<string, Dictionary<string, Func<object, object>>>(totalTypesInAssemblyModel);
            CacheSet = new Dictionary<string, Dictionary<string, Action<object, object>>>(totalTypesInAssemblyModel);
            CacheAttribute = new Dictionary<string, Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>>(totalTypesInAssemblyModel);
        }

        public static void SaveOnCacheIfNonExists<TValue>(bool saveAttribute = false, bool saveGet = true, bool saveSet = true, bool saveProperties = true)
        where TValue : class
        {
            string typeName = typeof(TValue).Name;
            PropertyInfo[] properties = null;

            if (!CacheGet.ContainsKey(typeName) || !CacheSet.ContainsKey(typeName) || !CacheProperties.ContainsKey(typeName))
            {
                properties = typeof(TValue).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                int totalProperties = properties.Length;

                if (saveGet && !CacheGet.ContainsKey(typeName))
                    CacheGet.Add(typeName, properties.ToDictionary(g => g.Name, m => CreateGetter<TValue>(m)));
                if (saveSet && !CacheSet.ContainsKey(typeName))
                    CacheSet.Add(typeName, properties.ToDictionary(s => s.Name, m => CreateSetter<TValue>(m)));
                if (saveProperties && !CacheProperties.ContainsKey(typeName))
                    CacheProperties.Add(typeName, properties.ToDictionary(p => p.Name, p => p));
                if (saveAttribute && !CacheAttribute.ContainsKey(typeName))
                {
                    CacheAttribute.Add(typeName, new Dictionary<string, Dictionary<string, CustomAttributeTypedArgument>>(totalProperties));
                    foreach (var property in properties)
                        SaveOnCacheAttrIfNonExist(property, typeName, totalProperties);
                }
            }
        }

        private static void SaveOnCacheAttrIfNonExist(PropertyInfo propertyInfo, string typeName, int totalProperties)
        {
            string propetyName = propertyInfo.Name;
            CacheAttribute[typeName].Add(propetyName, propertyInfo.GetCustomAttributesData().SelectMany(x => x.NamedArguments).ToDictionary(x => x.MemberName, x => x.TypedValue));
        }

        private static Func<object, object> CreateGetter<TValue>(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException($"{property.Name}");

            var getter = property.GetGetMethod(true);
            if (getter == null)
                throw new ArgumentException($"ERROR> ClassName: {nameof(CreateGetter)} {Environment.NewLine}Message: The property {property.Name} does not have a public accessor.");

            MethodInfo genericMethod = typeof(Commom).GetMethod("CreateGetterGeneric", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo genericHelper = genericMethod.MakeGenericMethod(typeof(TValue), property.PropertyType);
            return (Func<object, object>)genericHelper.Invoke(null, new object[] { getter });
        }

        private static Func<object, object> CreateGetterGeneric<T, R>(MethodInfo getter) where T : class
        {
            Func<T, R> getterTypedDelegate = (Func<T, R>)Delegate.CreateDelegate(typeof(Func<T, R>), getter);
            Func<object, object> getterDelegate = (Func<object, object>)((object instance) => getterTypedDelegate((T)instance));
            return getterDelegate;
        }

        private static Action<object, object> CreateSetter<TValue>(PropertyInfo property)
        {
            if (property == null)
                throw new ArgumentNullException($"{property.Name}");

            var setter = property.GetSetMethod(true);
            if (setter == null)
                throw new ArgumentException($"ERROR> ClassName: {nameof(CreateSetter)} {Environment.NewLine}Message:The property {property.Name} does not have a public setter.");

            var genericMethod = typeof(Commom).GetMethod("CreateSetterGeneric", BindingFlags.NonPublic | BindingFlags.Static);

            MethodInfo genericHelper = genericMethod.MakeGenericMethod(typeof(TValue), property.PropertyType);
            return (Action<object, object>)genericHelper.Invoke(null, new object[] { setter });
        }

        private static Action<object, object> CreateSetterGeneric<T, V>(MethodInfo setter) where T : class
        {
            Action<T, V> setterTypedDelegate = (Action<T, V>)Delegate.CreateDelegate(typeof(Action<T, V>), setter);
            Action<object, object> setterDelegate = (Action<object, object>)((object instance, object value) => { setterTypedDelegate((T)instance, (V)value); });
            return setterDelegate;
        }
    }
}