using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace UserManager.Framework.ModelMate
{

    
    public partial class Auth_UserAccountMate
    {
        public Auth_UserAccountMate()
        { }
        #region Model
        private int _userid;
        private string _username;
        private int _state;

        private DateTime _createtime;
        private DateTime _lastlogintime;

        public int ApplicationID { get; set; }

        private string _AvatarURL;
        public string AvatarURL
        {
            get
            {
                    return _AvatarURL;
            } set{_AvatarURL=value;} }
        public string NicName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        
        [Required(ErrorMessage="不能为空")]
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int State
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// 本地时间. 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return  _createtime; }
        }
        /// <summary>
        /// 本地时间. 
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get {
                return _lastlogintime; 
            }
        }

        public int UserPoint { get; set; }
        /// <summary>
        /// 多个ID用,分割
        /// </summary>
        public string UserSpecialID { get; set; }

        public string QQOpenID { get; set; }
        public string QQAcessToken { get; set; }

        #endregion Model

    }
}
