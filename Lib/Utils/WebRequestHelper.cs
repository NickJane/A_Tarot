using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace Lib.Utils
{
    public class WebRequestHelper
    {
        /// <summary>
        /// 传回来请求的url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReadUrl(string url)
        {
            WebClient client = new WebClient();
            string json = "0";
            using (
                    System.IO.Stream stream = client.OpenRead(url)
                )
            {
                using (System.IO.StreamReader _read = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8))
                {
                    json = _read.ReadToEnd();
                }
            }
            return json;
        }
    }
}
