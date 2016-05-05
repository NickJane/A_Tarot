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

namespace Admin3.Controllers
{
    public class Auth_RoleManagerController : BaseController
    {
        [AuthorizationFilter(ResourceCode = "001001001002")]
        public ActionResult List()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001001001002")]
        public ActionResult Form(int? id)
        {
            ViewBag.IsEdit = id == null ? false : true;

            Auth_Role model = id != null ?
                AuthVisitor.GetAuth_RoleByFunc(x => x.RoleID == id.Value) : new Auth_Role();
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001001001002")]
        [HttpPost]
        public ActionResult Form(Auth_Role model)
        {
            ViewBag.IsEdit = model.RoleID == 0 ? false : true;
            if (ModelState.IsValid)
            {
                if (model.RoleID == 0)
                {
                    var flag = true;
                    #region /*判断逻辑区*/
                    if (AuthVisitor.GetAuth_RoleByFunc(x => x.LanguagueCode == model.LanguagueCode) != null) {
                        ModelState.AddModelError("","该角色已经存在");
                        flag = false;
                    }
                    #endregion

                    if (flag)
                    {
                        model.State = 1;
                        var res = Request.Form["roleRes"].Split(',').Where(x => x.ToLower() != "false").Select(x => x.ToInt());
                        
                        var result = AuthVisitor.InsertAuth_Role(model, res);
                        if (result >= 1)
                            JSAlert_CloseIframe_ReloadGrid("添加成功");
                    }
                }
                else
                {
                    AuthEntities enti;
                    var dbModel = AuthVisitor.GetAuth_RoleByFunc(x => x.RoleID == model.RoleID, out enti);

                    if (dbModel == null)
                    {
                        JSCustom("parent.Reload();");
                        JSAlert_ReloadPage("并发冲突, 你看到的数据是旧的数据, 已经被其他用户修改过, 系统将刷新显示新的数据",
                                            "/Auth_Role/form/" + model.RoleID);
                    }
                    else
                    {
                        UpdateModel(dbModel);
                        try
                        {
                            enti.ExecuteStoreCommand("delete from auth_roleresource where roleid=@p0", dbModel.RoleID);
                            var res = Request.Form["roleRes"].Split(',').Where(x => x.ToLower() != "false").Select(x => x.ToInt());
                            foreach (var item in res)
                            {
                                enti.Auth_RoleResource.AddObject(new Auth_RoleResource { RoleID = dbModel.RoleID, ResourceID = item });
                            }
                            enti.SaveChanges();
                            JSAlert_CloseIframe_ReloadGrid("修改成功");
                        }
                        catch (Exception ex)
                        {
                            JSAlert_ReloadPage(ex.Message, "/Auth_Role/form/" + model.RoleID);
                        }
                    }
                }
            }
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001001001002")]
        public ActionResult Detail(int id)
        {
            return View(AuthVisitor.GetAuth_RoleByFunc(x => x.RoleID == id));
        }

        /**************************AJAX**************************/
        public ActionResult GetListByParameters(Auth_Role model)
        {
            //初始化实体类的辅助分页父类属性
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var lst = AuthVisitor.GetListByParameters(model, ref pageParm).Select(x => new
            {
                RoleID = x.RoleID,
                LanguagueCode = x.LanguagueCode,
                State = ((BLL.Enums.State)x.State).ToLocalLanguage(),
                IsSuperRole = x.IsSuperRole,
                ApplicationID = ((Authorizations.Framework.Enums.ApplicationEnum)x.ApplicationID).ToLocalLanguage(),
            }).ToList();

            return Json(new { Rows = lst, Total = pageParm.PRecordCount });
        }

        [AuthorizationFilter(ResourceCode = "001001001002")]
        public ActionResult UpdateState()
        {
            string[] ids = Request.QueryString["keyids"].Split(',');
            string[] timeMarks = Request.QueryString["timeMark"].Split('#');
            string field = Request.QueryString["field"];
            string value = Request.QueryString["value"];

            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i]))
                    continue;
                byte[] timemark = (timeMarks.Length > 0 && !string.IsNullOrEmpty(timeMarks[0])) ?
                                                                Convert.FromBase64String(timeMarks[i]) : null;
                AuthVisitor.UpdateState(ids[i], field, value, timemark);
            }
            return ReturnResult(1, "");
        }


        public ActionResult getResourcesByApplicationID() { 
            int id=Request.QueryString["id"].ToInt();
            int roleid = Request.QueryString["roleid"].ToInt();
            return PartialView("auth/_resourceListPartial", (new int[] { id, roleid }).ToList()); //(new int[] { CurrentApplicationID, id }).ToList());
        }
    }
}