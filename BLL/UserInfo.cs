using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.Utils;
using System.Web;
using System.Web.Security;
using Lib.ClassExt;
using UserManager.Framework.ModelMate;
namespace BLL
{
    public static class UserInfo
    {
        private static string SessionKey = "UserSystemModel";

        private static UserSystemModel anonymous = new UserSystemModel
        {
            CurrentUserResources = new string[] { },
            IsSuper = false,
            UserAccount = new Auth_UserAccountMate
            {
                ApplicationID = 2,
                AvatarURL = "~/content/images/men_head.gif",
                NicName="路人",
                UserID=-1
            }
        };

        /// <summary>
        /// 可写不可读
        /// </summary>
        public static UserSystemModel UserSystemModel
        {
            private get
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    var userdata = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
                    UserSystemModel obj = userdata.ToObject<UserSystemModel>();
                    return obj;
                }
                else
                    return anonymous;
            }
            set { 
                SessionHelper.SetValue(SessionKey, value); 
            }
        }

        /// <summary>
        /// 用户资料信息
        /// </summary>
        public static Auth_UserAccountMate UserAccount
        {
            get { return UserSystemModel.UserAccount; }
        }

        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public static bool IsSuper
        {
            get
            {
                if (UserSystemModel.IsSuper)
                    return true;
                else
                    return false;
            }
        }

        public static string[] CurrentUserResources
        {
            get { return UserSystemModel.CurrentUserResources; }
        }
    }
    /// <summary>
    /// 当前登录者的用户信息
    /// </summary>
    public class UserSystemModel
    {
        public Auth_UserAccountMate UserAccount { get; set; }

        public string[] CurrentUserResources { get; set; }

        public bool IsSuper { get; set; }
    }
}
