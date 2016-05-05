using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
namespace Data.MongoFramework.ModelMate
{
    /// <summary>
    /// 自定义标签
    /// </summary>
    public class CustomerTag
    {
        public ObjectId _id { get; set; }
        public string TagName { get; set; }
        /// <summary>
        /// 人气
        /// </summary>
        public int PopularityCount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastModifyTime { get; set; }
    }
}
