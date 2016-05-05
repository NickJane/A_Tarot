using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.Model;
using System.Web;
namespace Data.Framework.DataProvider
{
    public class TagVisitor
    {
        private static string cachefile = ConfigHelper.GetAppSetting("CacheDependencyFile");
        private static string cachekey = "TagVisitor";

        private static TagVisitor instance = new TagVisitor();
        private TagVisitor() { }
        public static TagVisitor GetInstance()
        {
            return instance;
        }

        public static TarotEntities GetBaseDbContext()
        {
            return new TarotEntities();
        }

        public Tag GetModelByFunc(Func<Tag, bool> func, out TarotEntities enti)
        {
            TarotEntities entity = new TarotEntities();
            enti = entity;
            return entity.Tag.Where(func).FirstOrDefault();
        }
        public Tag GetModelByFunc(Func<Tag, bool> func)
        {
            TarotEntities entity = new TarotEntities();
            return entity.Tag.Where(func).FirstOrDefault();
        }

        public int Insert(Tag model)
        {
            TarotEntities entity = new TarotEntities();
            entity.Tag.AddObject(model);
            return entity.SaveChanges();
        }

        public int Update(string id, string field, string value, byte[] timeMark)
        {
            using (Model.TarotEntities entity = new TarotEntities())
            {
                if (timeMark != null)
                    return entity.ExecuteStoreCommand("update Tag set " + field + "=" + value +
                    " where TagID=" + id + " and timemark=@p0", (timeMark));
                else
                    return entity.ExecuteStoreCommand("update Tag set " + field + "=" + value +
                    " where TagID=" + id);
            }
        }


        public IEnumerable<Tag> GetListByParameters(Tag model,
                            ref Lib.BaseClass.PageParameter pageParm)
        {
            TarotEntities entity = new TarotEntities();

            var lst = entity.Tag.Where(x => 1 == 1);

            if (model.TagID > 0)
                lst = lst.Where(x => x.TagID == (model.TagID));
            if (!string.IsNullOrEmpty(model.TagName))
                lst = lst.Where(x => x.TagName.Contains(model.TagName));
            
            if (model.State != -1)
                lst = lst.Where(x => x.State == (model.State));

            pageParm.PRecordCount = lst.Count();
            return lst.GetPageList(pageParm);
        }
        /// <summary>
        /// 已缓存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<Tag> GetListByParameters(Tag model)
        {
            if (CacheHelper.Exists(cachekey))
                return CacheHelper.Get<IEnumerable<Tag>>(cachekey);
            else
            {
                TarotEntities entity = new TarotEntities();

                var lst = entity.Tag.Where(x => 1 == 1);

                if (model.TagID > 0)
                    lst = lst.Where(x => x.TagID == (model.TagID));
                if (!string.IsNullOrEmpty(model.TagName))
                    lst = lst.Where(x => x.TagName.Contains(model.TagName));

                if (model.State != -1)
                    lst = lst.Where(x => x.State == (model.State));

                CacheHelper.SetCache(cachekey,lst,cachefile);
                return lst;
            }
        }

        /*
        using (TransactionScope tc = new TransactionScope())
        {
            try
            {
                NorthwindDataContext ctx = new NorthwindDataContext();
                
                ctx.AddCategory("test", "test", ref categoryId);
                ctx.DeleteCategory(1);

                tc.Complete();
            }
            catch
            { }
        }
        */
    }
}