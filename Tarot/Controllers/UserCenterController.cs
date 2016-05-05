using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarot.Models;
using UserManager.Framework;
using BLL;
using UserManager.Framework.ModelMate;
using Authorizations.Framework;
using Lib.ClassExt;
using System.Transactions;
using System.IO;
using Lib.Data;
using Data.MongoFramework.ModelMate;
using MongoDB.Driver.Builders;

namespace Tarot.Controllers
{
    public class UserCenterController :  BaseController
    {
        #region 用户中心
        [Authorize]
        public ActionResult UserCenter()
        {
            return View();
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        public ActionResult MyUserInfo() { return View( UserVisitor.GetUserInfoCoreByUserID(UserAccount.UserID)); }
        #region 上传头像
        [Authorize]
        public ActionResult ChangeAvatar() {
            var str=UserVisitor.GetUserInfoCoreByUserID(UserAccount.UserID);
            ViewBag.State = true;
            if (!str.Provinceid.HasValue || !str.Birthday.HasValue || !str.Sender.HasValue)
            {
                ViewBag.State = false;
                return View();
            }
            int port = Request.Url.Port;
            string ApplicationPath = Request.ApplicationPath != "/" ? Request.ApplicationPath : string.Empty;

            string Localhost = "http://" + Request.Url.Host + "/usercenter/UpdateAvatar";
            string EncodeLocalhost = HttpUtility.UrlEncode(Localhost);

            ViewData["avatarFlashParam"] = string.Format("{0}/common/camera.swf?nt=1&inajax=1&appid=1&input={1}&ucapi={2}",
                                            "http://" + Request.Url.Host + "/content", UserAccount.UserID, EncodeLocalhost);
            ViewData["Localhost"] = Localhost;
            ViewData["uid"] = UserAccount.UserID;
            return View();
        }
        [Authorize]
        public ActionResult UpdateAvatar() {
            string uid = Request.QueryString["input"];
            if (!string.IsNullOrEmpty(Request["Filename"]) && !string.IsNullOrEmpty(Request["Upload"]))
            {
                return Content(UploadTempAvatar(uid));
            }
            if (!string.IsNullOrEmpty(Request["avatar1"]) && !string.IsNullOrEmpty(Request["avatar2"]) && !string.IsNullOrEmpty(Request["avatar3"]))
            {

                CreateDir(uid);
                if (!(SaveAvatar("avatar1", uid) && SaveAvatar("avatar2", uid) && SaveAvatar("avatar3", uid)))
                {
                    return Content("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
                }
                using (UserManager.Framework.Model.UserManagerExamEntities entity = new UserManager.Framework.Model.UserManagerExamEntities()) {
                    entity.ExecuteStoreCommand("update auth_userinfocore set AvatarUrl=@p0 where userid=@p1",
                        "http://" + Request.Url.Host + "/content/images/avatars/" + uid + "/small.jpg", uid);
                    UserVisitor.DeleteCacheOfUserInfoCore(uid.ToInt(0));
                }
                return Content("<?xml version=\"1.0\" ?><root><face success=\"1\"/></root>");
            }
            return Content("<?xml version=\"1.0\" ?><root><face success=\"0\"/></root>");
        }
        private void CreateDir(string uid)
        {
            string avatarDir = string.Format(Server.MapPath("~/content/images/avatars")+ "/{0}",
                 uid);
            if (!Directory.Exists(avatarDir))
                Directory.CreateDirectory(avatarDir);
        }
        private bool SaveAvatar(string avatar, string uid)
        {
            byte[] b = FlashDataDecode(HttpContext.Request[avatar]);
            if (b.Length == 0)
                return false;
            string size = "";
            if (avatar == "avatar1")
                size = "large";
            else if (avatar == "avatar2")
                size = "medium";
            else
                size = "small";
            string avatarFileName = string.Format(Server.MapPath("~/content/images/avatars")+ "/{0}/{1}.jpg",
                uid, size);
            FileStream fs = new FileStream(avatarFileName, FileMode.Create);
            fs.Write(b, 0, b.Length);
            fs.Close();
            return true;
        }
        private byte[] FlashDataDecode(string s)
        {
            byte[] r = new byte[s.Length / 2];
            int l = s.Length;
            for (int i = 0; i < l; i = i + 2)
            {
                int k1 = ((int)s[i]) - 48;
                k1 -= k1 > 9 ? 7 : 0;
                int k2 = ((int)s[i + 1]) - 48;
                k2 -= k2 > 9 ? 7 : 0;
                r[i / 2] = (byte)(k1 << 4 | k2);
            }
            return r;
        }
        private string UploadTempAvatar(string uid)
        {
            string filename = uid + ".jpg";
            string uploadDir = Server.MapPath("~/content/images/tempavatars");
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            filename = "/" + filename;
            if (HttpContext.Request.Files.Count > 0)
            {
                HttpContext.Request.Files[0].SaveAs(uploadDir + filename);
            }

            return "http://" + Request.Url.Host + "/content/images/tempavatars" + filename;
        }
#endregion
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserVisitor.ValidUserLogin(UserAccount.UserName.ToString(), model.OldPassword);

                if (user != null)
                {
                    UserVisitor.UpdateState(user.UserID.ToString(), "password", model.NewPassword.ToMd5());
                    ModelState.AddModelError("", "修改密码成功。");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确。");
                }
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult MyUserInfo(UserManager.Framework.Model.Auth_UserInfoCore model) {
            if (ModelState.IsValid)
            {
                UserManager.Framework.Model.UserManagerExamEntities entity;
                var mo = UserVisitor.GetUserInfoCoreByFunc(x => x.UserID == UserAccount.UserID, out entity);
                base.UpdateModel(mo);
                var openedValues="";
                foreach (var item in Request.Form.AllKeys.Where(x=>x.ToLower().StartsWith("ckb")))
                {
                    if (Request.Form[item] == "on")
                        openedValues += item.Replace("ckb","")+";";
                }
                mo.OpendValues = openedValues;
                entity.SaveChanges();
                UserVisitor.DeleteCacheOfUserInfoCore(UserAccount.UserID);
                ModelState.AddModelError("", "更新成功");
                return View(mo); 
            }
            return View(model); 
        }

        #endregion

        #region 短消息
        [Authorize]
        public ActionResult MessageInBox(string id)
        {
            var pidx = id.ToInt(1);
            var psize = 20;
            int count = 0;
            var lst = BLLMessages.GetMessages(new PagerInfo { Page = pidx, PageSize = psize }, true, UserAccount.UserID, out count);
            ViewBag.PageCount = ((count - 1) / psize) + 1;
            ViewBag.CurrentPage = pidx;
            return View(lst);
        }
        [Authorize]
        public ActionResult MessagePostBox(string id)
        {
            var pidx = id.ToInt(1);
            var psize = 20;
            int count = 0;
            var lst = BLLMessages.GetMessages(new PagerInfo { Page = pidx, PageSize = psize }, false, UserAccount.UserID, out count);
            ViewBag.PageCount = ((count - 1) / psize) + 1;
            ViewBag.CurrentPage = pidx;
            return View(lst);
        }
        [Authorize]
        public ActionResult MessageForm(string id) {
            if (!string.IsNullOrEmpty(id))
                return View(new Messages { ToUser = new M_User { NicName = id } });
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult MessageForm(Messages model)
        {
            string name = model.ToUser.NicName;
            if (string.IsNullOrEmpty(name)) { ModelState.AddModelError("", "请输入对方用户名"); return View(model); }
            var user = UserVisitor.GetModelByFunc(x => x.UserName == name);
            if (user==null) { ModelState.AddModelError("", "用户名不存在"); return View(model); }

            var counter= MongoDBHelper.Count(Data.MongoFramework.CollectionName.Messages,
                                MongoDB.Driver.Builders.Query.And(
                                        Query.EQ("FromUser.UserID", UserAccount.UserID),
                                        Query.GTE("PostTime", DateTime.Now.AddDays(-1))
                                        )
                                        );
            if (counter >=30) { ModelState.AddModelError("", "二十四小时内只能发送30条短信"); return View(model); }

            if (ModelState.IsValid) {
                model.FromUser = new M_User { NicName=UserAccount.UserName, UserID=UserAccount.UserID };
                model.isRead = false;
                model._id = MongoDB.Bson.BsonObjectId.GenerateNewId();
                model.PostTime = DateTime.Now;
                model.ToUser.UserID = user.UserID;

                BLLMessages.SendMessage(model);
                ModelState.AddModelError("","发送成功!!");
                return View(new Messages());
            }

            return View();
        }
        public ActionResult readmessage() {
            BLLMessages.ReadMessage(Request.Form["_id"]);
            
            return Content("");
        }
        #endregion

        public ActionResult UserInfo(string id) {
            int uid = id.ToInt(0);
            if (uid == 0) return Content("参数非法");

            ViewBag.UserInfo = UserVisitor.GetModelByUserID(uid);
            if (ViewBag.UserInfo == null)
                return Content("未找到这个用户");
            else
            {
                ViewBag.UserInfoCore = UserVisitor.GetUserInfoCoreByUserID(uid);
                return View();
            }
        }
    }
}
