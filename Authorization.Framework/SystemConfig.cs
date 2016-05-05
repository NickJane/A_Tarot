using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authorizations.Framework
{
    internal class SystemConfig
    {
        /// <summary>
        /// 当前权限系统属于哪个应用程序
        /// </summary>
        public static int CurrentSystemAuthApplication
        {
            get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentApplicationID"]); }
        }
        public static bool UseCache { get { return false; } }
    }
}
