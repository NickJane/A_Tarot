using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLL.ActionFilters;
using Lib.BaseClass;
using Authorizations.Framework;
using Authorizations.Framework.Model;
using UserManager.Framework.Model;
using UserManager.Framework;
using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.Model;
using Data.Framework.DataProvider;

namespace Admin3.Controllers
{
    public class TagController : BaseController
    {
        [AuthorizationFilter(ResourceCode = "001002001003")]
        public ActionResult List()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001002001003")]
        public ActionResult Form(int? id)
        {
            ViewBag.IsEdit = id == null ? false : true;

            Tag model = id != null ?
                TagVisitor.GetInstance().GetModelByFunc(x => x.TagID == id.Value) : new Tag();
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001002001003")]
        [HttpPost]
        public ActionResult Form(Tag model)
        {
            ViewBag.IsEdit = model.TagID == 0 ? false : true;
            if (ModelState.IsValid)
            {
                if (model.TagID == 0)
                {
                    var flag = true;
                    #region /*判断逻辑区*/
                    /*if (UserVisitor.Exists("username", model.UserName.Trim()))
                    {
                        ModelState.AddModelError("", "该用户已存在");
                        flag = false;
                    }*/
                    #endregion

                    if (flag)
                    {
                        model.State = 1;

                        var result = TagVisitor.GetInstance().Insert(model);
                        if (result == 1)
                            JSAlert_CloseIframe_ReloadGrid("添加成功");
                    }
                }
                else
                {
                    TarotEntities enti;
                    var dbModel = TagVisitor.GetInstance().GetModelByFunc(x => x.TagID == model.TagID
                                    , out enti);

                    if (dbModel == null)
                    {
                        JSCustom("parent.Reload();");
                        JSAlert_ReloadPage("并发冲突, 你看到的数据是旧的数据, 已经被其他用户修改过, 系统将刷新显示新的数据",
                                            "/Tagmanager/form/" + model.TagID);
                    }
                    else
                    {
                        UpdateModel(dbModel);
                        try
                        {
                            enti.SaveChanges();
                            JSAlert_CloseIframe_ReloadGrid("修改成功");
                        }
                        catch (Exception ex)
                        {
                            JSAlert_ReloadPage(ex.Message, "Tagmanager/form/" + model.TagID);
                        }
                    }
                }
            }
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001002001003")]
        public ActionResult Detail(int id)
        {
            return View(TagVisitor.GetInstance().GetModelByFunc(x => x.TagID == id));
        }

        /**************************AJAX**************************/
        public ActionResult GetListByParameters(Tag model)
        {
            //初始化实体类的辅助分页父类属性
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var lst = TagVisitor.GetInstance().GetListByParameters(model, ref pageParm).Select(x => new
            {
                TagID = x.TagID,
                TagName = x.TagName,
                Popularity = x.Popularity,
                State = ((BLL.Enums.State)x.State).ToLocalLanguage(),
            }).ToList();

            return Json(new { Rows = lst, Total = pageParm.PRecordCount });
        }

        [AuthorizationFilter(ResourceCode = "001001001001")]
        public ActionResult UpdateState()
        {
            string[] ids = Request.QueryString["keyids"].Split(',');
            string[] timeMarks = string.IsNullOrEmpty(Request.QueryString["timeMark"]) ? new string[] { } : Request.QueryString["timeMark"].Split('#');
            string field = Request.QueryString["field"];
            string value = Request.QueryString["value"];

            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i]))
                    continue;
                byte[] timemark = (timeMarks.Length > 0 && !string.IsNullOrEmpty(timeMarks[0])) ?
                                                                Convert.FromBase64String(timeMarks[i]) : null;
                TagVisitor.GetInstance().Update(ids[i], field, value, timemark);
            }
            return ReturnResult(1, "");
        }
    }
}