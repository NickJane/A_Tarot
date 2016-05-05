using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.ClassExt
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LanguageAttribute : Attribute { 
        public LanguageAttribute(string languageKey, string languageValue) { 
            LanguageKey = languageKey; LanguageValue = languageValue; 
        } 
        public string LanguageKey { get; set; } 
        public string LanguageValue { get; set; } 
    }
    public static class EnumExt
    {
        /// <summary>
        /// 枚举值转换为本地语言
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static string ToLocalLanguage(this Enum _enum) {
            var type = _enum.GetType();
            var cachevalue=Lib.Utils.CacheHelper.Get<string>(type.FullName+"."+_enum.ToString());
            if (cachevalue != null)
                return cachevalue;

            var attr = type.GetField(_enum.ToString()).GetCustomAttributes(false); 
            string ret = ""; 
            foreach (var item in attr) { 
                var lang = ((LanguageAttribute)item); 
                if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == lang.LanguageKey.ToLower()) 
                { 
                    ret = lang.LanguageValue; break;
                } 
            }
            Lib.Utils.CacheHelper.Insert<string>(type.FullName + "." + _enum.ToString(), ret);
            return ret; 
        }
        /// <summary>
        /// 枚举转换为字典, 进而可以转换为selectItems供dropdownlist使用
        /// </summary>
        /// <param name="_enum"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ToDictionary(this Enum _enum) {
            return ToDictionary(_enum, "", "");
        }
        public static Dictionary<string, string> ToDictionary(this Enum _enum, string firstText, string firstValue)
        {
            var dic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(firstText) && !string.IsNullOrEmpty(firstValue))
                dic.Add(firstValue, firstText);
            Type type = _enum.GetType();
            var cachekey = type.FullName + firstText + firstValue;
            var cachevalue = Lib.Utils.CacheHelper.Get<Dictionary<string, string>>(cachekey);
            if (cachevalue != null)
                return cachevalue;

            Array values= Enum.GetValues(type);
            foreach (var val in values)
            {
                int value = (int)val;
                var attrs= type.GetField(val.ToString()).GetCustomAttributes(false);
                foreach (var attr in attrs)
                {
                    var lang = ((LanguageAttribute)attr);
                    if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() == lang.LanguageKey.ToLower())
                    {
                        dic.Add(value.ToString(), lang.LanguageValue);
                        break;
                    } 
                }
            }
            Lib.Utils.CacheHelper.Insert<Dictionary<string, string>>(cachekey, dic);
            return dic;
        }
    }
}
