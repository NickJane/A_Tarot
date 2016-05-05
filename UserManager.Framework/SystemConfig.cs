using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UserManager.Framework
{
    internal class SystemConfig
    {
        /// <summary>
        /// 当前权限系统属于哪个应用程序
        /// </summary>
        public static int CurrentSystemUserManagerApplication
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentApplicationID"]); }
        }
        /// <summary>
        /// GetUserAccount_{0}, userid
        /// </summary>
        public static string Cache_UserAccount = "GetUserAccount_{0}";
        /// <summary>
        /// GetUserInfoCore_{0}, userid
        /// </summary>
        public static string Cache_UserInfoCore = "GetUserInfoCore_{0}";
    }
}
