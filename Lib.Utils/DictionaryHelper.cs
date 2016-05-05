using System.Collections.Generic;

namespace Lib.Utils
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    public static class DictionaryHelper
    {
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, T> dict, string key)
        {
            return GetValue(dict, key, default(T));
        }

        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Dictionary<string, T> dict, string key, T defaultValue)
        {
            return dict == null ? default(T) : dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 添加键值对数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, T> AddValue<T>(this Dictionary<string, T> dict, string key, T value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 添加键值对数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dict"></param>
        /// <param name="keys"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Dictionary<string, T> AddValues<T>(this Dictionary<string, T> dict, string[] keys, T[] values)
        {
            var i = 0;
            foreach (var key in keys)
            {
                dict.AddValue(key, i >= values.Length ? default(T) : values[i]);
                i++;
            }
            return dict;
        }
    }
}