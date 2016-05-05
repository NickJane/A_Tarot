using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Authorizations.Framework;
using System.Web;
namespace BLL.ActionFilters
{
    /// <summary>
    /// 验证是否有访问该页面的权限
    /// </summary>
    public class AuthorizationFilter: ActionFilterAttribute
    {
         

        /// <summary>
        /// ResourceCode|ResourceCode|ResourceCode
        /// </summary>
        public string ResourceCode { get; set; }
        public AuthorizationFilter() {
            this.Order = 1;
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            string[] codes=ResourceCode.Split('|');
            filterContext.HttpContext.Items["CurrentResourceCode"] = codes[0];

            bool flag = false;
            foreach (var item in codes)
            {
                if(Authorizations.Framework.AuthVisitor.HasPermission(BLL.UserInfo.UserAccount.UserID,null,item))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag) {
                filterContext.HttpContext.Response.Write("您没用权限这个页面");
                filterContext.HttpContext.Response.End();
                filterContext.Result = new EmptyResult();
            }
            //var user = 
            //if (user.IsSuper) return;
            //string[] res=Authorization.Framework.AuthFactory.GetInstance<IResource>()
        }
    }
}
