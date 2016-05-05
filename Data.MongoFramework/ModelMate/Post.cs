using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using Data.MongoFramework.ModelMate;

namespace Data.MongoFramework.ModelMate
{
    
    /// <summary>
    /// mongodb 帖子类
    /// </summary>
    public class M_Post {
        //[MongoID]
        public ObjectId _id { get; set; }
        public string _idForString { get; set; }

        private DateTime _PostTime;
        public DateTime PostTime { get { return _PostTime.ToLocalTime(); } set { _PostTime = value; } }
        private DateTime _LastModifyTime;
        public DateTime LastModifyTime { get { return _LastModifyTime.ToLocalTime(); } set { _LastModifyTime = value; } }
        [Required(ErrorMessage="标题是必须的")]
        [StringLength(40,ErrorMessage="标题长度不能大于70")]
        public string Title { get; set; }
        /// <summary>
        /// 帖子内容
        /// </summary>
        [Required(ErrorMessage = "帖子内容是必须的")]
        [StringLength(1000, ErrorMessage = "长度不能大于2000")]
        public string Content { get; set; }

        public int ReplyCount { get; set; }
        /// <summary>
        /// 人气
        /// </summary>
        public int PopularityCount { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public bool AuditState { get; set; }

        /// <summary>
        /// 1塔罗贴, 2 普通帖
        /// </summary>
        public int PostType { get; set; }
        /// <summary>
        /// 帖子状态
        /// </summary>
        public int PostState { get; set; }
        public string IP { get; set; }

        /// <summary>
        /// 用作"顶", 或者"标记"
        /// </summary>
        public int Mark { get; set; }
        public int Down { get; set; }

        /// <summary>
        /// 总参与人数. 回帖+顶+踩的总数
        /// </summary>
        public int AllTakePartIn { get; set; }
        /// <summary>
        /// 自定义标签
        /// </summary>
        public CustomerTag[] CustomerTags { get; set; }

        /// <summary>
        /// 塔罗牌
        /// </summary>
        public CardFormationInfo CardFormationInfo { get; set; }
        /// <summary>
        /// mongodb 用户信息
        /// </summary>
        public M_User m_User { get; set; }
    }
    /// <summary>
    /// mongodb 用户信息
    /// </summary>
    public class M_User {
        public int UserID { get; set; }
        public string NicName { get; set; }
        private string _AvatarURL;
        /// <summary>
        /// 头像
        /// </summary>
        public string AvatarURL
        {
            get
            {
                    return _AvatarURL;
            }
            set { _AvatarURL = value; }
        }
        public int? UserPoint { get; set; }
        public string UserSpecialID { get; set; }
    }

    public class CardFormationInfo {
        public string CardFormationUrl { get; set; }
        public string CardFormationName { get; set; }
        public string CardFormationID { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public CardInfo[] CardInfos { get; set; }
    }
    public class CardInfo {
        public string CardName { get; set; }
        
        public bool IsHandstand { get; set; }

        public string ImgUrl { get; set; }

        public bool IsPointCard { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int SortIndex { get; set; }

        public string Describe { get; set; }

        public int W { get; set; }

        public int H { get; set; }
    }
}
