using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.ClassExt;
namespace UserManager.Framework.Enums
{
    /// <summary>
    /// 用户状态. 对应UserAccount中的state字段
    /// </summary>
    public enum UserState {
        [LanguageAttribute("zh-cn","启用")]
        Normal=1,
        [LanguageAttribute("zh-cn", "停用")]
        Lock=0,
    }
}
