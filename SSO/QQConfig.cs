using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace SSO
{
    /// <summary>
    /// QQ互联的配置数据
    /// </summary>
    [Serializable]
    internal class QQConnectConfig
    {
        public static readonly string ApiBaseUrl = "https://graph.qq.com/";
        private static NameValueCollection QzoneSection = (NameValueCollection)ConfigurationManager.GetSection("QQSectionGroup/QzoneSection");
        public static readonly string scope = "get_user_info,add_share ,check_page_fans ,add_t ,del_t ,add_pic_t ,get_repost_list ,get_info,get_other_info,get_fanslist,get_idolist,add_idol,del_idol";
        

        /// <summary>
        /// 获取Authorization Code的URL地址
        /// </summary>
        /// <returns></returns>
        public static string GetAuthorizeURL()
        {
            return QzoneSection["AuthorizeURL"];
        }
        /// <summary>
        /// 申请QQ登录成功后，分配给应用的appid
        /// </summary>
        /// <returns>string AppKey</returns>
        public static string GetAppKey()
        {
            return QzoneSection["AppKey"];
        }

        /// <summary>
        /// 申请QQ登录成功后，分配给网站的appkey
        /// </summary>
        /// <returns>string AppSecret</returns>
        public static string GetAppSecret()
        {
            return QzoneSection["AppSecret"];
        }

        /// <summary>
        /// 得到回调地址
        /// </summary>
        /// <returns></returns>
        public static Uri GetCallBackURI()
        {
            return new Uri(QzoneSection["CallBackURI"]);
        }
    }
}
