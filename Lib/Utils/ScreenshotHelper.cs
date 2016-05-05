using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class ScreenshotHelper
{
    /// <summary>
    /// 截屏服务 1成功, 2失败 3超时
    /// </summary>
    /// <param name="type"></param>
    public static int Run(string fullImgName,string ServicePath, int height, int width) {
        if (System.IO.File.Exists(fullImgName))
            return 1;

        WebsitesScreenshot.WebsitesScreenshot _WebsitesScreenshot = new WebsitesScreenshot.WebsitesScreenshot();
        WebsitesScreenshot.WebsitesScreenshot.Result _Result;

        _WebsitesScreenshot.ImageHeight = height;
        _WebsitesScreenshot.ImageWidth = width;
        _Result = _WebsitesScreenshot.CaptureWebpage(ServicePath);

        var ret = 0;
        if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Captured)
        {
            _WebsitesScreenshot.SaveImage(fullImgName);
            ret = 1;
        }
        else if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Failed)
        {
            ret= 2;
        }
        else if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Timeout)
        {
            ret= 3;
        }
        
        _WebsitesScreenshot.Dispose();
        return ret;
    }
}
