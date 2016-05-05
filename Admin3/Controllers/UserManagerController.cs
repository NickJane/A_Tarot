using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLL.ActionFilters;
using Lib.BaseClass;
using UserManager.Framework.Model;
using UserManager.Framework;
using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.DataProvider;
using Authorizations.Framework;
namespace Admin3.Controllers
{
    public class UserManagerController : BaseController
    {
#region 老的用户中心
        [AuthorizationFilter(ResourceCode="001001001001")]
        public ActionResult UserList()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001001001001")]
        public ActionResult Form(int? id) {
            ViewBag.IsEdit = id == null ? false : true;

            UserManager.Framework.Model.UserManagerExamEntities enti;
            UserManager.Framework.Model.Auth_UserAccount model = id != null ?
                UserVisitor.GetModelByFunc(x=>x.UserID==id.Value,out enti) : new UserManager.Framework.Model.Auth_UserAccount();
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001001001001")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Form(UserManager.Framework.Model.Auth_UserAccount model)
        {
            ViewBag.IsEdit = model.UserID == 0 ? false : true;
            if (ModelState.IsValid)
            {
                if (model.UserID == 0)
                {
                    var flag = true;
                    #region /*判断逻辑区*/
                    if (UserVisitor.Exists("username", model.UserName.Trim()))
                    {
                        ModelState.AddModelError("", "该用户已存在");
                        flag = false;
                    }
                    #endregion

                    if (flag)
                    {
                        model.State = 1;
                        model.Comment = Server.HtmlEncode(model.Comment);
                        var res = (Request.Form["ckbrole"] != null) ? Request.Form["ckbrole"].Split(',').Where(x => x.ToLower() != "false").Select(x => x.ToInt()).ToList() : new List<int>();
                        
                        var result = UserVisitor.InsertUserAccount(model);
                        Authorizations.Framework.Model.AuthEntities enty = new Authorizations.Framework.Model.AuthEntities();
                        foreach (var item in res)
                        {
                            enty.Auth_UserRole.AddObject(new Authorizations.Framework.Model.Auth_UserRole { RoleID = item, UserID = result });
                        }
                        enty.SaveChanges();
                        if (result >0)
                            return RedirectToAction("ForJs", "Shared", new
                            {
                                JS = "parent.Reload();alert('添加用户成功');"
                            });
                            JSAlert_CloseIframe_ReloadGrid("添加用户成功");
                    }
                }
                else {
                    UserManager.Framework.Model.UserManagerExamEntities enti;
                    var dbModel = UserVisitor.GetModelByFunc(x => x.UserID == model.UserID && x.TimeMark.SequenceEqual(model.TimeMark),out enti);
                    
                    if (dbModel == null)
                    {
                        return RedirectToAction("ForJs", "Shared", new
                        {
                            JS = "alert('并发冲突,你看到的数据是旧的数据,已经被其他用户修改过,系统将刷新显示新的数据');"
                             +"location.href='/usermanager/form/" + model.UserID + "';"
                        });
                    }
                    else
                    {
                        UpdateModel(dbModel);
                        try
                        {
                            var res = (Request.Form["ckbrole"] != null) ? Request.Form["ckbrole"].Split(',').Where(x => x.ToLower() != "false").Select(x => x.ToInt()).ToList() : new List<int>();
                            Authorizations.Framework.Model.AuthEntities Authenti = new Authorizations.Framework.Model.AuthEntities();
                            Authenti.ExecuteStoreCommand("delete from auth_userrole where userid=@p0", dbModel.UserID);
                            CacheHelper.Remove("useridResourceCode" + dbModel.UserID);
                            foreach (var item in res)
                            {
                                Authenti.Auth_UserRole.AddObject(new Authorizations.Framework.Model.Auth_UserRole { RoleID = item, UserID = model.UserID });
                            }
                            Authenti.SaveChanges();
                            //dbModel.Password = dbModel.Password.ToMd5();
                            enti.SaveChanges();
                            return RedirectToAction("ForJs", "Shared", new
                            {
                                JS = "parent.Reload();alert('修改成功');"
                            });
                        }
                        catch (Exception ex)
                        {
                            JSAlert_ReloadPage(ex.Message, "/usermanager/form/" + model.UserID);
                        }
                    }
                }
            }
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001001001001")]
        public ActionResult Detail(int id) {
            return View(UserVisitor.GetModelByFunc(x => x.UserID == id));
        }
#endregion
        /**************************AJAX**************************/
        public ActionResult GetListByParameters(Auth_UserAccount model)
        {
            //初始化实体类的辅助分页父类属性
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var lst = UserVisitor.GetListByParameters(model, ref pageParm)//.ToList();
            .Select(x => new
        {
            UserID = x.UserID,
            UserName = x.UserName,
            Comment = x.Comment,
            CreateTime = x.CreateTime != null ? x.CreateTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "null",
            State = ((UserManager.Framework.Enums.UserState)x.State).ToLocalLanguage(),
            TimeMark = System.Convert.ToBase64String(x.TimeMark),
            TimeMark2 = x.TimeMark,
            LastLoginTime = x.LastLoginTime.HasValue ? x.LastLoginTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "null"
            
        }).ToList();

            return Json(new { Rows = lst, Total = pageParm.PRecordCount });
        }

        [AuthorizationFilter(ResourceCode = "001001001001")]
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
                UserVisitor.UpdateState(ids[i], field, value, timemark);
            }
            return ReturnResult(1, "");
        }

        public ActionResult getrolesbyappid() {
            int appid = Request.QueryString["id"].ToInt(0);
            int userid = Request.QueryString["userid"].ToInt(0);
            var lst = AuthVisitor.GetRoles(appid);
            var roleids = AuthVisitor.GetUserRolesByUserID(userid).Select(x => x.RoleID).ToList();
            return Json(new { Data = lst.Select(x => new { RoleID = x.RoleID, RoleName = x.LanguagueCode, Checked = roleids.Contains(x.RoleID)}) }, JsonRequestBehavior.AllowGet);
        }
        #region json
        /*
         @"{ 'Rows': [
    { 'CustomerID': 'ALFKI', 'CompanyName': 'Alfreds Futterkiste', 'ContactName': 'Maria Anders', 'ContactTitle': 'Sales Representative', 'Address': 'Obere Str. 57', 'City': 'Berlin', 'Region': 'null', 'PostalCode': '12209', 'Country': 'Germany', 'Phone': '030-0074321', 'Fax': '030-0076545' },
    { 'CustomerID': 'ANATR', 'CompanyName': 'Ana Trujillo Emparedados y helados', 'ContactName': 'Ana Trujillo', 'ContactTitle': 'Owner', 'Address': 'Avda. de la Constitución 2222', 'City': 'México D.F.', 'Region': 'null', 'PostalCode': '05021', 'Country': 'Mexico', 'Phone': '(5) 555-4729', 'Fax': '(5) 555-3745' },
    { 'CustomerID': 'ANTON', 'CompanyName': 'Antonio Moreno Taquería', 'ContactName': 'Antonio Moreno', 'ContactTitle': 'Owner', 'Address': 'Mataderos  2312', 'City': 'México D.F.', 'Region': 'null', 'PostalCode': '05023', 'Country': 'Mexico', 'Phone': '(5) 555-3932', 'Fax': 'null' },
    { 'CustomerID': 'AROUT', 'CompanyName': 'Around the Horn', 'ContactName': 'Thomas Hardy', 'ContactTitle': 'Sales Representative', 'Address': '120 Hanover Sq.', 'City': 'London', 'Region': 'null', 'PostalCode': 'WA1 1DP', 'Country': 'UK', 'Phone': '(171) 555-7788', 'Fax': '(171) 555-6750' },
    { 'CustomerID': 'BERGS', 'CompanyName': 'Berglunds snabbköp', 'ContactName': 'Christina Berglund', 'ContactTitle': 'Order Administrator', 'Address': 'Berguvsvägen  8', 'City': 'Luleå', 'Region': 'null', 'PostalCode': 'S-958 22', 'Country': 'Sweden', 'Phone': '0921-12 34 65', 'Fax': '0921-12 34 67' },
    { 'CustomerID': 'BLAUS', 'CompanyName': 'Blauer See Delikatessen', 'ContactName': 'Hanna Moos', 'ContactTitle': 'Sales Representative', 'Address': 'Forsterstr. 57', 'City': 'Mannheim', 'Region': 'null', 'PostalCode': '68306', 'Country': 'Germany', 'Phone': '0621-08460', 'Fax': '0621-08924' },
    { 'CustomerID': 'BLONP', 'CompanyName': 'Blondel père et fils', 'ContactName': 'Frédérique Citeaux', 'ContactTitle': 'Marketing Manager', 'Address': '24, place Kléber', 'City': 'Strasbourg', 'Region': 'null', 'PostalCode': '67000', 'Country': 'France', 'Phone': '88.60.15.31', 'Fax': '88.60.15.32' },
    { 'CustomerID': 'BOLID', 'CompanyName': 'Bólido Comidas preparadas', 'ContactName': 'Martín Sommer', 'ContactTitle': 'Owner', 'Address': 'C/ Araquil, 67', 'City': 'Madrid', 'Region': 'null', 'PostalCode': '28023', 'Country': 'Spain', 'Phone': '(91) 555 22 82', 'Fax': '(91) 555 91 99' },
    { 'CustomerID': 'BONAP', 'CompanyName': 'Bon app', 'ContactName': 'Laurence Lebihan', 'ContactTitle': 'Owner', 'Address': '12, rue des Bouchers', 'City': 'Marseille', 'Region': 'null', 'PostalCode': '13008', 'Country': 'France', 'Phone': '91.24.45.40', 'Fax': '91.24.45.41' },
    { 'CustomerID': 'BOTTM', 'CompanyName': 'Bottom-Dollar Markets', 'ContactName': 'Elizabeth Lincoln', 'ContactTitle': 'Accounting Manager', 'Address': '23 Tsawassen Blvd.', 'City': 'Tsawwassen', 'Region': 'BC', 'PostalCode': 'T2F 8M4', 'Country': 'Canada', 'Phone': '(604) 555-4729', 'Fax': '(604) 555-3745' }
], 'Total': '10'
        }"
         */
        #endregion

        [HttpPost]
        public ActionResult changepwd(FormCollection form) {
            if (form["uid"] == BLL.UserInfo.UserAccount.UserID.ToString() || Authorizations.Framework.AuthVisitor.HasPermission(UserAccount.UserID, CurrentApplicationID, "001001001001"))
            {
                try
                {
                    UserVisitor.UpdateState(form["uid"], "password", form["pwd"].ToMd5());
                    return Json(new JsonResponse { State = (int)BLL.Enum_JsonResponse.OK, ResponseText = "无权限" });
                }
                catch {
                    return Json(new JsonResponse { State = (int)BLL.Enum_JsonResponse.ERROR, ResponseText = "出错了......" });
                }
            }
            else return Json(new JsonResponse { State = (int)BLL.Enum_JsonResponse.ERROR, ResponseText="无权限" });
        }


        #region new 网站用户信息管理中心
        [AuthorizationFilter(ResourceCode = "001004001001")]
        public ActionResult TarotUserList() {
            return View();
        }
        [AuthorizationFilter(ResourceCode = "001004001001")]
        public ActionResult TarotUserForm(int? id)
        {
            ViewBag.IsEdit = id == null ? false : true;

            Auth_UserInfoCore model = id != null ?
                UserVisitor.GetUserInfoCoreByUserID(id.Value) : new Auth_UserInfoCore();
            return View(model);;
        }

        [AuthorizationFilter(ResourceCode = "001004001001")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TarotUserForm(UserManager.Framework.Model.Auth_UserInfoCore model)
        {
            ViewBag.IsEdit = model.UserID == 0 ? false : true;
            if (ModelState.IsValid)
            {
                UserManager.Framework.Model.UserManagerExamEntities enti;
                var dbModel = UserVisitor.GetUserInfoCoreByFunc(x => x.UserID == model.UserID, out enti);
                UpdateModel(dbModel);
                try
                {
                    enti.SaveChanges();
                    UserVisitor.DeleteCacheOfUserInfoCore(dbModel.UserID.ToInt(0));
                    return RedirectToAction("ForJs", "Shared", new
                    {
                        JS = "parent.Reload();alert('修改成功');"
                    });
                }
                catch (Exception ex)
                {
                    JSAlert_ReloadPage(ex.Message, "/usermanager/TarotUserForm/" + model.UserID);
                }
            }
            return View(model);
        }

        #endregion
    }
}
