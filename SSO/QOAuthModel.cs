using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSO
{
    public class QOAuthModel
    {
        /// <summary>
        /// access token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId { get; set; }
    }

    /// <summary>
    /// 根据access_token获得对应用户身份的openid
    /// </summary>
    internal class Callback
    {
        /// <summary>
        /// 
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string openid { get; set; }
    }
}
