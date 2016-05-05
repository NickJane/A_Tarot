using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace TarotWinServicesFramework
{
    public class TaskFactory
    {
        static Hashtable ht = Hashtable.Synchronized(new Hashtable());

        public static IWindowTask GetTask(string classPath, IList parms) {
            if (ht[classPath] != null)
                return (IWindowTask)ht[classPath];
            else {
                //var taskInstance = Activator.CreateInstance(Type.GetType(classPath));
                //var taskInstance = System.Reflection.Assembly.Load("MyWindowServiceFramwork").CreateInstance(classPath);
                object taskInstance=null;
                    taskInstance = Type.GetType(classPath).GetConstructor(Type.EmptyTypes).Invoke(null);
                ht[classPath] = taskInstance;
                return (IWindowTask)taskInstance;
            }
        }
        public static IWindowTask GetTask(string classPath) {
            return GetTask(classPath, null);
        }
    }
}
