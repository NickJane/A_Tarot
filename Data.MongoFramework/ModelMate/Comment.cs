using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using Data.MongoFramework.ModelMate;
using System.ComponentModel.DataAnnotations;

namespace Data.MongoFramework.ModelMate
{
    /// <summary>
    /// 评论系统实体
    /// </summary>
    public class Comment
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// 冗余字段
        /// </summary>
        public string _idForString { get; set; }
        /// <summary>
        /// 冗余字段
        /// </summary>
        public int _Index { get; set; }
        /// <summary>
        /// 引用的外键id.
        /// </summary>
        public string FKid { get; set; }

        private string _Content;
        /// <summary>
        /// 回帖类型, 见枚举
        /// </summary>
        public int FKType { get; set; }
        [StringLength(500, ErrorMessage = "长度不能大于500")]
        public string Content { get {
            return _Content;
        } set { _Content = value; } }

        /// <summary>
        /// 子评论
        /// </summary>
        public Comment[] Children { get; set; }

        /// <summary>
        /// 帖子状态
        /// </summary>
        public int PostState { get; set; }
        public string IP { get; set; }
        public DateTime PostTime { get; set; }

        public M_User m_User { get; set; }

        /// <summary>
        /// 评论级别. 现在就顶级评论和子级评论
        /// </summary>
        public int CommentLevel { get; set; }
    }
}
