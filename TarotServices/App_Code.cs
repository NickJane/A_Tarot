using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TarotServices
{
    public class Common
    {
        ///// <summary>
        ///// 回复模板
        ///// </summary>
        //public static string ResponseXML =
        //    "<?xml version=\"1.0\" encoding=\"utf-8\" ?><Response><Code>{0}</Code><Content>{1}</Content></Response>";

        private static string ip = Lib.Utils.ConfigHelper.GetAppSetting("ValidIps");
        public static bool ValidIp(string ip) {
            return ip.Split(';').Contains(ip);     
        }
    }
}