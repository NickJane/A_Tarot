using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.ClassExt;

namespace Authorizations.Framework.Enums
{
    public enum ApplicationEnum
    {
        [Language("zh-cn", "运行管理系统")]
        Admin = 1,
        [Language("zh-cn", "塔罗来了")]
        Tarot = 2,
    }
    /// <summary>
    /// 系统级别枚举
    /// </summary>
    public enum ResourceEnum { 
        
        System=1,
        SecondLevelModule=2,
        ThirdLevelModule=3,
        Page=4,
        Button=5
    }
}
