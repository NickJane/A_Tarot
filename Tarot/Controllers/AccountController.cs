using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Tarot.Models;
using UserManager.Framework;
using BLL;
using UserManager.Framework.ModelMate;
using Authorizations.Framework;
using Lib.ClassExt;
using System.Transactions;
using QConnectSDK.Context;
using QConnectSDK;
using System.Text;

namespace Tarot.Controllers
{
    public class AccountController : BaseController
    {
        /// <summary>
        /// cookie失效天数
        /// </summary>
        static int cookietimeout = 2;
        #region 登入登出
        public ActionResult LogOn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            if (TempData["ValidateImageCode"] == null || TempData["ValidateImageCode"].ToString() != Request.Form["imgcode"])
            {
                ModelState.AddModelError("", "验证码输入错误。");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                string returnUrl = Request.QueryString["returnUrl"];
                if (!string.IsNullOrEmpty(returnUrl))
                    returnUrl = "http://" + Request.Url.Host + returnUrl;

                UserManager.Framework.Model.Auth_UserAccount User = UserVisitor.ValidUserLogin(model.UserName, model.Password);
                if (User != null)
                {
                    UserManager.Framework.Model.Auth_UserInfoCore info = null;
                    using (var entity = new UserManager.Framework.Model.UserManagerExamEntities())
                    {
                        info = entity.Auth_UserInfoCore.Where(x => x.UserID == User.UserID).First();
                    }

                    _Login(new Auth_UserAccountMate
                    {
                        ApplicationID = CurrentApplicationID,
                        State = 1,
                        UserID = User.UserID,
                        UserName = model.UserName,
                        CreateTime = User.CreateTime == null ? DateTime.Now : User.CreateTime.Value,
                        LastLoginTime = User.LastLoginTime == null ? DateTime.Now : User.LastLoginTime.Value,
                        AvatarURL = string.IsNullOrEmpty(info.AvatarUrl) ? Url.Content("~/content/images/men_head.gif") : info.AvatarUrl,
                        NicName = info.NicName,
                        UserPoint = info.UserPoint.HasValue ? info.UserPoint.Value : 0,
                        UserSpecialID = info.UserSpecialID
                    }, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/UserCenter/usercenter|用户中心;/home/index|首页"));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        public ActionResult QQLogin(string returnUrl)
        {
            var context = new QzoneContext();
            string state = Guid.NewGuid().ToString().Replace("-", "");
            Session["requeststate"] = state;
            //string scope = "get_user_info,add_share,list_album,upload_pic,check_page_fans,add_t,add_pic_t,del_t,get_repost_list,get_info,get_other_info,get_fanslist,get_idolist,add_idol,del_idol,add_one_blog,add_topic,get_tenpay_addr";
            string scope = "get_user_info,add_share ,check_page_fans ,add_t ,del_t ,add_pic_t ,get_repost_list ,get_info,get_other_info,get_fanslist,get_idolist,add_idol,del_idol";
            var authenticationUrl = context.GetAuthorizationUrl(state, scope);
            return new RedirectResult(authenticationUrl);

        }
        public ActionResult QQCallBackURI()
        {
            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch(); st.Start();
            Lib.Utils.LogHelper.CustomLog("\r\t\r\t" + Request.Url.Query + "\r\t");
            if (Request.Params["code"] != null)
            {
                QOpenClient qzone = null;

                var verifier = Request.Params["code"];
                var state = Request.Params["state"];
                string requestState = Session["requeststate"].ToString();
                Session.Remove("requeststate");
                if (state == requestState)
                {
                    try
                    {
                        Lib.Utils.LogHelper.CustomLog("运行计时1,," + st.ElapsedMilliseconds + "\r\n");
                        qzone = new QOpenClient(verifier, state);
                        Lib.Utils.LogHelper.CustomLog("运行计时2,," + st.ElapsedMilliseconds + "\r\n");
                        Lib.Utils.LogHelper.CustomLog("该QQ的OAuthToken.AccessToken,," + qzone.OAuthToken.AccessToken + "\r\n");
                        Lib.Utils.LogHelper.CustomLog("该QQ的OAuthToken.OpenId,," + qzone.OAuthToken.OpenId + "\r\n");
                        Lib.Utils.LogHelper.CustomLog("开始请求QQ空间信息,,\r\n");
                        var currentUser = qzone.GetCurrentUser();
                        Lib.Utils.LogHelper.CustomLog("结束请求QQ空间信息,,\r\n");
                        var friendlyName = currentUser.Nickname;
                        Lib.Utils.LogHelper.CustomLog("性别" + currentUser.Gender + "  运行计时3,," + st.ElapsedMilliseconds + "\r\n");

                        var userInfoCore = (UserVisitor.GetUserInfoCoreByFunc(x => x.QQKey == qzone.OAuthToken.OpenId));

                        if (userInfoCore == null)
                        {//如果这个QQ没有绑定过账号
                            Lib.Utils.LogHelper.CustomLog("如果这个QQ没有绑定过账号, 现在去跳转,," + st.ElapsedMilliseconds + "\r\n");
                            Session["QOpenClient"] = qzone;
                            ViewBag.QZoneAvator = currentUser.Figureurl;
                            return View("QQRegister", new QQWeiboRegisterModel { UserName = friendlyName });
                        }
                        else
                        {
                            Lib.Utils.LogHelper.CustomLog("如果这个QQ绑定过账号,," + st.ElapsedMilliseconds + "\r\n");
                            var userAccount = UserVisitor.GetModelByUserID(userInfoCore.UserID.Value);
                            _Login(new Auth_UserAccountMate
                            {
                                ApplicationID = CurrentApplicationID,
                                State = 1,
                                UserID = userInfoCore.UserID.Value,
                                UserName = userAccount.UserName,
                                CreateTime = userAccount.CreateTime == null ? DateTime.Now : userAccount.CreateTime.Value,
                                LastLoginTime = userAccount.LastLoginTime == null ? DateTime.Now : userAccount.LastLoginTime.Value,
                                AvatarURL = string.IsNullOrEmpty(userInfoCore.AvatarUrl) ? Url.Content("~/content/images/men_head.gif") : userInfoCore.AvatarUrl,
                                NicName = userInfoCore.NicName,
                                UserPoint = userInfoCore.UserPoint.HasValue ? userInfoCore.UserPoint.Value : 0,
                                UserSpecialID = userInfoCore.UserSpecialID,
                                QQOpenID = qzone.OAuthToken.OpenId,
                                QQAcessToken = qzone.OAuthToken.AccessToken
                            }, false);
                            Lib.Utils.LogHelper.CustomLog("写入cookie成功,," + st.ElapsedMilliseconds + "\r\n");
                            //return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/UserCenter/usercenter|用户中心;/home/index|首页"));
                            return Redirect("/home/index");
                        }
                    }
                    catch (QConnectSDK.Exceptions.QzoneException qe)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("当前异常的消息         : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        sb.AppendLine("当前异常的消息         : " + qe.Message);
                        if (qe.TargetSite.DeclaringType != null)
                            sb.AppendLine("引发当前异常的类名     : " + qe.TargetSite.DeclaringType.FullName);
                        if (!string.IsNullOrEmpty(qe.TargetSite.Name))
                            sb.AppendLine("引发当前异常的方法名   : " + qe.TargetSite.Name);
                        sb.AppendLine("堆栈信息               : " + qe.StackTrace);
                        sb.AppendLine("\r\n其他               : " + qe.Response.ErrorMessage);
                        sb.AppendLine("\r\nStatusCode               : " + qe.StatusCode);
                        sb.AppendLine("\r\n");

                        Lib.Utils.LogHelper.CustomLog(sb.ToString(), HttpContext.Server.MapPath("~/ErrorLog/"));
                        foreach (var item in BLL.BLLSystemConfig.ExceptionToEmails.Split(';'))
                        {
                            if (string.IsNullOrEmpty(item)) continue;
                            Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                            item,
                            "异常邮件", sb.ToString(), false, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Lib.Utils.LogHelper.CustomLog("\r\n");
                        Lib.Utils.LogHelper.CustomLog("当前异常的消息         : " + ex.Message);
                        if (ex.TargetSite.DeclaringType != null)
                            Lib.Utils.LogHelper.CustomLog("引发当前异常的类名     : " + ex.TargetSite.DeclaringType.FullName);
                        if (!string.IsNullOrEmpty(ex.TargetSite.Name))
                            Lib.Utils.LogHelper.CustomLog("引发当前异常的方法名   : " + ex.TargetSite.Name);
                        Lib.Utils.LogHelper.CustomLog("堆栈信息               : " + ex.StackTrace);
                        Lib.Utils.LogHelper.CustomLog("\r\n");
                        throw ex;
                    }
                    //SetAuthCookie(qzone.OAuthToken.OpenId, isPersistentCookie, friendlyName);

                    return Redirect(Url.Action("Index", "Home"));
                }

            }
            return View();

        }
        [HttpPost]
        public ActionResult QQRegister(FormCollection form, QQWeiboRegisterModel model)
        {

            var hiddentab = form["hiddentab"];
            if (hiddentab == "1")
            {
                #region 新注册用户
                if (BLLBadWordsFilter.HasBadWord(form["UserName"]))
                {
                    ModelState.AddModelError("", "用户名有敏感词汇。");
                    return View();
                }
                if (ModelState.IsValid)
                {
                    var ExistsUser = UserVisitor.Exists("UserName", model.UserName);
                    if (!ExistsUser)
                    {
                        #region 添加新用户
                        QOpenClient qqClient = Session["QOpenClient"] as QOpenClient;
                        var user = qqClient.GetCurrentUser();
                        var userid = 0;
                        #region 添加用户
                        userid = addUser(new RegisterModel
                        {
                            UserName = model.UserName,
                            Password = model.Password,
                            Email = model.Email
                        }, userid, qqClient.OAuthToken.OpenId, qqClient.OAuthToken.AccessToken, "", user.Gender);
                        #endregion

                        _Login(new Auth_UserAccountMate
                        {
                            ApplicationID = CurrentApplicationID,
                            State = 1,
                            UserID = userid,
                            UserName = model.UserName,
                            CreateTime = DateTime.Now,
                            AvatarURL = Url.Content("~/content/images/men_head.gif"),
                            LastLoginTime = DateTime.Now,
                            NicName = model.UserName,
                            UserPoint = 0,
                            UserSpecialID = "",
                            QQOpenID = qqClient.OAuthToken.OpenId,
                            QQAcessToken = qqClient.OAuthToken.AccessToken
                        }, false);
                        #endregion
                        //return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/usercenter/usercenter|用户中心;/home/index|首页"));
                        return Redirect("/home/index");
                    }
                    else
                    {
                        ViewBag.currentSpan = "1";
                        ModelState.AddModelError("", "该用户已存在");
                    }
                }
                #endregion
            }
            else if (hiddentab == "2")
            {
                #region 绑定
                UserManager.Framework.Model.Auth_UserAccount User = UserVisitor.ValidUserLogin(form["BandingUserName"].Trim(), form["BandingPassword"].Trim());
                if (User != null)
                {
                    UserManager.Framework.Model.Auth_UserInfoCore info = null;
                    QOpenClient qqClient = Session["QOpenClient"] as QOpenClient;
                    var user = qqClient.GetCurrentUser();
                    using (var entity = new UserManager.Framework.Model.UserManagerExamEntities())
                    {
                        info = entity.Auth_UserInfoCore.Where(x => x.UserID == User.UserID).First();
                        if (!string.IsNullOrEmpty(info.QQKey))
                        {
                            ViewBag.currentSpan = "2";
                            ModelState.AddModelError("", "该用户已经绑定了QQ。");
                            return View(model);
                        }
                        else
                        {
                            info.QQKey = qqClient.OAuthToken.OpenId;
                            info.QQAcessToken = qqClient.OAuthToken.AccessToken;
                            entity.SaveChanges();
                        }
                    }

                    _Login(new Auth_UserAccountMate
                    {
                        ApplicationID = CurrentApplicationID,
                        State = 1,
                        UserID = User.UserID,
                        UserName = User.UserName,
                        CreateTime = User.CreateTime == null ? DateTime.Now : User.CreateTime.Value,
                        LastLoginTime = User.LastLoginTime == null ? DateTime.Now : User.LastLoginTime.Value,
                        AvatarURL = string.IsNullOrEmpty(info.AvatarUrl) ? Url.Content("~/content/images/men_head.gif") : info.AvatarUrl,
                        NicName = info.NicName,
                        UserPoint = info.UserPoint.HasValue ? info.UserPoint.Value : 0,
                        UserSpecialID = info.UserSpecialID,
                        QQOpenID = qqClient.OAuthToken.OpenId,
                        QQAcessToken = qqClient.OAuthToken.AccessToken
                    }, false);

                    //return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/UserCenter/usercenter|用户中心;/home/index|首页"));
                    return Redirect("/home/index");
                }
                else
                {
                    ViewBag.currentSpan = "2";
                    ModelState.AddModelError("", "提供的用户名或密码不正确。");
                }
                #endregion
            }
            return View(model);
        }

        #region 注册
        public ActionResult Register()
        {
            return View();
        }
        //public ActionResult QQRegister()
        //{

        //    return View(new QQWeiboRegisterModel());
        //}

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (TempData["ValidateImageCode"] == null || TempData["ValidateImageCode"].ToString() != Request.Form["imgcode"])
            {
                ModelState.AddModelError("", "验证码输入错误。");
                return View(model);
            }
            if (BLLBadWordsFilter.HasBadWord(model.UserName))
            {
                ModelState.AddModelError("", "用户名有敏感词汇。");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                // 尝试注册用户
                var ExistsUser = UserVisitor.Exists("UserName", model.UserName);

                if (!ExistsUser)
                {
                    var userid = 0;
                    #region 添加用户
                    userid = addUser(model, userid);

                    #endregion

                    _Login(new Auth_UserAccountMate
                    {
                        ApplicationID = CurrentApplicationID,
                        State = 1,
                        UserID = userid,
                        UserName = model.UserName,
                        CreateTime = DateTime.Now,
                        AvatarURL = Url.Content("~/content/images/men_head.gif"),
                        LastLoginTime = DateTime.Now,
                        NicName = model.UserName,
                        UserPoint = 0,
                        UserSpecialID = ""
                    }, false);
                    return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/usercenter/usercenter|用户中心;/home/index|首页"));
                }
                else
                {
                    ModelState.AddModelError("", "该用户已存在");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        private int addUser(RegisterModel model, int userid, string QQkey = "", string QQAcessToken = "", string weibokey = "", string sex = "")
        {
            using (TransactionScope tc = new TransactionScope())
            {

                userid = UserManager.Framework.UserVisitor.InsertUserAccount(
                    new UserManager.Framework.Model.Auth_UserAccount
                    {
                        UserName = model.UserName,
                        Password = model.Password,
                        State = (int)(BLL.Enums.State.Normal),
                        CreateTime = DateTime.Now,
                        LastLoginTime = DateTime.Now,
                        ApplicationID = CurrentApplicationID,

                    },
                    new UserManager.Framework.Model.Auth_UserInfoCore
                    {
                        Email = model.Email,
                        NicName = string.IsNullOrEmpty(model.NicName) ? model.UserName : model.NicName,
                        LastModifyTime = DateTime.Now,
                        QQKey = QQkey,
                        WeiboKey = weibokey,
                        QQAcessToken = QQAcessToken,
                        WeiboAcessToken = ""
                    }
                    );
                Authorizations.Framework.AuthVisitor.InsertUserRole(userid, 2);
                tc.Complete();
            }
            return userid;
        }
        /// <summary>
        /// 写入cookie
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userid"></param>
        /// <param name="rememberMe"></param>
        private void _Login(Auth_UserAccountMate userAccountMate, bool rememberMe)
        {
            //初始化全局用户信息, 使用session保存
            //BLL.UserInfo.UserSystemModel = new UserSystemModel
            var tempUser = new UserSystemModel
            {
                UserAccount = userAccountMate,
                CurrentUserResources = AuthVisitor.GetCurrentUserResourceCode(userAccountMate.UserID, CurrentApplicationID),
                IsSuper = false
            };

            UserManager.Framework.UserVisitor.UpdateState(userAccountMate.UserID.ToString(), "lastlogintime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                userAccountMate.UserName,
                DateTime.Now,
                rememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddMinutes(FormsAuthentication.Timeout.Minutes),
                true,
                tempUser.ToJson<UserSystemModel>());//票据

            string cookiestr = FormsAuthentication.Encrypt(ticket);//加密
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);
            if (rememberMe)
                cookie.Expires = DateTime.Now.AddDays(cookietimeout);
            Response.Cookies.Add(cookie);//写入客户端
        }
        #endregion

        #region 找回密码
        private static string body = @"
            您好. 这里是塔罗来了www.taluolaile.com<br>
            这是您修改密码的链接, 请在30分钟内修改密码 <a href='{0}' target='_blank'>{0}</a><br>
            <br>
            请勿回复, 回复我也不会看也不会给您钱.
            <br>祝您天天开心 
        ";

        public ActionResult findPassword()
        {
            return View();
        }

        static string key = "findPWD@";
        [HttpPost]
        public ActionResult findPassword(FormCollection form)
        {
            if (TempData["ValidateImageCode"] == null || TempData["ValidateImageCode"].ToString() != Request.Form["imgcode"])
            {
                ModelState.AddModelError("", "验证码输入错误。");
                return View();
            }

            using (var entity = new UserManager.Framework.Model.UserManagerExamEntities())
            {
                string name = form["txtUserName"];
                var info = entity.Auth_UserAccount.Join(entity.Auth_UserInfoCore, x => x.UserID, y => y.UserID, (x, y) => new { y.Email, x.UserName, x.UserID }).
                    Where(x => x.UserName == name).FirstOrDefault();
                if (info == null)
                {
                    ModelState.AddModelError("", "该用户名不存在");
                }
                else
                {
                    try
                    {
                        string ip = Lib.Utils.Security.Encrypt(Request.UserHostAddress.Replace(".", "b"), key);
                        string urlename = Lib.Utils.Security.Encrypt(HttpUtility.UrlEncode(info.UserName), key);
                        string time = Lib.Utils.Security.Encrypt(DateTime.Now.AddMinutes(30).Ticks.ToString(), key);
                        string userid = Lib.Utils.Security.Encrypt(info.UserID.ToString(), key);
                        string temp = string.Format("p1={0}&p2={1}&p3={2}&p4={3}", urlename, ip, time, userid);

                        Lib.Utils.MailHelper.SendEmail("smtp.exmail.qq.com", "TaluoSystem@taluolaile.com", "TaluoSystem76568091",
                    info.Email,
                    "塔罗来了-修改密码", string.Format(body, "http://" + Request.Url.Host + "/account/changepassword?" + temp), true);
                    }
                    catch
                    {

                    }
                    ViewBag.Message = string.Format("已经发送到{0}. 请查收", info.Email.Replace(info.Email.Substring(0, info.Email.IndexOf("@") - 1), "*"));
                }
            }
            return View();
        }
        public ActionResult ChangePassword()
        {

            if (Request.QueryString.AllKeys.Count() != 4) return Content("");
            string name = HttpUtility.UrlDecode(Lib.Utils.Security.Decrypt(Request.QueryString[0], key));
            string ip = Lib.Utils.Security.Decrypt(Request.QueryString[1], key);
            string time = Lib.Utils.Security.Decrypt(Request.QueryString[2], key);
            if (ip != Request.UserHostAddress.Replace(".", "b")) return Content("ip error");
            long dt;
            if (long.TryParse(time, out dt))
            {
                if (DateTime.Now.Ticks > dt)
                    return Content("已过期");
            }
            ViewBag.UserName = name;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection form)
        {
            if (Request.QueryString.AllKeys.Count() != 4) return Content("");
            string name = HttpUtility.UrlDecode(Lib.Utils.Security.Decrypt(Request.QueryString[0], key));
            string ip = Lib.Utils.Security.Decrypt(Request.QueryString[1], key);
            string time = Lib.Utils.Security.Decrypt(Request.QueryString[2], key);
            string userid = Lib.Utils.Security.Decrypt(Request.QueryString[3], key);

            if (ip != Request.UserHostAddress.Replace(".", "b")) return Content("ip error");
            long dt;
            if (long.TryParse(time, out dt))
            {
                if (DateTime.Now.Ticks > dt)
                    return Content("已过期");
            }

            if (form["p1"].Length > 20 || form["p2"] != form["p1"])
            {
                ModelState.AddModelError("", "密码过长或者两次输入不一样");
                return View();
            }
            UserVisitor.UpdateState(userid, "password", form["p1"].ToMd5());
            ModelState.AddModelError("", "修改成功!!");
            return View();
        }
        #endregion
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入不同的用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入不同的电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }
        #endregion
    }
}
