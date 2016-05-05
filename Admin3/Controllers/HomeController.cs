using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Authorizations.Framework;
using Authorizations.Framework.Enums;
using BLL;
using Lib.ClassExt;
namespace Admin3.Controllers
{
    public class HomeController : BaseController
    {
        [HandleError(View="error")]
        public ActionResult Index()
        {
            ViewBag.Message = "欢迎使用 ASP.NET MVC!";

            ViewBag.Resources = AuthVisitor.GetAllResource(CurrentApplicationID, "");
            ViewBag.UserAccount = base.UserAccount;
            ViewBag.isSuper = base.IsSuper;
            return View();
        }
        [HttpPost]
        public ActionResult TreeData(FormCollection form) {
            var pid = Request["pid"];
            var lst = AuthVisitor.GetAllResource(CurrentApplicationID, pid);
            var sb = new System.Text.StringBuilder();
            sb.Append("[");
            foreach (var item in lst.Where(x=>x.ResourceType==3))
            {
                if (!AuthVisitor.HasPermission(UserAccount.UserID, CurrentApplicationID, item.ResourceCode))
                    continue;

                sb.Append("{text:'" + item.LanguageCode + "', url:'" + item.ResourceUrl + "', children:[");
                foreach (var item2 in lst.Where(x=>x.ResourceType==4 && x.ResourceCode.StartsWith(item.ResourceCode)))
                {
                    if (!AuthVisitor.HasPermission(UserAccount.UserID, CurrentApplicationID, item2.ResourceCode))
                        continue;

                    sb.Append("{text:'" + item2.LanguageCode + "', url:'" + item2.ResourceUrl + "'},");
                }
                if (sb[sb.Length - 1] == ',') sb.Remove(sb.Length - 1, 1);
                sb.Append("]},");
            }
            if (sb[sb.Length - 1] == ',') sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return Content(sb.ToString());
        }
        public ActionResult About()
        {
            base.RedirectToLoginPage();
            return View();
        }

        public ActionResult HeartBeat() {
            return Json(new { OK = 1 });
        }

        public ActionResult Page2() {
            return View();
        }
        public ActionResult Error() {
            return View();
        }
        
    }
}
