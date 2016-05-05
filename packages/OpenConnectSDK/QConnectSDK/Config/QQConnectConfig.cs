using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace QConnectSDK.Config
{
    /// <summary>
    /// QQ互联的配置数据
    /// </summary>
    [Serializable]
    public class QQConnectConfig
    {
        private NameValueCollection QzoneSection = (NameValueCollection)ConfigurationManager.GetSection("QQSectionGroup/QzoneSection");

        /// <summary>
        /// 获取Authorization Code的URL地址
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizeURL()
        {
            return QzoneSection["AuthorizeURL"];
        }
        /// <summary>
        /// 申请QQ登录成功后，分配给应用的appid
        /// </summary>
        /// <returns>string AppKey</returns>
        public string GetAppKey()
        {
            return QzoneSection["AppKey"];
        }

        /// <summary>
        /// 申请QQ登录成功后，分配给网站的appkey
        /// </summary>
        /// <returns>string AppSecret</returns>
        public string GetAppSecret()
        {
            return QzoneSection["AppSecret"];
        }

        /// <summary>
        /// 得到回调地址
        /// </summary>
        /// <returns></returns>
        public Uri GetCallBackURI()
        {
            return new Uri(QzoneSection["CallBackURI"]);
        }
    }
}
