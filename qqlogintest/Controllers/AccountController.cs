using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using qqlogintest.Models;
using QConnectSDK.Context;
using System.Text;
using QConnectSDK;

namespace qqlogintest.Controllers
{
    public class AccountController : Controller
    {
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
                        StringBuilder sb = new StringBuilder();

                        sb.AppendLine("运行计时1,," + st.ElapsedMilliseconds + "\r\n");
                        qzone = new QOpenClient(verifier, state);
                        sb.AppendLine("获得AccessToken和openid耗时,," + st.ElapsedMilliseconds + "\r\n");
                        sb.AppendLine("开始请求QQ空间信息,,\r\n");
                        var currentUser = qzone.GetCurrentUser();
                        sb.AppendLine("结束请求QQ空间信息,,\r\n");
                        var friendlyName = currentUser.Nickname;
                        sb.AppendLine("性别" + currentUser.Gender + "  名称," + currentUser.Nickname + "\r\n");

                        ViewBag.sb = sb;
                        return View();
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
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
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

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // 在某些出错情况下，ChangePassword 将引发异常，
                // 而不是返回 false。
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "当前密码不正确或新密码无效。");
                }
            }

            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

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
