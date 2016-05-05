using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.Web;

namespace Lib.Utils
{
    public class SessionHelper
    {
        // Methods
        public static T GetValue<T>(string key)
        {
            HttpSessionState session = HttpContext.Current.Session;
            if (session == null)
            {
                return default(T);
            }
            object obj2 = session[key];
            if (obj2 == null)
            {
                return default(T);
            }
            return (T)obj2;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
            HttpContext.Current.Session.Abandon();
        }

        public static void SetValue(string key, object val)
        {
            HttpContext.Current.Session.Add(key, val);
        }
    }

 

}
