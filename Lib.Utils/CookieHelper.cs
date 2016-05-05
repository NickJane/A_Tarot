using System;
using System.Web;

namespace Lib.Utils
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 根据key获取value
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            return cookie == null ? null : cookie.Value.UrlDecode();
        }

        /// <summary>
        /// 根据key获取value，未获取到值执行赋值函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getValueFunc"></param>
        /// <returns></returns>
        public static string GetValue(string key, Func<string> getValueFunc)
        {
            var val = GetValue(key);
            if (val == null)
            {
                val = getValueFunc();
                var cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie == null)
                {
                    SetValue(key, val);
                    return val;
                }
            }
            return val;
        }

        /// <summary>
        /// 设置键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="getValueFunc"></param>
        public static void SetValue(string key, Func<string> getValueFunc)
        {
            SetValue(key, getValueFunc());
        }

        /// <summary>
        /// 设置键值对
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetValue(string key, object value)
        {
            if (value == null)
            {
                HttpContext.Current.Response.Cookies.Remove(key);
                return;
            }
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Value = value.ToString();
                HttpContext.Current.Response.AppendCookie(cookie);
            }
            else
                HttpContext.Current.Response.AppendCookie(new HttpCookie(key, value.ToString().UrlEncode()));
        }

        /// <summary>
        /// 根据key移除项
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            HttpContext.Current.Request.Cookies.Remove(key);
        }
    }
}