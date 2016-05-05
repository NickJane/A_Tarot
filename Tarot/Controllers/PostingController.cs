using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.ClassExt;
using Lib.BaseClass;
using Data.Framework.ModelMate;
using BLL;
using BLL.ActionFilters;
using Data.MongoFramework.ModelMate;
using Data.MongoFramework;
using MongoDB.Bson;
using Lib.Data;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using BLL.Enums;
namespace Tarot.Controllers
{
    public class PostingController : BaseController
    {
        //
        // GET: /Posting/

        public ActionResult Posting() { return View(); }
        public ActionResult Posting2() { return View(); }

        #region 发表, 塔罗牌, 普通帖
        [HttpPost]
        [PostingFilter]
        [BadWordFilter]
        public ActionResult Posting(FormCollection form, M_Post post)
        {
            if (ModelState.IsValid)
            {
                #region 帖子列表信息
                post.AuditState = true;//默认为false. 目前不需要审核
                if(User.Identity.IsAuthenticated)
                    post.m_User = new M_User { AvatarURL = UserInfo.UserAccount.AvatarURL, NicName = UserInfo.UserAccount.UserName, 
                                            UserID=UserInfo.UserAccount.UserID, UserPoint=UserAccount.UserPoint, UserSpecialID=UserAccount.UserSpecialID };
                else
                    post.m_User = new M_User { AvatarURL = "", NicName = "游客", UserID = -1 };

                //IList<CustomerTag> lstCustomerTag = new List<CustomerTag>();
                //foreach (var item in Request["CustomerTag"].Split(' '))
                //{
                //    if (!string.IsNullOrEmpty(item)) lstCustomerTag.Add(new CustomerTag { TagName = item });
                //}
                //post.CustomerTags = lstCustomerTag.ToArray();
                post.PostState = 1;
                post.PostTime = DateTime.Now;
                post.LastModifyTime=DateTime.Now;
                post.IP = Request.UserHostAddress;

                var cards = HttpUtility.UrlDecode(Request.Form["CardInfos"]);
                IList<CardInfo> lstCardInfo = new List<CardInfo>();
                foreach (var item in cards.Split('$'))
                {
                    if (!string.IsNullOrEmpty(item)) lstCardInfo.Add(item.ToObject<CardInfo>());
                }
                if(lstCardInfo.Count==0)
                {
                    ModelState.AddModelError("","系统错误, 为能找到任何牌阵");
                    return View(post);
                }
                post.CardFormationInfo.CardInfos = lstCardInfo.ToArray();
                post.CardFormationInfo.CardFormationUrl=Server.MapPath("~/Content/Images/PostsCardF/")+Guid.NewGuid().ToString()+".jpg";
                #endregion

                post._id= ObjectId.GenerateNewId();//mongoid
                post.Content = post.Content.Replace("[img]", "<img src='http://www.taluolaile.com/Content/ueditor/dialogs/emotion/images/jx2/")
                                        .Replace("[/img]", ".gif' />");
                //写入帖子表
                var safemodel = Lib.Data.MongoDBHelper.InsertOne<M_Post>(CollectionName.Posts7Days, post);

                foreach (var item in post.CustomerTags)
                {//如果不存在这个标签, 则插入
                    if (!MongoDBHelper.Exists(CollectionName.CustomerTag, Query.EQ("TagName", new BsonString(item.TagName))))
                    {
                        MongoDBHelper.InsertOne(CollectionName.CustomerTag, item);
                    }
                }
                /*截屏*/
                new System.Threading.Thread(() => {
                    ScreenshotHelper.Run(post.CardFormationInfo.CardFormationUrl,
                        Lib.Utils.ConfigHelper.GetAppSetting("TarotServices") + "ScreenShotPost.aspx?id=" + post._id, 
                        post.CardFormationInfo.H+35,post.CardFormationInfo.W);
                }).Start();
                return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/posting/PostDetail/" + post._id.ToString() + "|我刚刚发表帖子"));
            }
            return View(post);
        }

        [HttpPost]
        [PostingFilter]
        [BadWordFilter]
        public ActionResult Posting2(FormCollection form, M_Post post)
        {
            if (ModelState.IsValid)
            {
                post.AuditState = true;//默认为false. 目前不需要审核
                if (User.Identity.IsAuthenticated)
                    post.m_User = new M_User
                    {
                        AvatarURL = UserInfo.UserAccount.AvatarURL,
                        NicName = UserInfo.UserAccount.UserName,
                        UserID = UserInfo.UserAccount.UserID,
                        UserPoint = UserAccount.UserPoint,
                        UserSpecialID = UserAccount.UserSpecialID
                    };
                else
                    post.m_User = new M_User { AvatarURL = "", NicName = "游客", UserID = -1 };

                post.PostState = 1;
                post.PostTime = DateTime.Now;
                post.LastModifyTime = DateTime.Now;
                post.IP = Request.UserHostAddress;
                post.PostType = (int)BLL.Enums.Mpost_PostType.Usual;
                post._id = ObjectId.GenerateNewId();
                post.Content = post.Content.Replace("[img]", "<img src='http://www.taluolaile.com/Content/ueditor/dialogs/emotion/images/jx2/")
                                        .Replace("[/img]", ".gif' />");

                Lib.Data.MongoDBHelper.InsertOne<M_Post>(CollectionName.Posts7Days, post);
                return Redirect("/shared/redirectpage?urls=" + HttpUtility.UrlEncode("/posting/PostDetail2/" + post._id.ToString() + "|我刚刚发表帖子"));
            }
            return View(post);
        }
        #endregion
        

        public ActionResult PostDetail(string id) {
            var model = BLLPost.GetM_PostByID(id);
            if (model == null) { throw new HttpException(404,""); return new EmptyResult(); }
            BLLPost.AddPostPV(id);//增加浏览量
            return View(model);
        }

        public ActionResult PostDetail2(string id) {
            var model = BLLPost.GetM_PostByID(id);
            if (model == null) { throw new HttpException(404, ""); return new EmptyResult(); }
            BLLPost.AddPostPV(id);//增加浏览量
            return View(model);
        }

        public ActionResult newsdetail(string id) {
            var model = BLLPost.GetM_PostByID(id,CollectionName.News);
            if (model == null) { throw new HttpException(404, ""); return new EmptyResult(); }
            BLLPost.AddPostPV(id,CollectionName.News);//增加浏览量
            return View("article", model);
        }

        [HttpPost]
        [CommonFilter]
        [BadWordFilter]
        public ActionResult SaveComment(Comment model) {
            if (ModelState.IsValid)
            {
                var flag = HttpContext.Items["flag"].ToBoolean();
                var msg = HttpContext.Items["msg"].ToString();

                if (!flag)
                {
                    return Json(new BLL.JsonResponse { State = (int)BLL.Enum_JsonResponse.ERROR, ResponseText = msg });
                }
                else
                {
                    model._id = ObjectId.GenerateNewId();
                    model._idForString = model._id.ToString();
                    model.IP = Request.UserHostAddress;
                    model.m_User = new M_User { 
                        AvatarURL =string.IsNullOrEmpty( UserInfo.UserAccount.AvatarURL)?
                                                "http://" + Request.Url.Host + "/content/images/men_head.gif" : UserInfo.UserAccount.AvatarURL
                        ,
                        NicName = UserInfo.UserAccount.UserName,
                        UserID = UserInfo.UserAccount.UserID,
                        UserPoint = UserAccount.UserPoint,
                        UserSpecialID = UserAccount.UserSpecialID
                    };
                    model.PostState = (int)BLL.Enums.State.Normal;
                    model.PostTime = DateTime.Now;

                    model.Content = model.Content.Replace("[img]", "<img src='http://www.taluolaile.com/Content/ueditor/dialogs/emotion/images/jx2/")
                                        .Replace("[/img]", ".gif' />");

                    model.Children = new Comment[] { };
                    if (Request.Form["fatherid"] == "")
                    {
                            MongoDBHelper.InsertOne<Comment>(CollectionName.Comments7Days, model);
                    }
                    else
                    {
                        var fid = ObjectId.Parse(Request.Form["fatherid"]);
                        if (MongoDBHelper.Exists(CollectionName.Comments7Days, Query.EQ("_id", fid)))
                            MongoDBHelper.UpdateAll<Comment>(CollectionName.Comments7Days, Query.EQ("_id", fid),
                                Update.AddToSet("Children", model.ToBsonDocument<Comment>()));
                        else
                            MongoDBHelper.UpdateAll<Comment>(CollectionName.Comments, Query.EQ("_id", fid),
                                Update.AddToSet("Children", model.ToBsonDocument<Comment>()));

                    }
                    
                    Func<string, string> func = (s) =>
                    {
                        string collectionName = "";
                        switch (s.ToLower()) { 
                            case "newsdetail":collectionName = CollectionName.News;break;
                            default: collectionName = ""; break;
                        }
                        return collectionName;
                    };

                    BLLPost.AddPostReplyCount(model.FKid, func(Request["actionName"]));
                    return Json(new BLL.JsonResponse { State = (int)BLL.Enum_JsonResponse.OK, ResponseText = msg });
                }
            }else
                return Json(new BLL.JsonResponse { State = (int)BLL.Enum_JsonResponse.ERROR, ResponseText = "输入不合法. 请重试" }); 
        }


        #region AJAX选择牌阵, 获得随机牌, 获得牌阵信息
        /// <summary>
        /// 根据帖子ID找回复
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AJAXGetCommentsID(string id)
        {
            var pidx = Request.QueryString["pageindex"].ToInt(1);
            var psize = Request.QueryString["pagesize"].ToInt(20);
            int count = 0;
            var lst = BLLPost.GetCommentListByID(id, new PagerInfo { Page = pidx, PageSize = psize }, out count);

            return Json(new { data = lst, count = count }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AJAXUpPost() {
            string actiontype = Request.QueryString["actiontype"];
            BLLPost.UpPost(Request.QueryString["id"], Request.QueryString["actiontype"].ToLower(), HttpContext);
            return Content("");
        }

        [HttpPost]
        [OpendAjaxFilter]
        public ActionResult AJAXSelectFormation(CardFormationMate model)
        {
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var lst = StaticData.CardFormationMateLst.Where(x => x.CardsCount <= 7);

            if (Request.Form["TagName"] != "-1")
                lst = lst.Where(x => x.TagIDs.Contains(Request.Form["TagName"].ToInt(0)));
            if (!string.IsNullOrEmpty(model.FormationName))
                lst = lst.Where(x => x.FormationName.Contains(model.FormationName));

            pageParm.PRecordCount = lst.Count();
            lst = lst.GetPageList(pageParm);

            return Json(new { Count = pageParm.PRecordCount, Data = lst });
        }
        [HttpPost]
        [OpendAjaxFilter]
        public ActionResult AJAXgetRandomCards()
        {
            var NeedAllCards = Request.Form["NeedAllCards"].ToInt(0);
            var IsGongTingPai = Request.Form["IsGongTingPai"].ToBoolean();
            Func<Data.Framework.Model.TarotCard, bool> fun = x => 1 == 1;
            if (NeedAllCards == 2) fun = x => x.IsBigAkana == true;
            if (NeedAllCards == 3) fun = x => x.IsBigAkana == false;
            if (IsGongTingPai) fun = x => x.IsGongTingPai == true;
            return Json(BLLTarotCard.GetCardList(fun));
        }

        [HttpPost]
        [OpendAjaxFilter]
        public ActionResult AJAXgetCardFormationCore()
        {
            var fid = Request.Form["formationid"].ToInt(0);
            var lst = StaticData.CardFormationCoreLst.Where(x => x.FormationID == fid);
            return Json(lst);
        }
        #endregion

        public ActionResult SelectFormation() { return View(); }

        /*删除 帖子 和 回复 操作*/
        #region
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode= "002001001001001")]
        public ActionResult deletePost() {
            string id=Request.QueryString["id"];
            string action = Request.QueryString["action"];
            if (string.IsNullOrEmpty(action))
            {
                BLLPost.UpdateState<M_Post, BsonInt32>(id, CollectionName.Posts7Days, "PostState", BsonInt32.Create((int)BLL.Enums.Mpost_PostState.Lock));
                BLL.BLLLogcs.AddAppLog(
                    string.Format("锁定帖子<a href='{0}' target='_blank'>{1}</a>", "http://" + Request.Url.Host + "/posting/postdetail/" + id, Request["desc"]),
                    "", this.ControllerContext);
            }
            else {
                BLLPost.UpdateState<M_Post, BsonInt32>(id, CollectionName.Posts7Days, "PostState", BsonInt32.Create((int)BLL.Enums.Mpost_PostState.Lock));
                BLL.BLLLogcs.AddAppLog(
                    string.Format("彻底删除帖子:{1}", "http://" + Request.Url.Host + "/posting/postdetail/" + id, Request["desc"]),
                    "", this.ControllerContext);
            }
            return Json(new BLL.JsonResponse { State = 1 }, JsonRequestBehavior.AllowGet);
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "002001001001001")]
        public ActionResult ReturnPost()
        {
            string id = Request.QueryString["id"];
            BLLPost.UpdateState<M_Post, BsonInt32>(id, CollectionName.Posts7Days, "PostState", BsonInt32.Create((int)BLL.Enums.Mpost_PostState.Normal));
            BLL.BLLLogcs.AddAppLog(
                string.Format("恢复帖子<a href='{0}' target='_blank'>{1}</a>", "http://" + Request.Url.Host + "/posting/postdetail/" + id, Request["desc"]),
                "", this.ControllerContext);
            return Json(new BLL.JsonResponse { State = 1 }, JsonRequestBehavior.AllowGet);
        }
        [BLL.ActionFilters.AuthorizationFilter(ResourceCode = "002001001001002")]
        public ActionResult DeleteComment()
        {
            string id = Request.QueryString["id"];
            if (Request["type"] == "1")
                BLLPost.UpdateArrayState<Comment, BsonInt32>(id, CollectionName.Comments7Days, "PostState", BsonInt32.Create((int)Mpost_PostState.Lock), "Children");
            else
                BLLPost.UpdateState<Comment, BsonInt32>(id, CollectionName.Comments7Days, "PostState", BsonInt32.Create((int)Mpost_PostState.Lock));

            BLL.BLLLogcs.AddAppLog(
                string.Format("删除回复|{0}|{1}|左边是主键, 右边1是子回复,0 是主回复", id, Request["type"]),
                "", this.ControllerContext);
            return Json(new BLL.JsonResponse { State = 1 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
