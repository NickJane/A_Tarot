using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin3.Controllers
{
    public class ShareDController : Controller
    {
        //
        // GET: /Share/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ForJs(string JS) {
            ViewBag.JS = HttpUtility.UrlEncode( JS);
            return View();
        }

    }
}
