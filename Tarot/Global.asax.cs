using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;

namespace Tarot
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new BLL.ActionFilters.CompleteRunTimesFilter());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("usercenter/crossdomain.xml");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "home", action = "index", id = UrlParameter.Optional } // 参数默认值
            );

        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            /*
             * tips 动态请求controller的action
                var routeData = new RouteData();    
                routeData.Values.Add("controller", "shared");
                routeData.Values.Add("action", "notfound");
                IController errorController = new Tarot.Controllers.SharedController();
                errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                 
             * */
            
            
            
            string ex= Lib.Utils.LogHelper.ErrorLog(Server.GetLastError());

            HttpException httpex = exception as HttpException;
            if (httpex != null && httpex.GetHttpCode() == 404)
            {
                return;
            }
//#if Release
            foreach (var item in BLL.BLLSystemConfig.ExceptionToEmails.Split(';'))
            {
                if (string.IsNullOrEmpty(item)) continue;
                Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                item,
                "异常邮件", ex, false, null);
            }
//#endif
            //Server.ClearError();
            //Response.Redirect("/shared/Error");
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}