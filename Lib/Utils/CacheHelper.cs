using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;

namespace Lib.Utils
{
    public static class CacheHelper
    {
        // Methods
        public static void Clear()
        {
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
            List<string> list = new List<string>();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Key.ToString());
            }
            foreach (string str in list)
            {
                Remove(str);
            }
        }

        public static bool Exists(string key)
        {
            return (HttpRuntime.Cache[key] != null);
        }

        public static T Get<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        public static void Get<T>(string key, out T t)
        {
            t = (T)HttpRuntime.Cache.Get(key);
        }

        public static T Get<T>(string key, Func<T> f) where T : class
        {
            if (!Exists(key))
            {
                Insert<T>(key, f());
            }
            return (T)HttpRuntime.Cache[key];
        }

        public static T Get<T>(string key, Func<T> f, int minitues)
        {
            if (!Exists(key))
            {
                Insert<T>(key, f(), minitues);
            }
            return (T)HttpRuntime.Cache[key];
        }

        public static void Insert<T>(string key, T t) where T : class
        {
            if (t != null)
            {
                HttpRuntime.Cache.Insert(key, t);
            }
        }

        public static void Insert<T>(string key, T t, int minitues)
        {
            HttpRuntime.Cache.Insert(key, t, null, DateTime.Now.AddMinutes((double)minitues), TimeSpan.Zero);
        }

        public static void Remove(string key)
        {
            if(Exists(key))
            HttpRuntime.Cache.Remove(key);
        }
        /// <summary>
        /// 设置以缓存依赖文件的方式缓存数据
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        /// <param name="cacheDepen">依赖对象System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency("C:\\test.txt");</param>
        public static void SetCache(string CacheKey, object objObject, string url)
        {

            System.Web.Caching.Cache objCache = HttpRuntime.Cache;

            objCache.Insert(
                CacheKey,
                objObject,
                new System.Web.Caching.CacheDependency(url),
                System.Web.Caching.Cache.NoAbsoluteExpiration, //从不过期
                System.Web.Caching.Cache.NoSlidingExpiration, //禁用可调过期
                System.Web.Caching.CacheItemPriority.Default,
                null);

        }

    }


}
