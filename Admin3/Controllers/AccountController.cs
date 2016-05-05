using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;


using Admin3.Models;
using Authorizations.Framework;
using BLL;
using UserManager.Framework;
using UserManager.Framework.Enums;
using Lib.ClassExt;
using UserManager.Framework.ModelMate;

namespace Admin3.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return View();
        }
        [HttpPost]
        public ActionResult Login(Admin3.Models.LoginModel model)
        {
            if (ModelState.IsValid)
            {
                UserManager.Framework.Model.Auth_UserAccount user = UserVisitor.ValidUserLogin(model.UserName, model.Password);
                
                if (user != null)
                {
                    if (user.State == (int)UserState.Normal)//用户状态正确
                    {
                        //初始化全局用户信息, 使用session保存
                        //BLL.UserInfo.UserSystemModel = new UserSystemModel
                        var tempUser = new UserSystemModel
                        {
                            UserAccount = new Auth_UserAccountMate { 
                                ApplicationID=user.ApplicationID,
                                State=user.State,
                                UserID=user.UserID,
                                UserName=user.UserName
                            },
                            CurrentUserResources = AuthVisitor.GetCurrentUserResourceCode(user.UserID, CurrentApplicationID),
                            IsSuper = Authorizations.Framework.AuthVisitor.isSuper(user.UserID)
                        };
                        
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                            1, 
                            model.UserName,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(FormsAuthentication.Timeout.Minutes),
                            false,
                            tempUser.ToJson<UserSystemModel>());//票据
                        string cookiestr = FormsAuthentication.Encrypt(ticket);//加密
                        HttpCookie cookie = (new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr));
                        //cookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(cookie);//写入客户端
                        
                        return Redirect(!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) ? Server.UrlDecode(Request.QueryString["ReturnUrl"]) : FormsAuthentication.DefaultUrl);

                    }
                }
                else
                {
                    ModelState.AddModelError("", "用户名或者密码错误, 请重新输入!");
                }
            }
            return View();
        }

        
    }
}
