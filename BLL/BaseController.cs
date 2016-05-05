using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using BLL;
using Lib.ClassExt;
using Lib.Utils;
using Lib.BaseClass;
using UserManager.Framework.ModelMate;
using System.Web.Mvc;
using System.Web;

namespace BLL
{
    public class BaseController: System.Web.Mvc.Controller
    {
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            if (User.Identity.IsAuthenticated)
            {
                FormsIdentity identity = User.Identity as FormsIdentity;
                FormsAuthenticationTicket t = identity.Ticket;
                DateTime t2=Request.Cookies["lastlogintime"]==null?DateTime.Now:Convert.ToDateTime(Request.Cookies["lastlogintime"].Value);
                //如果是第一次登陆则记录时间, 并且5个小时刷新一次登陆时间
                if (Request.Cookies["lastlogintime"] == null || (DateTime.Now - t2).TotalHours >= 5)
                {
                    UserManager.Framework.UserVisitor.UpdateState(UserAccount.UserID.ToString(), "lastlogintime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Response.Cookies.Add(new HttpCookie("lastlogintime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }
    

        /// <summary>
        /// 返回json Json(new { Result = Result(1成功0失败), Message = Message(如果为空则由前端自由发挥) }
        /// </summary>
        /// <param name="Result">1成功0失败</param>
        /// <returns></returns>
        protected ActionResult ReturnResult(int Result, string Message) {
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
        
        protected void RedirectToLoginPage() {
            Response.Write("<script>window.parent.location='../../account/login'; </script>");
        }

        protected void JSAlert_CloseIframe_ReloadGrid(string Message) {
            Response.Write("<script>alert('" + Message + "');window.parent.Reload(); $(\".l-dialog,.l-window-mask\", window.parent.document.body).remove();</script>");
            
        }
        protected void JSAlert_ReloadPage(string Message, string url) {
            Response.Write("<script>alert('" + Message + "');location.href='" + url + "';</script>");
        }
        /// <summary>
        /// Response.Write("<script>" + javascript + "</script>"); 全自定义例如<script>alert('" + Message + "');location.href='" + url + "';</script>
        /// </summary>
        /// <param name="javascript"></param>
        protected void JSCustom(string javascript)
        {
            Response.Write("<script>" + javascript + "</script>");
        }
        #region 快捷访问用户信息
        public string[] CurrentUserResources {
            get { return BLL.UserInfo.CurrentUserResources; }
        }
        /// <summary>
        /// 用户资料信息
        /// </summary>
        public Auth_UserAccountMate UserAccount
        {
            get {return BLL.UserInfo.UserAccount;}
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsSuper
        {
            get
            {
                if (BLL.UserInfo.IsSuper)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        public void FillPageParameters(ref Lib.BaseClass.PageParameter t) 
        {
            t.PCurrentPageIndex = Request["page"].ToInt();
            t.PIsAsc = Request["sortorder"].ToLower() == "asc" ? true : false;
            t.POrderField = Request["sortname"];
            t.PPageSize = Request["pagesize"].ToInt();

        }

        public int CurrentApplicationID { get { return int.Parse(System.Configuration.ConfigurationManager.AppSettings["CurrentApplicationID"]); } }
    }
}
