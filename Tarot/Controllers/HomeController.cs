using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;
using BLL;
using Lib.Data;
using Lib.ClassExt;
namespace Tarot.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string id)
        {
            ViewBag.SilderShow0 = BLLSystemConfig.Sildershow[0];
            ViewBag.SilderShow1 = BLLSystemConfig.Sildershow[1];
            ViewBag.SilderShow2 = BLLSystemConfig.Sildershow[2];
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
