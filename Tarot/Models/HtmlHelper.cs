using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tarot.Models
{
    public static class htmlPager
    {
        public static MvcHtmlString myPager(this System.Web.Mvc.HtmlHelper html, int totalPage, int currentPage,
             Func<int, string> actionUrls)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            #region “前一页”
            TagBuilder tgPrev = new TagBuilder("a");
            tgPrev.InnerHtml = "前一页";

            if (currentPage != 1 && totalPage > 1)
            {
                tgPrev.MergeAttribute("href", actionUrls(1));
            }
            sb.Append(tgPrev.ToString());
            #endregion

            if (totalPage <= 10)
            {
                for (int i = 1; i <= totalPage; i++)
                {
                    TagBuilder tg = new TagBuilder("a");
                    tg.MergeAttribute("href", actionUrls(i));
                    tg.InnerHtml = (i.ToString());

                    if (i == currentPage)
                    {
                        tg.AddCssClass("current");
                    }
                    sb.Append(tg.ToString());
                }
            }
            else
            {//大于10页
                if (currentPage < 7)
                {
                    for (int i = 1; i <= totalPage; i++)
                    {
                        TagBuilder tg = new TagBuilder("a");
                        if (i < 8)
                        {
                            tg.MergeAttribute("href", actionUrls(i));
                            tg.InnerHtml = (i.ToString());
                        }
                        else if (i == 8)
                            tg.InnerHtml = "……";
                        else
                        {
                            if (i > (totalPage - 2))
                            {
                                tg.MergeAttribute("href", actionUrls(i));
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        if (i == currentPage)
                        {
                            tg.AddCssClass("current");
                        }
                        if (tg.InnerHtml != "")
                            sb.Append(tg.ToString());
                    }
                }
                else if (currentPage >= 7)
                {
                    for (int i = 1; i <= totalPage; i++)
                    {
                        TagBuilder tg = new TagBuilder("a");
                        if (i < 3)
                        {
                            tg.MergeAttribute("href", actionUrls(i));
                            tg.InnerHtml = (i.ToString());
                        }
                        if (i == 3)
                            tg.InnerHtml = "……";
                        if ((totalPage - currentPage) > 6)
                        {
                            if ((totalPage - currentPage) > 5)
                            {
                                if ((currentPage - i) <= 2 && (currentPage - i) >= -3)
                                {
                                    tg.MergeAttribute("href", actionUrls(i));
                                    tg.InnerHtml = (i.ToString());
                                }
                                if ((currentPage - i) == -4)
                                    tg.InnerHtml = "……";
                                if ((totalPage - i) <= 2)
                                {
                                    tg.MergeAttribute("href", actionUrls(i));
                                    tg.InnerHtml = (i.ToString());
                                }
                            }
                            else if ((currentPage - i) < 2)
                            {
                                tg.MergeAttribute("href", actionUrls(i));
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        else
                        {
                            if ((currentPage - i) < 2 || (totalPage - i) < 7)
                            {
                                tg.MergeAttribute("href", actionUrls(i));
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        if (i == currentPage)
                            tg.AddCssClass("current");
                        if (tg.InnerHtml != "")
                            sb.Append(tg.ToString());
                    }
                }
            }

            #region “后一页”
            TagBuilder tgNext = new TagBuilder("a");
            tgNext.InnerHtml = "后一页";

            if (currentPage != totalPage && totalPage > 1)
            {
                tgNext.MergeAttribute("href", actionUrls(currentPage + 1));
            }
            sb.Append(tgNext.ToString());
            #endregion

            #region 搜索
            //TagBuilder tbTxt = new TagBuilder("input");
            //tbTxt.MergeAttribute("value", currentPage.ToString());
            //tbTxt.MergeAttribute("id", "_Ext_txtmyPageIndex");
            //tbTxt.MergeAttribute("name", "_Ext_txtmyPageIndex");
            //tbTxt.MergeAttribute("style", "width:50px;");
            //sb.Append(tbTxt.ToString());

            //TagBuilder tbHidden_TotalPages = new TagBuilder("input");
            //tbHidden_TotalPages.MergeAttribute("value", totalPage.ToString());
            //tbHidden_TotalPages.MergeAttribute("id", "_Ext_txtmyTotalPage");
            //tbHidden_TotalPages.MergeAttribute("name", "_Ext_txtmyTotalPage");
            //tbHidden_TotalPages.MergeAttribute("type", "hidden");
            //sb.Append(tbHidden_TotalPages.ToString());

            //TagBuilder tbGo = new TagBuilder("input");
            //tbGo.MergeAttribute("type", "submit");
            //tbGo.MergeAttribute("value", "GO");
            //sb.Append(tbGo.ToString());
            #endregion

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString myPager(this System.Web.Mvc.HtmlHelper html, int totalPage, object recordCount, int currentPage,
             Func<int, string> actionUrls, string queryString)
        {

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            TagBuilder tgrecordCount = new TagBuilder("span");
            tgrecordCount.InnerHtml = string.Format("共{0}条记录&nbsp;&nbsp;", recordCount);

            sb.Append(tgrecordCount.ToString());

            #region “前一页”
            TagBuilder tgPrev = new TagBuilder("a");
            tgPrev.InnerHtml = "前一页";

            if (currentPage != 1 && totalPage > 1)
            {
                tgPrev.MergeAttribute("href", actionUrls(1) + queryString);
            }
            sb.Append(tgPrev.ToString());
            #endregion


            if (totalPage <= 10)
            {
                for (int i = 1; i <= totalPage; i++)
                {
                    TagBuilder tg = new TagBuilder("a");
                    tg.MergeAttribute("href", actionUrls(i) + queryString);
                    tg.InnerHtml = (i.ToString());

                    if (i == currentPage)
                    {
                        tg.AddCssClass("selected");
                    }
                    sb.Append(tg.ToString());
                }
                if (totalPage <= 0)
                {
                    TagBuilder tg = new TagBuilder("a");
                    tg.InnerHtml = "1";
                    tg.AddCssClass("selected");
                    sb.Append(tg.ToString());
                }
            }
            else
            {//大于10页
                if (currentPage < 7)
                {
                    for (int i = 1; i <= totalPage; i++)
                    {
                        TagBuilder tg = new TagBuilder("a");
                        if (i < 8)
                        {
                            tg.MergeAttribute("href", actionUrls(i) + queryString);
                            tg.InnerHtml = (i.ToString());
                        }
                        else if (i == 8)
                            tg.InnerHtml = "……";
                        else
                        {
                            if (i > (totalPage - 2))
                            {
                                tg.MergeAttribute("href", actionUrls(i) + queryString);
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        if (i == currentPage)
                        {
                            tg.AddCssClass("selected");
                        }
                        if (tg.InnerHtml != "")
                            sb.Append(tg.ToString());
                    }
                }
                else if (currentPage >= 7)
                {
                    for (int i = 1; i <= totalPage; i++)
                    {
                        TagBuilder tg = new TagBuilder("a");
                        if (i < 3)
                        {
                            tg.MergeAttribute("href", actionUrls(i) + queryString);
                            tg.InnerHtml = (i.ToString());
                        }
                        if (i == 3)
                            tg.InnerHtml = "……";
                        if ((totalPage - currentPage) > 6)
                        {
                            if ((totalPage - currentPage) > 5)
                            {
                                if ((currentPage - i) <= 2 && (currentPage - i) >= -3)
                                {
                                    tg.MergeAttribute("href", actionUrls(i) + queryString);
                                    tg.InnerHtml = (i.ToString());
                                }
                                if ((currentPage - i) == -4)
                                    tg.InnerHtml = "……";
                                if ((totalPage - i) <= 2)
                                {
                                    tg.MergeAttribute("href", actionUrls(i) + queryString);
                                    tg.InnerHtml = (i.ToString());
                                }
                            }
                            else if ((currentPage - i) < 2)
                            {
                                tg.MergeAttribute("href", actionUrls(i) + queryString);
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        else
                        {
                            if ((currentPage - i) < 2 || (totalPage - i) < 7)
                            {
                                tg.MergeAttribute("href", actionUrls(i) + queryString);
                                tg.InnerHtml = (i.ToString());
                            }
                        }
                        if (i == currentPage)
                            tg.AddCssClass("selected");
                        if (tg.InnerHtml != "")
                            sb.Append(tg.ToString());
                    }
                }
            }

            #region “后一页”
            TagBuilder tgNext = new TagBuilder("a");
            tgNext.InnerHtml = "后一页";

            if (currentPage != totalPage && totalPage > 1)
            {
                tgNext.MergeAttribute("href", actionUrls(currentPage + 1) + queryString);
            }
            sb.Append(tgNext.ToString());
            #endregion

            #region 搜索
            TagBuilder tbTxt = new TagBuilder("input");
            tbTxt.MergeAttribute("value", currentPage.ToString());
            tbTxt.MergeAttribute("id", "_Ext_txtmyPageIndex");
            tbTxt.MergeAttribute("name", "_Ext_txtmyPageIndex");
            tbTxt.MergeAttribute("style", "width:50px;");
            sb.Append(tbTxt.ToString());

            TagBuilder tbHidden_TotalPages = new TagBuilder("input");
            tbHidden_TotalPages.MergeAttribute("value", totalPage.ToString());
            tbHidden_TotalPages.MergeAttribute("id", "_Ext_txtmyTotalPage");
            tbHidden_TotalPages.MergeAttribute("name", "_Ext_txtmyTotalPage");
            tbHidden_TotalPages.MergeAttribute("type", "hidden");
            sb.Append(tbHidden_TotalPages.ToString());

            TagBuilder tbGo = new TagBuilder("input");
            tbGo.MergeAttribute("type", "submit");
            tbGo.MergeAttribute("value", "GO");
            sb.Append(tbGo.ToString());
            #endregion

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}