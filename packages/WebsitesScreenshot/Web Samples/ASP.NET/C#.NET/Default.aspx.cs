/*========================================================
 * Copyright © WebsitesScreenshot.com, All rights reserved.
 *========================================================

 * Thank you for evaluating Websites Screenshot DLL.
 
 * Your evaluation is backed by full technical support.
 * Here are your support contacts:
 * Help: http://www.websitesscreenshot.com/Usage.html
 * Email: support@websitesscreenshot.com
 
 * NOTE: Unlike the full version, the evaluation copy produces image 
   with gray CROSS WATERMARK, if you wish to use our product without watermark limitation you must purchase a full version. 

 * Purchase: http://www.websitesscreenshot.com/Purchase.html

 * Do you want to suggest new feature for the future version?
 * Did you find any bug in our product?
 * Do you want us to customize this dll for your project?
 * Please let us know if you have any other questions or need any additional information.

 * Thank you for giving us the opportunity to work with you.
 * We look forward to hearing from you.

 * Regards,
 * WebsitesScreenshot.com
 *=============================================================*/
using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{

    protected void Button1_Click(object sender, EventArgs e)
    {
        WebsitesScreenshot.WebsitesScreenshot _WebsitesScreenshot = new WebsitesScreenshot.WebsitesScreenshot();
        WebsitesScreenshot.WebsitesScreenshot.Result _Result ;
        string path = null;
        path = Server.MapPath(".");
        _WebsitesScreenshot.ImageHeight = 300;
        _WebsitesScreenshot.ImageWidth = 200;
        _Result = _WebsitesScreenshot.CaptureWebpage("http://www.WebsitesScreenshot.com");
        if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Captured)
        {
            _WebsitesScreenshot.SaveImage(path + "\\WebsitesScreenshot.jpg");
            Response.Write("");
            Response.Write("<img src=WebsitesScreenshot.jpg>");
        }
        else if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Failed)
        {
            Response.Write("");
            Response.Write("Failed");
        }
        else if (_Result == WebsitesScreenshot.WebsitesScreenshot.Result.Timeout)
        {
            Response.Write("");
            Response.Write("Timeout");
        }
        _WebsitesScreenshot.Dispose();
    }
}
