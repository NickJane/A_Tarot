using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using Data.MongoFramework.ModelMate;

namespace BLL.ActionFilters
{
    public class BadWordFilter : ActionFilterAttribute
    {
        
        /// <summary>
        /// 搞定所有的脏字过滤
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.RouteData.Values["action"].ToString().ToLower() == "savecomment") {
                var comment = filterContext.ActionParameters["model"] as Comment;
                comment.Content = BLL.BLLBadWordsFilter.Filter(comment.Content);
                comment.Content = comment.Content.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace(" ", "&nbsp;");
            }
            if (filterContext.RouteData.Values["action"].ToString().ToLower() == "posting")
            {
                //塔罗牌的帖子
                var model = filterContext.ActionParameters["post"] as M_Post;
                if (BLLBadWordsFilter.HasBadWord(model.Title))
                {
                    filterContext.Controller.ValidateRequest = false;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", "标题中不能有敏感词");
                }
                model.Content = BLL.BLLBadWordsFilter.Filter(model.Content);
                //model.Title = BLL.BLLBadWordsFilter.Filter(model.Title);
                
                var tags = filterContext.RequestContext.HttpContext.Request["CustomerTag"]; tags = tags ?? "";
                List<CustomerTag> newtags = new List<CustomerTag>();
                foreach (var item in tags.Split(' '))
                {
                    if (!BLL.BLLBadWordsFilter.HasBadWord(item) && !string.IsNullOrEmpty(item))
                        newtags.Add(new CustomerTag { TagName = item });
                }
                model.CustomerTags = newtags.ToArray();
                model.Content = model.Content.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace(" ", "&nbsp;");
            }
            if (filterContext.RouteData.Values["action"].ToString().ToLower() == "posting2")
            {
                //普通的帖子
                var model = filterContext.ActionParameters["post"] as M_Post;
                if (BLLBadWordsFilter.HasBadWord(model.Title))
                {
                    filterContext.Controller.ValidateRequest = false;
                    filterContext.Controller.ViewData.ModelState.AddModelError("", "标题中不能有敏感词");
                }
                model.Content = BLL.BLLBadWordsFilter.Filter(model.Content);

                model.Content = model.Content.Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>").Replace(" ", "&nbsp;");
            }
        }

    }
}
