using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.MongoFramework.ModelMate
{
    /// <summary>
    /// OperateLog:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class LogOperate
    {
        public LogOperate()
        { }
        #region Model
        #region
        private MongoDB.Bson.ObjectId _id;
        private int _logtype;
        private string _username;
        private string _pagename;
        private string _modulename;
        private string _comment;
        private DateTime _createtime;
        private int? _userid;
        private string _operateobjectid;
        private int _applicationid;
        private string _clientip;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public MongoDB.Bson.ObjectId Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// LogType. 枚举
        /// </summary>
        public int LogType
        {
            set { _logtype = value; }
            get { return _logtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PageName
        {
            set { _pagename = value; }
            get { return _pagename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ModuleName
        {
            set { _modulename = value; }
            get { return _modulename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Comment
        {
            set { _comment = value; }
            get { return _comment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 操作者. 如果为操作日志, 则由操作者. 其他如系统日志, 该项就为null
        /// </summary>
        public int? UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OperateObjectId
        {
            set { _operateobjectid = value; }
            get { return _operateobjectid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ApplicationId
        {
            set { _applicationid = value; }
            get { return _applicationid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ClientIP
        {
            set { _clientip = value; }
            get { return _clientip; }
        }
        #endregion Model

    }
}
