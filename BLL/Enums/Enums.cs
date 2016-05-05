using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.ClassExt;
namespace BLL.Enums
{
    public enum State {
        
        [LanguageAttribute("zh-cn", "启用")]
        Normal = 1,
        [LanguageAttribute("zh-cn", "停用")]
        Lock = 0,
    }
    /// <summary>
    /// 这张牌是什么牌(大阿卡纳还是小阿卡纳
    /// </summary>
    public enum TarotCard_Type {
        [LanguageAttribute("zh-cn", "大阿卡纳")]
        BigAkana=1,
        [LanguageAttribute("zh-cn", "小阿卡纳")]
        SmallAkana=0
    }
    /// <summary>
    /// 塔罗牌阵需要牌的数量. 78,22,56
    /// </summary>
    public enum TarotF_NeedAllCards {
        [LanguageAttribute("zh-cn", "78张")]
        All=1,
        [LanguageAttribute("zh-cn", "大阿卡纳")]
        Big=2,
        [LanguageAttribute("zh-cn", "小阿卡纳")]
        Small=3
    }
    /// <summary>
    /// 塔罗牌的正逆位
    /// </summary>
    public enum TarotCard_UpDown {
        /// <summary>
        /// 正位
        /// </summary>
        [LanguageAttribute("zh-cn", "正位")]
        Up=1
        ,
        /// <summary>
        /// 逆位
        /// </summary>
        [LanguageAttribute("zh-cn", "逆位")]
        Down=2
    }

    /// <summary>
    /// 帖子类型
    /// </summary>
    public enum Mpost_PostType {
        /// <summary>
        /// 塔罗贴
        /// </summary>
        [LanguageAttribute("zh-cn", "塔罗牌")]
        Tarot=1,
        /// <summary>
        /// 普通帖
        /// </summary>
        [LanguageAttribute("zh-cn", "普通")]
        Usual=2,
        /// <summary>
        /// 塔罗牌新闻
        /// </summary>
        [LanguageAttribute("zh-cn", "塔罗牌新闻")]
        News=3,

        /// <summary>
        /// 塔罗牌文章列表
        /// </summary>
        [LanguageAttribute("zh-cn", "塔罗牌文章列表")]
        ArticleList = 4


    }

    /// <summary>
    /// 帖子状态
    /// </summary>
    public enum Mpost_PostState
    {
        [LanguageAttribute("zh-cn", "正常")]
        Normal = 1,
        [LanguageAttribute("zh-cn", "删除")]
        Lock = 0,
        [LanguageAttribute("zh-cn", "过期只读")]
        OnlyRead=2
    }
    /// <summary>
    /// 回帖从属
    /// </summary>
    public enum Comment_FKType
    {
        /// <summary>
        /// 塔罗回帖
        /// </summary>
        [LanguageAttribute("zh-cn", "塔罗回帖")]
        TarotPost = 1,
        /// <summary>
        /// 普通回帖
        /// </summary>
        [LanguageAttribute("zh-cn", "普通回帖")]
        UsualPost = 2,
        /// <summary>
        /// (顶/标记)贴
        /// </summary>
        [LanguageAttribute("zh-cn", "(顶/标记)贴")]
        Mark = 3,
        /// <summary>
        /// 踩帖
        /// </summary>
        [LanguageAttribute("zh-cn", "踩帖")]
        Down = 4
    }

    /// <summary>
    /// 日志类别
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 后台系统操作日志
        /// </summary>
        [LanguageAttribute("zh-cn", "后台系统操作日志")]
        OperateLog = 1,
        /// <summary>
        /// 系统日志
        /// </summary>
        [LanguageAttribute("zh-cn", "系统日志")]
        SystemRunLog = 2,

        /// <summary>
        /// 网站管理员操作日志
        /// </summary>
        [LanguageAttribute("zh-cn", "网站管理员操作日志")]
        SiteManagerLog = 3,

        /// <summary>
        /// 错误日志
        /// </summary>
        [LanguageAttribute("zh-cn", "错误日志")]
        Error = 4,

    }

    /// <summary>
    /// 用户标识, 和用户标识js对应
    /// </summary>
    public enum UserSpecialID {
        /// <summary>
        /// 塔罗官方
        /// </summary>
        [LanguageAttribute("zh-cn", "塔罗官方")]
        admin=1
    }
}
