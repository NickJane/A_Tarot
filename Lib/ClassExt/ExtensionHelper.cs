using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using Lib.Utils;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Lib.ClassExt
{
    public static class ExtensionHelper
    {
        #region 转成基本的类型
        /// <summary>
        /// 转成基本的类型（支持可空类型）
        /// </summary>
        public static T Convert<T>(this string str, T defaultValue)
        {
            if (String.IsNullOrEmpty(str))
                return defaultValue;

            var type = typeof(T);


            var typeName = type.FullName;

            if (type.Name == "Nullable`1")
            {
                var m = Regex.Match(typeName, @"((?<=\[)(\w+.\w+)(?=,))");

                type = Type.GetType(m.Value);
            }


            MethodInfo method = null;
            foreach (var m in type.GetMethods())
            {
                if (m.Name == "Parse" && m.GetParameters().Length == 1)
                {
                    method = m;
                    break;
                }
            }

            var result = defaultValue;
            try
            {
                if (method != null)
                    result = (T)method.Invoke(null, new[] { str });
            }
            catch
            {
                return defaultValue;
            }

            return result;
        }
        #endregion

        #region 字符串转Guid对象
        /// <summary>
        /// 转换成Guid
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string str)
        {
            try
            {
                return new Guid(str);
            }
            catch
            {
                return Guid.Empty;
            }
        }
        #endregion

        #region 转数字字符串
        /// <summary>
        /// 转成数字字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToNumber(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return "";

            return Regex.Replace(str, @"\D+", "");
        }
        #endregion

        #region 转成整数
        /// <summary>
        /// 转成整数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        /// 
        public static int ToInt(this string str, int defaultValue)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ToInt(this object str, int defaultValue)
        {
            try
            {
                return ConvertHelper.GetInteger(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int ToInt(this string str)
        {
            try
            {
                return int.Parse(str);
            }
            catch
            {
                return -1;
            }
        }

        public static int? ToIntOrNull(this string str)
        {
            if (str == string.Empty)
                return null;
            return ToInt(str);
        }
        #endregion

        #region 将对象变量转成短整型变量的方法

        /// <summary>
        /// 将对象变量转成短整型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>短整型变量</returns>
        public static short ToShortInt(this object obj)
        {
            return ConvertHelper.GetShortInt(ConvertHelper.GetString(obj));
        }
        #endregion

        #region 将对象变量转成64位整数型变量的方法
        /// <summary>
        /// 将对象变量转成64位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>64位整数型变量</returns>
        public static long ToLong(this object obj)
        {
            return ConvertHelper.GetLong(ConvertHelper.GetString(obj));
        }
        #endregion

        #region 将对象变量转成双精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>双精度浮点型变量</returns>
        public static double ToDouble(this object obj)
        {
            return ConvertHelper.GetDouble(ConvertHelper.GetString(obj));
        }
        #endregion

        #region 转成浮点类型
        /// <summary>
        /// 转成浮点类型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = Regex.Replace(str, "[^0-9^-^.]", string.Empty);
                if (str != "")
                    str = str.GetLeft(1) + str.Substring(1).Replace('-', '\0');

                var dotIndex = str.IndexOf('.');
                if (dotIndex == -1)
                    return float.Parse(str);
                str = str.Substring(0, dotIndex) + "." + str.Substring(dotIndex + 1).Replace('.', '\0');

                int result;
                int.TryParse(str, out result);
                return result;
            }
            return -1;
        }
        #endregion

        #region 将对象变量转成布尔型变量的方法
        /// <summary>
        /// 将对象变量转成布尔型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>布尔型变量</returns>
        public static bool ToBoolean(this object obj)
        {
            return (obj == DBNull.Value || obj == null) ? false :
                ConvertHelper.GetString(obj).ToLower() == "true" ? true : false;
        }
        #endregion

        #region 将对象变量转成十进制数字变量的方法
        /// <summary>
        /// 将对象变量转成十进制数字变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>十进制数字变量</returns>
        public static decimal ToDecimal(this object obj)
        {
            return ConvertHelper.GetDecimal(ConvertHelper.GetString(obj));
        }
        #endregion

        #region 将对象变量转成日期时间型字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期时间型字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <param name="sFormat">时间字符串格式，例：yyyy-MM-dd</param>
        /// <returns>时间型字符串变量</returns>
        public static string ToDateTimeString(this object obj, string sFormat)
        {
            return ConvertHelper.GetDateTime(obj).ToString(sFormat);
        }
        #endregion

        #region 将对象变量转成日期字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期字符串变量</returns>
        public static string ToShortDateString(this object obj)
        {
            return ConvertHelper.GetDateTimeString(obj, "yyyy-MM-dd");
        }
        #endregion

        #region 将对象变量转成日期型变量的方法
        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime ToDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(ConvertHelper.GetString(obj), out result);
            return result;
        }
        #endregion

        #region 获取字符串左侧字符
        /// <summary>
        /// 获取字符串左侧字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLeft(this string str, int length)
        {
            return String.IsNullOrEmpty(str) ? "" : (length >= str.Length ? str : str.Substring(0, length));
        }
        #endregion

        #region 获取字符串右侧字符
        /// <summary>
        /// 获取字符串右侧字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRight(this string str, int length)
        {
            return String.IsNullOrEmpty(str) ? "" : (length >= str.Length ? str : str.Substring(str.Length - length));
        }
        #endregion

        #region Url code
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return HttpUtility.UrlEncode(str);
        }
        /// <summary>
        ///Url解码 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return HttpUtility.UrlDecode(str);
        }
        #endregion

        #region Object To Json
        static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();

        /// <summary>
        /// 将一个对象转换成json格式字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T t)
        {
            return Serializer.Serialize(t);
        }

        /// <summary>
        /// 将json格式字符串转换成强类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json)
        {
            return Serializer.Deserialize<T>(json);
        }
        #endregion

        #region string to md5
        public static string ToMd5(this string str)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }
        #endregion

        public static T DeepClone<T>(this T obj) where T : class
        {
            using (var ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj); ms.Position = 0;
                return (T)bf.Deserialize(ms);
            }
        }
    }
}
