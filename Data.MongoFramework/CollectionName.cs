using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.MongoFramework
{
    /// <summary>
    /// Mongodb数据库的collection名称定义静态类
    /// </summary>
    public struct CollectionName
    {
        /// <summary>
        /// 帖子
        /// </summary>
        public static string Posts = "Posts";
        /// <summary>
        /// 七天内的帖子. 需要通过ssis同步
        /// </summary>
        public static string Posts7Days = "Posts7Days";

        public static string Comments = "Comments";

        public static string Comments7Days = "Comments7Days"; 

        /// <summary>
        /// 自定义标签
        /// </summary>
        public static string CustomerTag = "CustomerTag";

        /// <summary>
        /// 操作以及系统日志记录
        /// </summary>
        public static string LogOperate = "LogOperate";

        /// <summary>
        /// 本站动态. 
        /// </summary>
        public static string News = "News";

        /// <summary>
        /// 短消息
        /// </summary>
        public static string Messages = "Messages";
    }
}
