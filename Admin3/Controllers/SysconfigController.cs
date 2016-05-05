using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.ActionFilters;
using Data.Framework.DataProvider;
using BLL;
using System.Net;
using Lib.BaseClass;
using Lib.Data;
using Data.MongoFramework.ModelMate;
using Data.MongoFramework;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson;
using Lib.Utils;
using Lib.ClassExt;
using Data.Framework;
using Data.Framework.Model;
namespace Admin3.Controllers
{
    public class SysconfigController : BLL.BaseController
    {
        #region get
        [AuthorizationFilter(ResourceCode = "001001002001")]
        public ActionResult SystemParams()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001001002002")]
        public ActionResult BadWords() { return View(); }
        [AuthorizationFilter(ResourceCode = "001001002003")]
        public ActionResult NoCatchException() { return View(); }

        [AuthorizationFilter(ResourceCode = "001001002004")]
        public ActionResult UpdateStaticValues()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001001003001")]
        public ActionResult SystemLog()
        {
            return View();
        }
        #endregion


        [AuthorizationFilter(ResourceCode = "001001002001")]
        [HttpPost]
        public ActionResult SystemParams(FormCollection form)
        {
            string ExceptionsTo = form["ExceptionsTo"];
            var entity = SystemConfigVisitor.GetBaseDbContext();

            if (entity.SystemConfig.Where(x => x.C_Key.ToLower() == "exceptionsto").Count() == 0)
                entity.SystemConfig.AddObject(new SystemConfig { C_Key = "exceptionsto", C_Value = ExceptionsTo });
            else
                SystemConfigVisitor.GetInstance().Update("exceptionsto", ExceptionsTo);

            entity.SaveChanges();
            BLLSystemConfig.Init();
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001001002002")]
        [HttpPost]
        public ActionResult BadWords(FormCollection form) {
            string word = form["word"];
            if (form["actiontype"] == "add")
                SystemConfigVisitor.GetInstance().Update(BLLSystemConfig.Key_BadWords, BLLSystemConfig.BadWords + word + ";");
            else
                SystemConfigVisitor.GetInstance().Update(BLLSystemConfig.Key_BadWords, BLLSystemConfig.BadWords.Replace(word + ";", ""));

            BLL.BLLLogcs.AddAppLog((form["actiontype"] == "add" ? "添加" : "去掉") + "关键字" + word, "", ControllerContext);
            
            BLLSystemConfig.Init();
            var ret = new BLL.JsonResponse { State = 1 };
            return Json(new { ret }); 
        }

        [AuthorizationFilter(ResourceCode = "001001002003")]
        [HttpPost]
        public ActionResult NoCatchException(FormCollection form)
        {
            string word = form["word"];
            if (form["actiontype"] == "add")
                SystemConfigVisitor.GetInstance().Update(BLLSystemConfig.Key_NoCatchException, BLLSystemConfig.NoCatchException + word + ";");
            else
                SystemConfigVisitor.GetInstance().Update(BLLSystemConfig.Key_NoCatchException, BLLSystemConfig.NoCatchException.Replace(word + ";", ""));

            BLL.BLLLogcs.AddAppLog((form["actiontype"] == "add" ? "添加" : "去掉") + "类名" + word, "", ControllerContext);

            BLLSystemConfig.Init();
            var ret = new BLL.JsonResponse { State = 1 };
            return Json(new { ret });
        }

        [AuthorizationFilter(ResourceCode = "001001002004")]
        [HttpPost]
        public ActionResult UpdateStaticValues(FormCollection form)
        {
            ViewBag.Result = _UpdateStaticValues(HttpContext);
            return View();
        }
        public string _UpdateStaticValues(HttpContextBase content) {
            WebClient client = new WebClient();
            string json = "0";
            using (
                    System.IO.Stream stream = client.OpenRead("http://www." +
                       content.Request.Url.Host.Substring(content.Request.Url.Host.IndexOf('.') + 1) + "/systemmonitor/ClearStaticValues")
                )
            {
                using (System.IO.StreamReader _read = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    json = _read.ReadToEnd();
                }
            }
            return json;
        }

        [AuthorizationFilter(ResourceCode = "001001003001")]
        [HttpPost]
        public ActionResult GetListByParameters()
        {
            //初始化实体类的辅助分页父类属性
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            IList<IMongoQuery> condition = new List<IMongoQuery>();
            condition.Add(Query.EQ("ApplicationId", Request["ApplicationId"].ToInt(1)));
            if(Request["type"]!="-1")
                condition.Add(Query.EQ("LogType", Request["type"].ToInt(1)));
            if (!string.IsNullOrEmpty(Request["content"].Trim()))
                condition.Add(Query.Matches("Comment", BsonRegularExpression.Create(Request["content"], "gi")));
            if (!string.IsNullOrEmpty(Request["username"].Trim()))
                condition.Add(Query.Matches("UserName", BsonRegularExpression.Create(Request["username"], "gi")));

            IMongoSortBy sortby=null;
            if (pageParm.PIsAsc) sortby = SortBy.Ascending(pageParm.POrderField); else sortby = SortBy.Descending(pageParm.POrderField);


            var lst = MongoDBHelper.GetAll<LogOperate>(
                                    CollectionName.LogOperate,
                                    Query.And(condition.ToArray()),
                                    new PagerInfo { Page=pageParm.PCurrentPageIndex, PageSize=pageParm.PPageSize },
                                    sortby);

            var count = MongoDBHelper.Count(
                                            CollectionName.LogOperate,
                                            Query.And(condition.ToArray())
                                        );


            return Json(new { Rows = lst, Total = count });
        }

    }
}
