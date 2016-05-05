using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BLL.ActionFilters
{
    public class CompleteRunTimesFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Items["StartCompleteRunTimes"] = DateTime.Now;
        }
        public override void  OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            DateTime? time=null;
            if(filterContext.HttpContext.Items["StartCompleteRunTimes"] != null)
                time=Convert.ToDateTime(filterContext.HttpContext.Items["StartCompleteRunTimes"]);
            if (time != null)
            {
                TimeSpan ts = DateTime.Now - time.Value;
                filterContext.HttpContext.Items["AllCompleteRunTimes"] = ts.Milliseconds;
            }
        }
    }
}
