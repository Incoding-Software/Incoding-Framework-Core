using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Incoding.Core.Extensions
{
    #region << Using >>

    #endregion

    public static class DictionaryExtensions
    {

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            return source.GetOrDefault(key, default(TValue));
        }

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
        {
            TValue res;
            return source.TryGetValue(key, out res) ? res : defaultValue;
        }

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> src, IEnumerable<KeyValuePair<TKey, TValue>> dest)
        {
            // ReSharper disable PossibleMultipleEnumeration
            foreach (var value in dest)
                src.Set(value.Key, value.Value);

            // ReSharper restore PossibleMultipleEnumeration
        }

        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            source.Set(new KeyValuePair<TKey, TValue>(key, value));
        }

        public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, KeyValuePair<TKey, TValue> valuePair)
        {
            if (source.ContainsKey(valuePair.Key))
                source[valuePair.Key] = valuePair.Value;
            else
                source.Add(valuePair.Key, valuePair.Value);
        }

        /// <summary>
        /// Combine Dynamics
        /// </summary>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public static IDictionary<string, object> CombineDynamics(this object object1, object object2)
        {
            var dict1 = ToDictionary(object1);
            var dict2 = ToDictionary(object2);

            var d = new Dictionary<string, object>();
            foreach (var pair in dict1.Concat(dict2))
            {
                if (!d.ContainsKey(pair.Key))
                    d.Add(pair.Key, pair.Value);
            }

            return d;
        }

        private static IDictionary<string, object> ToDictionary(object obj)
        {
            if (obj is IDictionary<string, object>)
                return obj as IDictionary<string, object>;

            Dictionary<string, object> queryString2 = new Dictionary<string, object>();
            Type type = obj.GetType();
            foreach (PropertyInfo propertyInfo in type.IsAnonymous() ? type.GetProperties() : (type.GetProperties(BindingFlags.Instance | BindingFlags.Public)).Where(r => !r.HasAttribute<IgnoreDataMemberAttribute>()).Where(r => r.CanWrite))
                queryString2.Add(propertyInfo.Name, obj.TryGetValue<object>(propertyInfo.Name));
            return queryString2;
        }

    }
}