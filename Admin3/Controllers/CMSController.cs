using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Admin3.Models;
using Authorizations.Framework;
using BLL;
using UserManager.Framework;
using UserManager.Framework.Enums;
using Lib.ClassExt;
using UserManager.Framework.ModelMate;
using Lib.Data;
using Data.MongoFramework;
using Data.MongoFramework.ModelMate;
using Lib.BaseClass;
using MongoDB.Bson;
using Data.Framework.DataProvider;
using System.Web.Routing;
using MongoDB.Driver.Builders;
namespace Admin3.Controllers
{
    public class CMSController : BaseController
    {
        //
        // GET: /CMS/
        #region  本站动态和公共区

        #region 本站动态 列表. 增删
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult News()
        {
            return View();
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult GetNewsList() {//本站动态列表
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var lst = Lib.Data.MongoDBHelper.GetAll<M_Post>(
                CollectionName.News,
                Query.And(
                    Query.EQ("PostType",Request.Form["ddlposttype"].ToInt()),
                    Query.EQ("PostState", Request.Form["state"].ToInt())
                ),
                new PagerInfo { Page = pageParm.PCurrentPageIndex, PageSize = pageParm.PPageSize },
                MongoDB.Driver.Builders.SortBy.Descending("PostTime")
                );
            var count = MongoDBHelper.Count(
                CollectionName.News, 
                Query.And(
                    Query.EQ("PostType", Request.Form["ddlposttype"].ToInt()),
                    Query.EQ("PostState", Request.Form["state"].ToInt())
                ));
            return Json(new { Rows = lst, Total = count });
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult NewsForm(string id) {//本站动态
            ViewBag.IsEdit = id == null ? false : true;

            M_Post model = id == null ? null :
                MongoDBHelper.GetOne<M_Post>(CollectionName.News, MongoDB.Driver.Builders.Query.EQ("_id", MongoDB.Bson.ObjectId.Parse(id)));
            return View(model);
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NewsForm(string id, M_Post post, FormCollection form)
        {//本站动态
            ViewBag.IsEdit = id == null ? false : true;
            if (!ModelState.IsValid) return View(post);
            post.Content = form["text"];
            if (!ViewBag.IsEdit)
            {
                post.AuditState = true;//默认为false. 目前不需要审核
                post.m_User = new M_User { AvatarURL = "", NicName = "塔罗来了", UserID = UserAccount.UserID };
                post.PostState = 1;
                post.PostTime = DateTime.Now;
                post.LastModifyTime = DateTime.Now;
                post.IP = Request.UserHostAddress;
                post._id = ObjectId.GenerateNewId();
                post._idForString = post._id.ToString();
                post.PostType = Request.Form["ddlposttype"].ToInt();
                Lib.Data.MongoDBHelper.InsertOne<M_Post>(CollectionName.News, post);

                BLLLogcs.AddAppLog("添加本站动态, title:" + post.Title + ", content:" + post.Content, "", ControllerContext);
            }
            else {
                Lib.Data.MongoDBHelper.UpdateAll<M_Post>(CollectionName.News, 
                    MongoDB.Driver.Builders.Query.EQ("_id", MongoDB.Bson.ObjectId.Parse(id)),
                    MongoDB.Driver.Builders.Update.Set("Content", post.Content).Set("Title", post.Title));
                BLLLogcs.AddAppLog("修改本站动态, title:" + post.Title + ", content:" + post.Content, "", ControllerContext);
            }
            return RedirectToAction("ForJs", "Shared", new { JS = "alert('操作成功');location.href='/cms/news';" });
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult deleteNews(string id) {
            MongoDBHelper.Delete(CollectionName.News, id);
            return Json(new BLL.JsonResponse { State = 1 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001002")]//作废
        public ActionResult PublicArea()
        {
            return View();
        }
        
        #endregion

        #region 首页区

        #region sildershow
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003002001")]
        public ActionResult sildershow() { return View(); }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003002001")]
        [HttpPost]
        public ActionResult sildershow(FormCollection form) {
            var v = form["sildershows0"] + "■" + form["sildershows1"] + "■" + form["sildershows2"];

            BLL.BLLLogcs.AddAppLog("修改首页slidershow值从" +
                BLL.BLLSystemConfig.Sildershow[0] + "■" + BLL.BLLSystemConfig.Sildershow[1] + "■" + BLL.BLLSystemConfig.Sildershow[2] + "■"
                 +"到" + v, "", this.ControllerContext);

            SystemConfigVisitor.GetInstance().Update(BLL.BLLSystemConfig.Key_sildershow, v);
            BLLSystemConfig.Init();

            //var routeData = new RouteData();    
            //routeData.Values.Add("controller", "sysconfig");
            //routeData.Values.Add("action", "UpdateStaticValues");
            //((IController)new SysconfigController()).Execute(new System.Web.Routing.RequestContext(HttpContext, routeData));
            new SysconfigController()._UpdateStaticValues(HttpContext);
            ModelState.AddModelError("", "更新成功");
            return View(); 
        }
        #endregion

        #region sildershowRight右侧文章块
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003002002")]
        public ActionResult sildershowRight() {
            var path = Lib.Utils.ConfigHelper.GetAppSetting("TarotWebSite")+"\\Views\\Home\\IndexPartial\\head.cshtml";
            ViewBag.htmls = Lib.Utils.FileHelper.ReadPageModel(path);
            return View();
        }

        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003002002")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult sildershowRight(FormCollection form)
        {
            var path = Lib.Utils.ConfigHelper.GetAppSetting("TarotWebSite") + "\\Views\\Home\\IndexPartial\\head.cshtml";
            
            BLL.BLLLogcs.AddAppLog("修改首页slidershow右侧文章列表从" +
                Lib.Utils.FileHelper.ReadPageModel(path)
                 + "到" + form["text"], "", this.ControllerContext);
            System.IO.File.WriteAllText(path, form["text"].Replace("&nbsp;",""),System.Text.Encoding.UTF8);
            ModelState.AddModelError("", "修改成功");
            return View();
        }
        #endregion

        #endregion

        #region test UEditor
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult TestUeditor() {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "001003001001")]
        public ActionResult TestUeditor(FormCollection form)
        {
            return View();
        }

        public ActionResult uploadimage() {
            var CMSImgFolder = Lib.Utils.ConfigHelper.GetAppSetting("CMSImgFolder");
            string state="SUCCESS";
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileExtName = file.FileName.Split('.')[1];

            }
            return Content("");
        }
        #endregion
    }
}
