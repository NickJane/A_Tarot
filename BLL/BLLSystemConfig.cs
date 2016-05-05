using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Framework.ModelMate;
using Data.Framework.Model;
using Data.Framework.DataProvider;
using Lib.Utils;
using Lib.ClassExt;
using Lib.BaseClass;
using BLL.Enums;
using MongoDB.Driver.Builders;
using Data.MongoFramework.ModelMate;
using MongoDB.Bson;
using Lib.Data;
using Data.MongoFramework;

namespace BLL
{
    public class BLLSystemConfig
    {
        public static string Key_BadWords = "BadWords";
        public static string Key_NoCatchException = "NoCatchException";
        public static string Key_ExceptionToEmails = "exceptionsto";

        public static string Key_sildershow = "sildershow";

        private static Dictionary<string,string> dic;
        static BLLSystemConfig() {
            Init();
        }
        public static void Init()
        {
            dic = SystemConfigVisitor.GetBaseDbContext().SystemConfig.ToDictionary(x => x.C_Key, y => y.C_Value);
        }
        private static void CheckKey(bool has, string word, string value="") {
            if (!has) {
                SystemConfigVisitor.GetInstance().Insert(new SystemConfig { C_Key = word, C_Value = value });
                Init();
            }
        }
        /// <summary>
        /// 过滤字, 读
        /// </summary>
        public static string BadWords
        { 
            get {
                CheckKey(dic.Keys.Contains(Key_BadWords), Key_BadWords);
                return dic[Key_BadWords]; 
            }

        }
        /// <summary>
        /// 接收错误邮件的邮箱地址
        /// </summary>
        public static string ExceptionToEmails
        {
            get
            {
                CheckKey(dic.Keys.Contains(Key_ExceptionToEmails), Key_ExceptionToEmails);
                return dic[Key_ExceptionToEmails];
            }
        }

        /// <summary>
        /// 不捕获的异常类名,, 读
        /// </summary>
        public static string NoCatchException
        { 
            get {
                CheckKey(dic.Keys.Contains(Key_NoCatchException), Key_NoCatchException);
                return dic[Key_NoCatchException]; 
            }
        }

        /// <summary>
        /// 首页sildershow, 格式为 [0]图片url数组, [1]图片说明数组 [2]图片链接地址数组
        /// </summary>
        public static string[] Sildershow {
            get
            {
                CheckKey(dic.Keys.Contains(Key_sildershow), Key_sildershow, "[]■[]■[]");
                return dic[Key_sildershow].Split('■');
            }
        }
    }
}
