using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace UserManager.Framework
{
    internal class UserFactory
    {
        private static Hashtable ht = Hashtable.Synchronized(new Hashtable());

        static string DBType = System.Configuration.ConfigurationManager.AppSettings["DBType"];

        public static IUser GetUserManagerInstance()
        {
            string key = "UserManager.Framework." + DBType + ".DalUserManager";
            if (ht[key] == null)
            {
                var ins = Type.GetType(key).GetConstructor(Type.EmptyTypes).Invoke(null);
                ht[key] = ins;
                return (IUser)ins;
            }
            else
                return (IUser)ht[key];
        }
    }
}
