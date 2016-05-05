using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Data.MongoFramework.ModelMate
{
    /// <summary>
    /// 短消息
    /// </summary>
    public class Messages
    {
        public MongoDB.Bson.BsonObjectId _id { get; set; }
        public M_User FromUser { get; set; }
        public M_User ToUser { get; set; }
        [StringLength(30, ErrorMessage = "长度不能大于30")]
        public string Title { get; set; }
        public DateTime PostTime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool isRead { get; set; }
        [StringLength(500, ErrorMessage = "长度不能大于500")]
        public string Content { get; set; }
    }
}
