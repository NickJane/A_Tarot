using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Framework.DataProvider
{
    public class ResourceProvider
    {
        private static ResourceProvider instance = new ResourceProvider();
        private ResourceProvider() { }
        public static ResourceProvider GetInstance() {
            return instance;
        }


    }
}
