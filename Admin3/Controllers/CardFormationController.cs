using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using BLL.ActionFilters;
using Lib.BaseClass;
using Authorizations.Framework;
using Authorizations.Framework.Model;
using UserManager.Framework.Model;
using UserManager.Framework;
using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.Model;
using Data.Framework.DataProvider;
using System.Transactions;
namespace Admin3.Controllers
{
    public class CardFormationController : BaseController
    {
        [AuthorizationFilter(ResourceCode = "001002001002")]
        public ActionResult List()
        {
            return View();
        }

        [AuthorizationFilter(ResourceCode = "001002001002")]
        public ActionResult Form(int? id)
        {
            ViewBag.IsEdit = id == null ? false : true;
            if(ViewBag.IsEdit)
                ViewBag.FormationCards = CardFormationVisitor.GetBaseDbContext().CardFormationCore.Where(x => x.FormationID == id).ToList();
            CardFormation model = id != null ?
                CardFormationVisitor.GetInstance().GetModelByFunc(x => x.CardFormationID == id.Value) : new CardFormation();
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001002001002")]
        [HttpPost]
        public ActionResult Form(CardFormation model)//添加或者删除牌阵
        {
            Lib.Utils.FileHelper.AppendFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|", Lib.Utils.ConfigHelper.GetAppSetting("CacheDependencyFile"));

            ViewBag.IsEdit = model.CardFormationID == 0 ? false : true;
            if (ModelState.IsValid)
            {
                if (model.CardFormationID == 0)
                {
                    var flag = true;
                    #region /*判断逻辑区*/
                    if (Data.Framework.DataProvider.CardFormationVisitor.GetInstance().GetModelByFunc(x=>x.FormationName==model.FormationName.Trim())!=null)
                    {
                        ModelState.AddModelError("", "该牌阵已存在");
                        flag = false;
                    }
                    #endregion

                    if (flag)
                    {
                        model.State = 1;
                        var cards = Request.Form.AllKeys.Where(x => x.StartsWith("X"));
                        IList<CardFormationCore> lst = new List<CardFormationCore>();
                        for (int i = 1; i <= cards.Count(); i++)
                        {
                            lst.Add(new CardFormationCore
                            {
                                H = Request.Form["H" + i].ToInt(),
                                IsHandstand=false,
                                IsPointCard=false,
                                SortIndex = Request.Form["SortIndex" + i].ToInt(),
                                W = Request.Form["W" + i].ToInt(),
                                X = Request.Form["X" + i].ToInt(),
                                Y = Request.Form["Y" + i].ToInt(),
                                Describe = Request.Form["des" + i]
                            });
                        }
                        var result = CardFormationVisitor.GetInstance().Insert(model,lst);

                        ScreenshotHelper.Run(ConfigHelper.GetAppSetting("SysCardF")+model.CardFormationID+".jpg",
                        Lib.Utils.ConfigHelper.GetAppSetting("TarotServices") + "ScreenShottarotformation.aspx?id=" + model.CardFormationID,
                        model.PlaceHeight, model.PlaceWidth);

                        if (result >= 1)
                            return RedirectToAction("ForJs", "Shared", new { JS = "alert('添加成功');location.href='/cardformation/list';" });
                    }
                }
                else
                {
                    TarotEntities enti;
                    var dbModel = CardFormationVisitor.GetInstance().GetModelByFunc(x => x.CardFormationID == model.CardFormationID
                                    , out enti);

                    if (dbModel == null)
                    {
                        JSCustom("parent.Reload();");
                        JSAlert_ReloadPage("并发冲突, 你看到的数据是旧的数据, 已经被其他用户修改过, 系统将刷新显示新的数据",
                                            "/CardFormationmanager/form/" + model.CardFormationID);
                    }
                    else
                    {
                        UpdateModel(dbModel);
                        model.State = 1;
                        using (TransactionScope tc = new TransactionScope())
                        {
                            try
                            {
                                enti.ExecuteStoreCommand("delete from CardFormationCore where Formationid=" + model.CardFormationID);

                                var cards = Request.Form.AllKeys.Where(x => x.StartsWith("X"));

                                for (int i = 1; i <= cards.Count(); i++)
                                {
                                    enti.CardFormationCore.AddObject(new CardFormationCore
                                    {
                                        FormationID=model.CardFormationID,
                                        H = Request.Form["H" + i].ToInt(),
                                        IsHandstand = false,
                                        IsPointCard = false,
                                        SortIndex = Request.Form["SortIndex" + i].ToInt(),
                                        W = Request.Form["W" + i].ToInt(),
                                        X = Request.Form["X" + i].ToInt(),
                                        Y = Request.Form["Y" + i].ToInt(),
                                        Describe = Request.Form["des" + i]
                                    });
                                }
                                var result = enti.SaveChanges();
                                tc.Complete();
                                if (result >= 1)
                                {
                                    return RedirectToAction("ForJs", "Shared", new { JS = "alert('修改成功');location.href='/cardformation/list';" });
                                }
                            }
                            catch (Exception ex)
                            {
                                JSAlert_ReloadPage(ex.Message, "/cardformation/form/" + model.CardFormationID);
                            }
                        }
                        
                    }
                }
            }
            return View(model);
        }

        [AuthorizationFilter(ResourceCode = "001002001002")]
        public ActionResult Detail(int id)
        {
            ViewBag.FormationCards = CardFormationVisitor.GetBaseDbContext().CardFormationCore.Where(x => x.FormationID == id).ToList();
            return View(CardFormationVisitor.GetInstance().GetModelByFunc(x => x.CardFormationID == id));
        }

        /**************************AJAX**************************/
        public ActionResult GetListByParameters(CardFormation model)
        {
            //初始化实体类的辅助分页父类属性
            PageParameter pageParm = new PageParameter();
            base.FillPageParameters(ref pageParm);

            var HasBindTag = Request["ddlIsbindTag"];
            pageParm.OtherCondition = string.Format("{0}:{1}", "HasBindTag", HasBindTag);

            //var enti = CardFormationVisitor.GetBaseDbContext();
            var lst = CardFormationVisitor.GetInstance().GetListByParameters(model, ref pageParm).Select(x => new
            {
                CardFormationID = x.CardFormationID,
                FormationName = x.FormationName,
                Describe = x.Describe,
                Popularity = x.Popularity,
                State = ((BLL.Enums.State)x.State).ToLocalLanguage(),
                NeedAllCards = ((BLL.Enums.TarotF_NeedAllCards)x.NeedAllCards).ToLocalLanguage(),
                HasBindTag =x.BindTag //enti.Ral_CardF_Tag.FirstOrDefault(y => y.CardFormationID == x.CardFormationID) == null ? false : true
            }).ToList();
            
            //foreach (var item in lst)
            //{
            //    var temp=enti.Ral_CardF_Tag.FirstOrDefault(x=>x.CardFormationID==item.CardFormationID);
            //    item.HasBindTag = temp == null ? false : true;
            //}
            return Json(new { Rows = lst, Total = pageParm.PRecordCount });
        }

        [AuthorizationFilter(ResourceCode = "001002001002")]
        public ActionResult UpdateState()
        {
            Lib.Utils.FileHelper.AppendFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|", Lib.Utils.ConfigHelper.GetAppSetting("CacheDependencyFile"));

            string[] ids = Request.QueryString["keyids"].Split(',');
            string[] timeMarks = string.IsNullOrEmpty(Request.QueryString["timeMark"]) ? new string[] { } : Request.QueryString["timeMark"].Split('#');
            string field = Request.QueryString["field"];
            string value = Request.QueryString["value"];

            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i]))
                    continue;
                byte[] timemark = (timeMarks.Length > 0 && !string.IsNullOrEmpty(timeMarks[0])) ?
                                                                Convert.FromBase64String(timeMarks[i]) : null;
                CardFormationVisitor.GetInstance().Update(ids[i], field, value, timemark);
            }
            return ReturnResult(1, "");
        }

        [AuthorizationFilter(ResourceCode = "001002001002001")]
        public ActionResult BindFormationToTag(int id) {
            return View(CardFormationVisitor.GetInstance().GetModelByFunc(x=>x.CardFormationID==id));
        }
        [AuthorizationFilter(ResourceCode = "001002001002001")]
        [HttpPost]
        public ActionResult BindFormationToTag(FormCollection form)
        {
            Lib.Utils.FileHelper.AppendFile(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|", Lib.Utils.ConfigHelper.GetAppSetting("CacheDependencyFile"));

            var checkedTagIDs = string.IsNullOrEmpty(form["TagName"]) ? new string[] { } : form["TagName"].Split(',');
            var formationid = form["CardFormationID"].ToInt();
            var entity = CardFormationVisitor.GetBaseDbContext();
            entity.ExecuteStoreCommand("delete from Ral_CardF_Tag where CardFormationID=@p0", formationid);
            foreach (var item in checkedTagIDs)
            {
                if (string.IsNullOrEmpty(item)) continue;
                Ral_CardF_Tag model = new Ral_CardF_Tag { 
                    CardFormationID=form["CardFormationID"].ToInt(), 
                    CecommendLevel=form["rad"+item].ToInt(),
                    TagID=item.ToInt(),
                };
                entity.Ral_CardF_Tag.AddObject(model);
            }
            entity.SaveChanges();

            return RedirectToAction("ForJs", "Shared", new { JS = "parent.Reload(); alert('关联成功');" });
        }
    }
}