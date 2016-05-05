using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authorizations.Framework
{
    internal class AuthFactory
    {
        static System.Collections.Hashtable ht = System.Collections.Hashtable.Synchronized(new System.Collections.Hashtable());
        static string DBType = System.Configuration.ConfigurationManager.AppSettings["DBType"];

        /// <summary>
        /// 获得接口实例
        /// </summary>
        /// <typeparam name="T">接口</typeparam>
        /// <param name="className">实现该接口的类名</param>
        /// <returns></returns>
        public static T GetInstance<T>(string className)
        {
            string key = "Authorizations.Framework." + DBType + "." + className;
            if (ht[key] == null)
            {
                var ins = Type.GetType(key).GetConstructor(Type.EmptyTypes).Invoke(null);
                ht[key] = ins;
                return (T)ins;
            }
            else
                return (T)ht[key];
        }

        /*
        public static IResource GetResourceInstance() {
            string key = "Authorization.Framework." + DBType + ".DalResource";
            if (ht[key] == null) {
                var ins = Type.GetType(key).GetConstructor(Type.EmptyTypes).Invoke(null);
                ht[key] = ins;
                return (IResource)ins;
            }
            else
                return (IResource)ht[key];
        }
        public static IRole GetRoleInstance()
        {
            string key = "Authorization.Framework." + DBType + ".DalRole";
            if (ht[key] == null)
            {
                var ins = Type.GetType(key).GetConstructor(Type.EmptyTypes).Invoke(null);
                ht[key] = ins;
                return (IRole)ins;
            }
            else
                return (IRole)ht[key];
        }*/
    }
}
