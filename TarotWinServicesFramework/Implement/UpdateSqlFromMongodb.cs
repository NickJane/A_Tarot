using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Data;
using Lib.Utils;
using System.Data;
using System.Data.SqlClient;
using Data.MongoFramework.ModelMate;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
namespace TarotWinServicesFramework.Implement
{
    public class UpdateSqlFromMongodb : IWindowTask
    {
        public void Run(params object[] parms)
        {
            var temp = SqlHelper.ExecuteScalar(ConfigHelper.GetAppSetting("conn"), CommandType.Text, "select nextruntime from mytask where name='UpdateSqlFromMongodb'");
            DateTime starttime;
            if (temp == null || temp == DBNull.Value)//如果没有nextruntime, 则初始化
            {
                MongoDBHelper.DeleteAll("Posts");
                MongoDBHelper.DeleteAll("Comments");

                MongoDBHelper.UpdateAll<CustomerTag>("CustomerTag", Query.Null, Update.Set("PopularityCount", 0));

                SqlHelper.ExecuteNonQuery(ConfigHelper.GetAppSetting("conn"), CommandType.Text, "update CardFormation set Popularity=0");

                starttime = new DateTime(2012, 1, 10);
            }else//获得上次的短日期
            {
                starttime = Convert.ToDateTime(SqlHelper.ExecuteScalar(ConfigHelper.GetAppSetting("conn"), CommandType.Text, "select lastruntime from mytask where name='UpdateSqlFromMongodb'"));
                starttime = new DateTime(starttime.Year, starttime.Month, starttime.Day);
            }

            var endtime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            string s1, s2;
            ToFormation(BsonDateTime.Create(starttime), BsonDateTime.Create(endtime),out s1);
            ToComments(BsonDateTime.Create(starttime), BsonDateTime.Create(endtime), out s2);

            string[] ErrorEmails = Lib.Utils.ConfigHelper.GetAppSetting("ErrorEmails").Split(';');
            foreach (var mail in ErrorEmails)
            {
                if (string.IsNullOrEmpty(mail)) continue;
                Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                mail,
                "异常邮件", s1 + s2, false, null);
            }
        }
        /// <summary>
        ///  update Formation's Popularity in Sql server
        /// </summary>
        public void ToFormation(BsonDateTime startTime, BsonDateTime endtime,out string result)
        {
            result = "";
            var list = MongoDBHelper.GetAll<M_Post>("Posts7Days",
                                                MongoDB.Driver.Builders.Query.And(
                                                    Query.GTE("PostTime", startTime),
                                                    Query.LT("PostTime", endtime)
                                                )
                                                );
            foreach (var item in list)
            {
                if (item.PostType != 1) continue;
                SqlHelper.ExecuteNonQuery(ConfigHelper.GetAppSetting("conn"), CommandType.Text, 
                    "update CardFormation set Popularity=Popularity+1 where CardFormationID=" + item.CardFormationInfo.CardFormationID);
                foreach (var tag in item.CustomerTags)
                {
                    result += string.Format("更新自定义标签{0}.........", tag.TagName);
                    MongoDBHelper.UpdateAll<CustomerTag>("CustomerTag", Query.EQ("TagName", tag.TagName), Update.Inc("PopularityCount", 1));
                }
            }

            result += string.Format("这次共同步帖子{0}个, 其中塔罗贴{1}个.........", list.Count, list.Where(x => x.PostType != 1).Count());


            MongoDBHelper.InsertAll<M_Post>("Posts", list);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="entTime"></param>
        public void ToComments(BsonDateTime startTime, BsonDateTime entTime, out string result)
        {
            var lst = MongoDBHelper.GetAll<Comment>("Comments7Days",
                                                                                MongoDB.Driver.Builders.Query.And(
                                                                                    Query.GTE("PostTime", startTime),
                                                                                    Query.LT("PostTime", entTime)
                                                                                )
                                                                              );
            MongoDBHelper.InsertAll<Comment>("Comments",
                                                lst
                                                );

            result = string.Format("这次共同步评论{0}个", lst.Count);
        }

    }
}
