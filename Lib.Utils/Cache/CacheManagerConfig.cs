using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Utils.Cache
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public static class CacheManagerConfig
    {
        public static string CacheManagerType { get { return ConfigHelper.GetAppSetting("CacheManagerType"); } }
    }
}
