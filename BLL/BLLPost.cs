using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Framework.ModelMate;
using Data.Framework.Model;

using Lib.Utils;
using Lib.ClassExt;
using Lib.BaseClass;
using BLL.Enums;
using MongoDB.Driver.Builders;
using Data.MongoFramework.ModelMate;
using MongoDB.Bson;
using Lib.Data;
using Data.MongoFramework;

namespace BLL
{
    /// <summary>
    /// 发帖, 回复
    /// </summary>
    public class BLLPost
    {
        #region 操作

        public static void AddPostReplyCount(string id, string collectionName="")
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (BsonObjectId.TryParse(id, out _id))
            {
                if (collectionName == "")
                {
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts7Days, Query.EQ("_id", _id),
                        Update.Inc("ReplyCount", 1).Set("LastModifyTime", DateTime.Now).Inc("AllTakePartIn", 1));
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts, Query.EQ("_id", _id),
                        Update.Inc("ReplyCount", 1).Set("LastModifyTime", DateTime.Now).Inc("AllTakePartIn", 1));
                }else
                    MongoDBHelper.UpdateAll<M_Post>(collectionName, Query.EQ("_id", _id),
                    Update.Inc("ReplyCount", 1).Set("LastModifyTime", DateTime.Now).Inc("AllTakePartIn", 1));
            }
        }
        /// <summary>
        /// 顶贴或者踩贴
        /// </summary>
        /// <param name="id">帖子ID</param>
        /// <param name="actiontype">mark , down</param>
        /// <param name="content"></param>
        public static void UpPost(string id,string actiontype,System.Web.HttpContextBase content) { 
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (BsonObjectId.TryParse(id, out _id))
            {                    
                var user=new M_User();
                user.UserID=content.User.Identity.IsAuthenticated?BLL.UserInfo.UserAccount.UserID:-1;
                var flag = true;
                if (MongoDBHelper.Exists(CollectionName.Comments7Days,
                                            Query.And(
                                                        Query.EQ("FKid", _id.ToString()),
                                                        Query.EQ("IP", content.Request.UserHostAddress),
                                                        Query.EQ("m_User.UserID", user.UserID),
                                                        Query.EQ("FKType", actiontype == "mark" ? 3 : 4)
                                                        )
                                        )
                    ) 
                    flag = false;

                if (actiontype == "mark" && flag)
                { //标记.  mark+1.  回帖表加一条记录 posttype=mark. 总参与数加一
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts7Days, Query.EQ("_id", _id),
                        Update.Inc("Mark", 1).Set("LastModifyTime", DateTime.Now).Inc("AllTakePartIn", 1));
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts, Query.EQ("_id", _id),
                        Update.Inc("Mark", 1).Set("LastModifyTime", DateTime.Now).Inc("AllTakePartIn", 1));


                    Comment model = new Comment { FKid=_id.ToString(), FKType=(int)Comment_FKType.Mark, PostState=1,
                                                  PostTime = DateTime.Now,
                                                  IP = content.Request.UserHostAddress,
                                                  m_User = user 
                    };
                    
                    MongoDBHelper.InsertOne<Comment>(CollectionName.Comments7Days, model);

                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionName">collectionName为空则是塔罗贴或者普通帖. 不为空则为新闻贴或者其他扩展帖子</param>
        public static void AddPostPV(string id, string collectionName="")
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); 
            if (BsonObjectId.TryParse(id, out _id))
            {
                if (collectionName != "") {
                    MongoDBHelper.UpdateAll<M_Post>(collectionName, Query.EQ("_id", _id), Update.Inc("PopularityCount", 1));
                }
                else
                {
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts7Days, Query.EQ("_id", _id), Update.Inc("PopularityCount", 1));
                    MongoDBHelper.UpdateAll<M_Post>(CollectionName.Posts, Query.EQ("_id", _id), Update.Inc("PopularityCount", 1));
                }
            }
        }
        /// <summary>
        /// 更新状态. 主要为删帖服务.
        /// </summary>
        /// <typeparam name="T">更新那个类</typeparam>
        /// <typeparam name="T2">更新的值的bson类型</typeparam>
        /// <param name="id">objectid</param>
        /// <param name="collectionsName">表名</param>
        /// <param name="field">字段名</param>
        /// <param name="value">更新值</param>
        public static void UpdateState<T, T2>(string id, string collectionsName, string field, T2 value, string idField = "_id") where T2 : BsonValue
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (BsonObjectId.TryParse(id, out _id))
            {
                MongoDBHelper.UpdateAll<T>(collectionsName, Query.EQ(idField, _id), Update.Set(field, value));
            }
        }
        /// <summary>
        /// 更新数组内的对象状态. 主要为删帖服务.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="id"></param>
        /// <param name="collectionsName"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="ArrayName"></param>
        /// <param name="idField"></param>
        public static void UpdateArrayState<T, T2>(string id, string collectionsName, string field, T2 value, string ArrayName, string idField = "_id") where T2 : BsonValue
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (BsonObjectId.TryParse(id, out _id))
            {
                MongoDBHelper.UpdateAll<T>(collectionsName, Query.ElemMatch(ArrayName, Query.EQ(idField, _id)), Update.Pull(ArrayName, Query.EQ(idField, _id)));
            }
        }

        #endregion

        #region 读取
        /// <summary>
        /// 获得帖子. 塔罗贴和普通帖
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static M_Post GetM_PostByID(string id)
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (!BsonObjectId.TryParse(id, out _id)) return null;
            var model = MongoDBHelper.GetOne<M_Post>(CollectionName.Posts7Days, Query.And(Query.EQ("_id", _id)/*, Query.EQ("PostState", 1)*/));
            if (model == null)
                MongoDBHelper.GetOne<M_Post>(CollectionName.Posts, Query.And(Query.EQ("_id", _id)/*, Query.EQ("PostState", 1)*/));
            return model;
        }
        /// <summary>
        /// 获得新闻贴和其他扩展帖子
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public static M_Post GetM_PostByID(string id, string collectionName)
        {
            BsonObjectId _id = ObjectId.GenerateNewId(); ;
            if (!BsonObjectId.TryParse(id, out _id)) return null;
            var model = MongoDBHelper.GetOne<M_Post>(collectionName, Query.And(Query.EQ("_id", _id)/*, Query.EQ("PostState", 1)*/));
            return model;
        }

        /// <summary>
        /// 评论. 外键ID, 评论级别为1的数据. 由于要显示被删除的帖子. 则取消PostState状态
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="pagerinfo"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList<Comment> GetCommentListByID(string _id, PagerInfo pagerinfo, out int count)
        {

            var model = MongoDBHelper.GetAll<Comment>(CollectionName.Comments7Days,
                                    Query.And(
                                        Query.EQ("FKid", _id), /*Query.EQ("PostState", 1),*/ 
                                        Query.EQ("CommentLevel", 1),
                                        Query.NE("FKType",(int)Comment_FKType.Mark),
                                        Query.NE("FKType",(int)Comment_FKType.Down)
                                        ), pagerinfo, SortBy.Ascending("PostTime"));

            count = MongoDBHelper.Count(CollectionName.Comments7Days,
                                    Query.And(
                                        Query.EQ("FKid", _id),/*Query.EQ("PostState", 1),*/
                                        Query.EQ("CommentLevel", 1),
                                        Query.NE("FKType", (int)Comment_FKType.Mark),
                                        Query.NE("FKType", (int)Comment_FKType.Down)
                                        ));
            if (model == null || model.Count == 0)
            {
                model = MongoDBHelper.GetAll<Comment>(CollectionName.Comments,
                                    Query.And(
                                        Query.EQ("FKid", _id), /*Query.EQ("PostState", 1),*/
                                            Query.EQ("CommentLevel", 1),
                                            Query.NE("FKType", (int)Comment_FKType.Mark),
                                            Query.NE("FKType", (int)Comment_FKType.Down)
                                    ), pagerinfo, SortBy.Ascending("PostTime"));
                count = MongoDBHelper.Count(CollectionName.Comments7Days, 
                                    Query.And(
                                        Query.EQ("FKid", _id), /*Query.EQ("PostState", 1),*/
                                        Query.EQ("CommentLevel", 1),
                                        Query.NE("FKType", (int)Comment_FKType.Mark),
                                        Query.NE("FKType", (int)Comment_FKType.Down)
                                    ));
            }
            int i = 1;
            foreach (var item in model)
            {
                item._Index = (pagerinfo.Page - 1) * pagerinfo.PageSize + i; i++;
            }
            return model;
        }

        public static Comment GetCommentByQuery(MongoDB.Driver.IMongoQuery q)
        {
            var model = MongoDBHelper.GetOne<Comment>(CollectionName.Comments7Days, q);
            if (model == null)
                MongoDBHelper.GetOne<Comment>(CollectionName.Comments, q);
            return model;
        }
        #endregion

        #region 首页三大列表
        public static IList<M_Post> GetBBSList(PagerInfo pagerinfo, out int count)
        {
            var model = MongoDBHelper.GetAll<M_Post>(
                                    CollectionName.Posts7Days,
                                    Query.And(
                                        Query.EQ("PostType", (int)BLL.Enums.Mpost_PostType.Usual),
                                        Query.EQ("PostState", 1)
                                        ),
                                    pagerinfo, SortBy.Descending("LastModifyTime"), new string[] { "Title", "Content", "_id", "ReplyCount", "PopularityCount", "m_User", "LastModifyTime" });

            count = MongoDBHelper.Count(
                                            CollectionName.Posts7Days,
                                            Query.And(
                                                Query.EQ("PostType", (int)BLL.Enums.Mpost_PostType.Usual),
                                                Query.EQ("PostState", 1)
                                            )
                                        );
            return model;
        }
        public static IList<M_Post> GetNewPostList(PagerInfo pagerinfo, out int count, string orderby = "PostTime")
        {
            var model = MongoDBHelper.GetAll<M_Post>(
                                    CollectionName.Posts7Days,
                                    Query.And(
                                        Query.EQ("PostType", (int)BLL.Enums.Mpost_PostType.Tarot),
                                        Query.EQ("PostState", 1)
                                        ),
                                    pagerinfo, SortBy.Descending(orderby),
                                    new string[] { "Title", "Content", "_id", "ReplyCount", "PopularityCount", "m_User", "LastModifyTime","PostTime",
                                        "CustomerTags","CardFormationInfo.CardFormationUrl","CardFormationInfo.CardFormationName","Mark","PostState" });

            count = MongoDBHelper.Count(
                                            CollectionName.Posts7Days,
                                            Query.And(
                                                Query.EQ("PostType", (int)BLL.Enums.Mpost_PostType.Tarot),
                                                Query.EQ("PostState", 1)
                                            )
                                        );


            return model;
        }
        #endregion

        #region 本站动态
        public static IList<M_Post> GetNewsList(PagerInfo pagerinfo, out int count,BLL.Enums.Mpost_PostType PostType, string orderby = "PostTime")
        {
            var model = MongoDBHelper.GetAll<M_Post>(
                                    CollectionName.News,
                                    Query.And(
                                        Query.EQ("PostType", (int)PostType)
                                        ,
                                        Query.EQ("PostState", 1)
                                        )
                                        ,
                                    pagerinfo, SortBy.Descending(orderby),
                                    new string[] { "Title", "Content", "_id", "ReplyCount", "PopularityCount", "m_User", "LastModifyTime","PostType" });

            count = MongoDBHelper.Count(
                                            CollectionName.News,
                                            Query.Null
                                        );


            return model;
        }
        #endregion


    }
}
