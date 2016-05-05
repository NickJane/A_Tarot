using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib;
using Lib.ClassExt;
namespace SSO
{
    public class QQConnect
    {
        /// <summary>
        /// access token和用户openid
        /// </summary>
        public QOAuthModel oAuthModel { get; set; }
        /// <summary>
        /// 构造函数，用于用户接受授权后使用Authorization Code获取AccessToken. 再获得openid
        /// </summary>
        /// <param name="verifierCode">Authorization Code（注意此code会在10分钟内过期）。</param>
        /// <param name="state">client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回</param>
        public QQConnect(string verifierCode, string state)
        {
            DateTime dt = DateTime.Now;
            string url="";
            url= string.Format("oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&redirect_uri={3}&state={4}",
                QQConnectConfig.GetAppKey(), QQConnectConfig.GetAppSecret(), verifierCode, QQConnectConfig.GetCallBackURI(), state);
            Lib.Utils.LogHelper.CustomLog("开始任务 " + (DateTime.Now - dt).TotalMilliseconds + "\r\n");
            Lib.Utils.LogHelper.CustomLog("请求地址 " + QQConnectConfig.ApiBaseUrl + url + "\r\n");
            
            oAuthModel = GetUserAccessToken(Lib.Utils.WebRequestHelper.ReadUrl(QQConnectConfig.ApiBaseUrl + url));
            
            Lib.Utils.LogHelper.CustomLog("第一步, 获得AccessToken,耗时 " + (DateTime.Now - dt).TotalMilliseconds + "\r\n");

            url = "oauth2.0/me?access_token=" + oAuthModel.AccessToken;
            oAuthModel.OpenId = GetUserOpenId(Lib.Utils.WebRequestHelper.ReadUrl(QQConnectConfig.ApiBaseUrl + url));
            Lib.Utils.LogHelper.CustomLog("第二步, 获得openid,耗时 " + (DateTime.Now - dt).TotalMilliseconds + "\r\n");
            Lib.Utils.LogHelper.CustomLog(string.Format("tokenis  {0},  openid is {1}",oAuthModel.AccessToken,oAuthModel.OpenId) + "\r\n");
        }
        #region 为第一步获取AccessToken. 再获得openid 服务
        private string GetUserOpenId(string content)
        {
            string strJson = content.Replace("callback(", "").Replace(");", "");
            var payload = (strJson.ToObject<Callback>());
            return payload.openid;
        }
        private QOAuthModel GetUserAccessToken(string urlParams)
        {
            QOAuthModel token = new QOAuthModel();
            var parameters = urlParams.Split('&');
            foreach (var parameter in parameters)
            {
                var accessTokens = parameter.Split('=');
                if (accessTokens[0] == "access_token")
                {
                    token.AccessToken = accessTokens[1];

                }
            }
            return token;
        }
        #endregion

        /// <summary>
        /// 获取Authorization Code的URL地址
        /// </summary>
        /// <param name="state">client端的状态值。用于第三方应用防止CSRF攻击，成功授权后回调时会原样带回。</param>
        /// <param name="scope">请求用户授权时向用户显示的可进行授权的列表。可填写的值是【QQ登录】API文档中列出的接口，
        /// 以及一些动作型的授权（目前仅有：do_like），如果要填写多个接口名称，请用逗号隔开。
        /// 例如：scope=get_user_info,add_share,list_album,upload_pic,check_page_fans,add_t,add_pic_t,del_t,get_repost_list,get_info,get_other_info 
        /// get_fanslist,get_idolist,add_idol,del_idol
        /// 不传则默认请求对接口get_user_info进行授权。
        /// 建议控制授权项的数量，只传入必要的接口名称，因为授权项越多，用户越可能拒绝进行任何授权。</param>
        /// <returns></returns>
        public static string Step1_GetAuthorizationUrl(string state)
        {
            string url = string.Empty;
            url = string.Format("{0}?response_type=code&client_id={1}&redirect_uri={2}&state={3}&scope={4}",
                QQConnectConfig.GetAuthorizeURL(), QQConnectConfig.GetAppKey(), QQConnectConfig.GetCallBackURI().ToString(), state, QQConnectConfig.scope);
            return url;
        }
    }
}
