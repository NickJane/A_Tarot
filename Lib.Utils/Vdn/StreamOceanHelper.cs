using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Utils.Vdn
{
    public static class StreamOceanHelper
    {
        public enum ConentType
        {
            Vod,
            VirtualChannel,
            Live,
        }

        public enum Platform
        {
            Stb,
            Ipad,
            Pc
        }

        public enum Size
        {
            S1028p,
            S720p
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="platform"></param>
        /// <param name="contentId"></param>
        /// <param name="isRateAdapt"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public static string GetUrl(ConentType contentType, Platform platform, Guid contentId, bool isRateAdapt = false, string fmt = null)
        {
            StringBuilder sb = new StringBuilder(ConfigHelper.GetAppSetting("Itv.StreamOcean.VideoDomain." + contentType));

            sb.AppendFormat("{0}/", contentType.ToString().ToLowerInvariant())
                .AppendFormat(@"{0:yyyy\/MM\/dd\/}", DateTime.Today)
                .AppendFormat("{0:N}.ts", contentId);

            if (fmt == null)
            {
                switch (platform)
                {
                    case Platform.Pc:
                        sb.Append("?fmt=x264_0k_flv");
                        break;
                    case Platform.Stb:
                        sb.Append("?fmt=x264_0k_mpegts");
                        break;
                    case Platform.Ipad:
                        sb.Append(".m3u8?fmt=x264_0k_mpegts");
                        break;
                    default:
                        sb.Append("?fmt=x264_0k_mpegts");
                        break;
                }
            }
            else
            {
                sb.Append("?fmt=").Append(fmt);
            }

            if (isRateAdapt) sb.Append("&sora=1");

            return sb.ToString();
        }
    }
}
