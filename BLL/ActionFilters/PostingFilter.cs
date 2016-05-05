using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using MongoDB.Driver.Builders;
using MongoDB.Bson;
namespace BLL.ActionFilters
{
    /// <summary>
    /// 用于公开的方法过滤.
    /// </summary>
    public class PostingFilter : ActionFilterAttribute
    {
        /*
         过滤器中能获得的值
         * protected HttpSessionStateBase Session;        
         * protected ModelStateDictionary State;        
         * protected ViewDataDictionary ViewData;        
         * protected TempDataDictionary TempData;        
         * protected HttpRequestBase Request;        
         * protected Dictionary<string, string> RouteValues;
         */
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (Lib.Data.MongoDBHelper.Count(Data.MongoFramework.CollectionName.Posts7Days,
                    Query.And(
                        Query.EQ("IP", filterContext.HttpContext.Request.UserHostAddress),
                        Query.GTE("PostTime", BsonDateTime.Create(DateTime.Now.AddDays(-1))),
                        Query.EQ("m_User.UserID", -1)
                        )
                    ) >= 3)
                {
                    ViewResult posting = new ViewResult();
                    posting.ViewName = filterContext.RouteData.Values["action"].ToString();
                    posting.ViewData["AnonymityError"] = "对不起, 未注册用户在每个自然天内只能发三个帖子";
                    filterContext.Result = posting;
                }
            }
            if (filterContext.HttpContext.Request.Form["CustomerTag"]!=null && filterContext.HttpContext.Request.Form["CustomerTag"].Length > 50)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError("", "标签长度不能大于50");
                filterContext.Controller.ValidateRequest = false;
            }
        }
        
    }
    /// <summary>
    /// 回帖过滤
    /// 非注册用户不能评论
    /// 评论间隔不能少于5秒钟
    /// </summary>
    public class CommonFilter : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var flag = true;
            filterContext.HttpContext.Items["flag"] = flag;
            filterContext.HttpContext.Items["msg"] = "";

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                flag = false;
                filterContext.HttpContext.Items["flag"] = flag;
                filterContext.HttpContext.Items["msg"] = "非注册用户不能评论";
            }
            else {
                if (Lib.Data.MongoDBHelper.Count(Data.MongoFramework.CollectionName.Comments7Days,
                    Query.And(
                        Query.EQ("m_User.UserID", UserInfo.UserAccount.UserID),
                        Query.GT("PostTime", MongoDB.Bson.BsonDateTime.Create(DateTime.Now.AddSeconds(-5)))
                        )
                    ) > 0)
                {
                    flag = false;
                    filterContext.HttpContext.Items["flag"] = flag;
                    filterContext.HttpContext.Items["msg"] = "评论间隔不能少于5秒钟";
                }
            }

        }
    }
}
