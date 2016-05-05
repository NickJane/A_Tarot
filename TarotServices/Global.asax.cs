using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.ComponentModel;

namespace TarotServices
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            // Call target Controller and pass the routeData.            
            //IController errorController = new WEB.Controllers.ErrorController();            
            //errorController.Execute(new RequestContext(           new HttpContextWrapper(Context), routeData));
            /*
             * tips 动态请求controller的action
                IController errorController = new Admin3.Controllers.AccountController();
                errorController.Execute(new RequestContext(Context,
             * */
            string ex = Lib.Utils.LogHelper.ErrorLog(Server.GetLastError());
//#if Release
            foreach (var item in BLL.BLLSystemConfig.ExceptionToEmails.Split(';'))
            {
                if (string.IsNullOrEmpty(item)) continue;
                Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                item,
                "异常邮件", ex, false, null);
            }
//#endif
            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}