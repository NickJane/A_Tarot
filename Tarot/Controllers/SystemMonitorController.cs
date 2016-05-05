using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tarot.Controllers
{
    /// <summary>
    /// 静态化入口
    /// </summary>
    public class SystemMonitorController : Controller
    {
        //
        // GET: /SystemMonitor/
        [HttpGet]
        public ActionResult ClearStaticValues()
        {
            BLL.BLLSystemConfig.Init();
            BLL.BLLBadWordsFilter.badWordsFilter = null;
            return Content("1");
        }

    }
}
