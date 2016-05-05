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
    public class BLLMessages
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagerinfo"></param>
        /// <param name="from_or_to">true为收件</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IList<Messages> GetMessages(PagerInfo pagerinfo,bool from_or_to, int userid, out int count) {
            var list= MongoDBHelper.GetAll<Messages>(CollectionName.Messages,
                from_or_to ? Query.EQ("ToUser.UserID", userid) : Query.EQ("FromUser.UserID", userid), 
                pagerinfo, 
                SortBy.Descending("PostTime"));
            count = MongoDBHelper.Count(CollectionName.Messages, from_or_to ? Query.EQ("ToUser.UserID", userid) : Query.EQ("FromUser.UserID", userid));
            return list;
        }

        public static void SendMessage(Messages model) {
            CacheHelper.Remove(string.Format(CacheKey.MessageNewCounter, model.ToUser.UserID));

            MongoDBHelper.InsertOne<Messages>(CollectionName.Messages, model);
        }

        public static void ReadMessage(string _id) {
            CacheHelper.Remove(string.Format(CacheKey.MessageNewCounter, UserInfo.UserAccount.UserID));
            MongoDB.Bson.ObjectId id;
            if (MongoDB.Bson.ObjectId.TryParse(_id, out id)) {
                MongoDBHelper.UpdateAll<Messages>(CollectionName.Messages, Query.EQ("_id", id), Update.Set("isRead", BsonBoolean.True));   
            }
        }

        public static int GetNewMessageCount() {
            var counter = CacheHelper.Get<object>(
                    string.Format(CacheKey.MessageNewCounter, UserInfo.UserAccount.UserID),
                    () =>
                    MongoDBHelper.Count(CollectionName.Messages,
                                        Query.And(
                                            Query.EQ("ToUser.UserID", UserInfo.UserAccount.UserID),
                                            Query.EQ("isRead", BsonBoolean.False)
                                        )
                    ),30
                );
            return counter.ToInt(0);
        }
    }
}
