using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Enums;
using Lib.ClassExt;
using Lib.Utils;
using Data.MongoFramework.ModelMate;
using Data.MongoFramework;
using System.Web.Mvc;
namespace BLL
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class BLLLogcs
    {
        /// <summary>
        /// 人为操作记录
        /// </summary>
        /// <param name="strComment"></param>
        /// <param name="strOperateObjectId"></param>
        /// <param name="Context"></param>
        public static void AddAppLog(string strComment, string strOperateObjectId, ControllerContext Context)
        {
            AddAppLog(LogType.OperateLog,
                Context.HttpContext.User.Identity.Name,
                Context.RouteData.Values.Values.First().ToString(),
                Context.RouteData.Values.Values.Last().ToString(),
                strComment,
                Context.HttpContext.Request.UserHostAddress,
                strOperateObjectId,
                UserInfo.UserAccount.UserID);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="appLogType"></param>
        /// <param name="strUserName"></param>
        /// <param name="strModuleName"></param>
        /// <param name="strPageName"></param>
        /// <param name="strComment"></param>
        /// <param name="ip"></param>
        /// <param name="strOperateObjectId">本次被操作对象的ID. 比如评论id, 帖子ID. 用户ID, 牌阵ID等等</param>
        /// <param name="userid">如果为null则是系统日志, 其他为人为操作日志</param>
        public static void AddAppLog(LogType appLogType, string strUserName, string strModuleName, string strPageName, string strComment,string ip,
            string strOperateObjectId, int? userid)
        {
            if (strComment.Length > 300)
            {
                strComment = strComment.Substring(0, 300);
            }
            if (strOperateObjectId.Length > 50)
            {
                strOperateObjectId = strOperateObjectId.Substring(0, 50);
            }
            var ApplicationId = Lib.Utils.ConfigHelper.GetAppSetting("CurrentApplicationID").ToInt();
            var log = new LogOperate()
            {
                LogType = (int)appLogType,
                UserName = strUserName,
                ModuleName = strModuleName,
                Comment = strComment,
                CreateTime = DateTime.Now,
                PageName = strPageName,
                ApplicationId = ApplicationId,
                OperateObjectId = strOperateObjectId,
                UserId = userid,
                ClientIP = ip,
            };
            
            Lib.Data.MongoDBHelper.InsertOne<LogOperate>(CollectionName.LogOperate, log);
        }
        
    }
}
