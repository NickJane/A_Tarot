using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;

namespace Admin3
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            );

        }
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            string ex = Lib.Utils.LogHelper.ErrorLog(Server.GetLastError());

            foreach (var item in BLL.BLLSystemConfig.ExceptionToEmails.Split(';'))
            {
                if (string.IsNullOrEmpty(item)) continue;
                Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                item,
                "异常邮件", ex, false, null);
            }

            Server.ClearError();
            Response.Redirect("/home/Error");
        }
        /// <summary>
        /// 这个时间是在第一次访问网站的时候触发..比如你发布了一个网站,,
///我第一个来访问,,就会触发这个事件..
///以后再有人来访问就不会触发了..
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}