using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BLL.ActionFilters
{
    /// <summary>
    /// 用于公开的ajax方法过滤.
    /// </summary>
    public class OpendAjaxFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            
        }
    }
}
