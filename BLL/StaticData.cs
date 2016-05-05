using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Framework.Model;
using Data.Framework.ModelMate;
using Lib.Utils;
using Data.MongoFramework;
using System.Web;
namespace BLL
{
    /// <summary>
    /// 静态化元数据
    /// </summary>
    public class StaticData
    {
        private static string _TarotCardLst="cache_TarotCardLst";
        private static string _TagLst = "cache_TagLst";
        private static string _CardFormationMateLst = "cache_CardFormationMateLst";
        private static string _CardFormationCoreLst = "cache_CardFormationCoreLst";
        private static string _Ral_CardF_TagLst = "cache_Ral_CardF_TagLst";
        private static string _TarotCards = "cache_TarotCards";
        private static string _CustomerTags = "cache_CustomerTags";
        private static string _dic_Province = "cache_dic_Province";

        private static string CacheDependencyFile = Lib.Utils.ConfigHelper.GetAppSetting("CacheDependencyFile");

        #region 省份
        public static IList<dic_Province> Dic_Province
        {
            get
            {
                if (CacheHelper.Exists(_dic_Province))
                    return CacheHelper.Get<IList<dic_Province>>(_dic_Province);
                else
                {
                    using (var entity = new TarotEntities())
                    {
                        var lst = entity.Dic_Province.ToList();
                        lst.Insert(0, new dic_Province { ProvinceId=-1, ProvinceName="--请选择--" });
                        CacheHelper.SetCache(_TarotCardLst, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }
        #endregion

        public static IList<TarotCard> TarotCardLst { get{
            if (CacheHelper.Exists(_TarotCardLst))
                return CacheHelper.Get<IList<TarotCard>>(_TarotCardLst);
            else {
                using (var entity = new TarotEntities()) {
                    var lst = entity.TarotCard.ToList();
                    CacheHelper.SetCache(_TarotCardLst, lst, CacheDependencyFile);
                    return lst;
                }
            }
        } }
        public static IList<Tag> TagLst
        {
            get{
                if (CacheHelper.Exists(_TagLst))
                    return CacheHelper.Get<IList<Tag>>(_TagLst);
                else {
                    using (var entity = new TarotEntities()) {
                        var lst = entity.Tag.Where(x => x.State == 1).ToList();
                        CacheHelper.SetCache(_TagLst, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }
        public static IList<CardFormationMate> CardFormationMateLst
        {
            get
            {
                if (CacheHelper.Exists(_CardFormationMateLst))
                    return CacheHelper.Get<IList<CardFormationMate>>(_CardFormationMateLst);
                else
                {
                    using (var entity = new TarotEntities())
                    {
                        var lst = entity.CardFormation.Where(x => x.State == 1).Select(x => new CardFormationMate
                        {
                            CardFormationID = x.CardFormationID,
                            CardsCount = x.CardFormationCore.Where(y => y.FormationID == x.CardFormationID).Count(),
                            Describe = x.Describe,
                            FormationName = x.FormationName,
                            NeedAllCards = x.NeedAllCards,
                            Popularity = x.Popularity,
                            State = 1,
                            PlaceHeight=x.PlaceHeight,
                            PlaceWidth=x.PlaceWidth
                            //TagIDs = 
                        }).ToList();
                        foreach (var item in lst)
                        {
                            item.TagIDs = Ral_CardF_TagLst.Where(y => y.CardFormationID == item.CardFormationID).Select(y => y.TagID).ToList();
                        }
                        CacheHelper.SetCache(_CardFormationMateLst, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }
        public static IList<CardFormationCoreMate> CardFormationCoreLst
        {
            get
            {
                if (CacheHelper.Exists(_CardFormationCoreLst))
                    return CacheHelper.Get<IList<CardFormationCoreMate>>(_CardFormationCoreLst);
                else
                {
                    using (var entity = new TarotEntities())
                    {
                        var lst = entity.CardFormationCore.Select(x => new CardFormationCoreMate
                        {
                            Describe=x.Describe,
                            FormationID=x.FormationID,
                            H=x.H.Value,
                            IsHandstand=x.IsHandstand,
                            IsPointCard=x.IsPointCard,
                            SortIndex=x.SortIndex,
                            W=x.W,
                            X=x.X,
                            Y=x.Y
                        }).ToList();
                        CacheHelper.SetCache(_CardFormationCoreLst, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }
        public static IList<Ral_CardF_Tag> Ral_CardF_TagLst
        {
            get
            {
                if (CacheHelper.Exists(_Ral_CardF_TagLst))
                    return CacheHelper.Get<IList<Ral_CardF_Tag>>(_Ral_CardF_TagLst);
                else
                {
                    using (var entity = new TarotEntities())
                    {
                        var lst = entity.Ral_CardF_Tag.ToList();
                        CacheHelper.SetCache(_Ral_CardF_TagLst, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }
        public static IList<TarotCard> TarotCard
        {
            get
            {
                if (CacheHelper.Exists(_TarotCards))
                    return CacheHelper.Get<IList<TarotCard>>(_TarotCards);
                else
                {
                    using (var entity = new TarotEntities())
                    {
                        var lst = entity.TarotCard.ToList();
                        CacheHelper.SetCache(_TarotCards, lst, CacheDependencyFile);
                        return lst;
                    }
                }
            }
        }

        public static string CustomerTag {
            get
            {
                IList<Data.MongoFramework.ModelMate.CustomerTag> list;
                if (CacheHelper.Exists(_CustomerTags))
                    list= CacheHelper.Get<IList<Data.MongoFramework.ModelMate.CustomerTag>>(_CustomerTags);
                else
                {
                    var lst = Lib.Data.MongoDBHelper.GetAll<Data.MongoFramework.ModelMate.CustomerTag>(
                        CollectionName.CustomerTag, 30, MongoDB.Driver.Builders.SortBy.Descending("PopularityCount"));

                    CacheHelper.SetCache(_CustomerTags, lst, CacheDependencyFile);
                    list= lst;
                }
                StringBuilder sb=new StringBuilder();
                foreach (var item in list)
                {
                    sb.Append("<a class='A_CustomerTag' href='" +
                        string.Format("http://www.google.com/cse?cx=016222199086485784095:ynnkhml7jiw&q={0}#gsc.tab=0&gsc.q={0}&gsc.page=1", HttpUtility.UrlEncode("tags " + item.TagName))
                    + "' target='_blank'>"
                    + item.TagName + "</a>" + "&nbsp;&nbsp;&nbsp;&nbsp;");
                    
                }
                return sb.ToString();
            }
        }
    }
}
