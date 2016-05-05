using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BLL.HttpHandler
{
    public class NoPicture : IHttpHandler
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此处理程序，
        /// 并向 IIS 注册此处理程序，然后才能进行使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.UrlReferrer==null || context.Request.UrlReferrer.Host.ToLower().Contains("taluo")
                || context.Request.UrlReferrer.Host.ToLower().Contains("share.v.t.qq.com")
                || context.Request.UrlReferrer.Host.ToLower().Contains("service.t.sina.com.cn")
                || context.Request.UrlReferrer.Host.ToLower().Contains("qzone.qq.com")
                || context.Request.UrlReferrer.Host.ToLower().Contains("share.renren.com")
                || context.Request.UrlReferrer.Host.ToLower().Contains("s.jiathis.com")
                )
            {
                
                //context.Response.CacheControl = "Public";
                //context.Response.AddHeader("Expires", "Thu, 12 Jan 2010 03:45:16 GMT"); 
                //context.Response.AddHeader("Last-Modified", "Thu, 12 Jan 2010 03:45:16 GMT");
                
                //设置输出文件类型
                context.Response.ContentType = "image/jpg";
                //将请求文件写入到输出缓存中
                if (System.IO.File.Exists(context.Request.PhysicalPath))
                {
                    SetClientCaching(context.Response, DateTime.Now);
                    context.Response.WriteFile(context.Request.PhysicalPath);
                }
                //将输出缓存中的信息传送到客户端
                context.Response.End();
            }
            //如果不是本地引用，则是盗链本站图片
            else
            {
                context.Response.End();
            }
        }
        private void SetFileCaching(HttpResponse response, string fileName)
       {
           response.AddFileDependency(fileName);
          //基于处理程序文件依赖项的时间戳设置 ETag HTTP 标头。 
          response.Cache.SetETagFromFileDependencies();
          //基于处理程序文件依赖项的时间戳设置 Last-Modified HTTP 标头。
          response.Cache.SetLastModifiedFromFileDependencies();
          response.Cache.SetCacheability(HttpCacheability.Public);
          //response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
          response.Cache.SetMaxAge(new TimeSpan(0, 0, 30, 0));
          response.Cache.SetSlidingExpiration(true);
      }
        private void SetClientCaching(HttpResponse response, DateTime lastModified)
        {
            response.Cache.SetETag(lastModified.Ticks.ToString());
            response.Cache.SetLastModified(lastModified);
            //public 以指定响应能由客户端和共享（代理）缓存进行缓存。
            response.Cache.SetCacheability(HttpCacheability.Public);
            //是允许文档在被视为陈旧之前存在的最长绝对时间。
            response.Cache.SetMaxAge(new TimeSpan(7, 0, 0, 0));
            //将缓存过期从绝对时间设置为可调时间
            response.Cache.SetSlidingExpiration(true);
        }
 

        #endregion
    }
}
