using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Data.Framework.ModelMate;
using Data.Framework.Model;

using Lib.Utils;
using Lib.ClassExt;
using Lib.BaseClass;
using BLL.Enums;
using MongoDB.Driver.Builders;
using Data.MongoFramework.ModelMate;
using MongoDB.Bson;
using Lib.Data;
using Data.MongoFramework;

namespace Tarot.Controllers
{
    public class PostListController : BaseController
    {

        public ActionResult BBSList(string id) {
            var pidx = id.ToInt(1);
            var psize = 25;
            int count = 0;
            var lst = BLLPost.GetBBSList(new PagerInfo { Page = pidx, PageSize = psize }, out count);
            ViewBag.PageCount = ((count-1) / psize)+1;
            ViewBag.CurrentPage = pidx;
            return View(lst);
        }

        public ActionResult NewPostList(string id)
        {
            var pidx = id.ToInt(1);
            var psize = 10;
            int count = 0;
            var lst = BLLPost.GetNewPostList(new PagerInfo { Page = pidx, PageSize = psize }, out count);
            ViewBag.PageCount = ((count - 1) / psize) + 1;
            ViewBag.CurrentPage = pidx;
            return View(lst);
        }

        public ActionResult HotPostList(string id)
        {
            var pidx = id.ToInt(1);
            var psize = 10;
            int count = 0;
            var lst = BLLPost.GetNewPostList(new PagerInfo { Page = pidx, PageSize = psize }, out count,"AllTakePartIn");
            ViewBag.PageCount = ((count - 1) / psize) + 1;
            ViewBag.CurrentPage = pidx;
            return View(lst);
        }

        public ActionResult News(string id)
        {
            var lst = NewsMethod(id, Mpost_PostType.News);
            return View(lst);
        }

        private IList<M_Post> NewsMethod(string id, Mpost_PostType PostType)
        {
            var pidx = id.ToInt(1);
            var psize = 25;
            int count = 0;
            var lst = BLLPost.GetNewsList(new PagerInfo { Page = pidx, PageSize = psize }, out count,PostType);
            ViewBag.PageCount = ((count - 1) / psize) + 1;
            ViewBag.CurrentPage = pidx;
            return lst;
        }

        public ActionResult articleList(string id)
        {
            var lst = NewsMethod(id, Mpost_PostType.ArticleList);
            return View("News",lst);
        }
    }
}
