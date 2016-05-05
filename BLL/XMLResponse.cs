using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 用于代替进行通讯的类
    /// </summary>
    public class JsonResponse
    {
        public int State { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseText { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Enum_JsonResponse { 
        OK=1,
        /// <summary>
        /// 2
        /// </summary>
        ERROR=2
    }
}
