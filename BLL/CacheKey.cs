using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public struct CacheKey
    {
        public static string TarotCards = "TarotCards";

        /// <summary>
        /// "MessageNewCounter_{UserID}"
        /// </summary>
        public static string MessageNewCounter = "MessageNewCounter_{0}";
    }
}
