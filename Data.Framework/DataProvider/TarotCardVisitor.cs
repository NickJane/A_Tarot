using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.Model;
namespace Data.Framework.DataProvider
{
    public class TarotCardVisitor
    {
        private static TarotCardVisitor instance = new TarotCardVisitor();
        private TarotCardVisitor() { }
        public static TarotCardVisitor GetInstance()
        {
            return instance;
        }

        public static TarotEntities GetBaseDbContext()
        {
            return new TarotEntities();
        }

        public TarotCard GetModelByFunc(Func<TarotCard, bool> func, out TarotEntities enti)
        {
            TarotEntities entity = new TarotEntities();
            enti = entity;
            return enti.TarotCard.Where(func).FirstOrDefault();
        }
        public TarotCard GetModelByFunc(Func<TarotCard, bool> func)
        {
            TarotEntities entity = new TarotEntities();
            return entity.TarotCard.Where(func).FirstOrDefault();
        }

        public int Insert(TarotCard model)
        {
            TarotEntities entity = new TarotEntities();
            entity.TarotCard.AddObject(model);
            return entity.SaveChanges();
        }

        public int Update(string id, string field, string value, byte[] timeMark)
        {
            using (Model.TarotEntities entity = new TarotEntities())
            {
                if (timeMark != null)
                    return entity.ExecuteStoreCommand("update TarotCard set " + field + "=" + value +
                    " where CardID=" + id + " and timemark=@p0", (timeMark));
                else
                    return entity.ExecuteStoreCommand("update TarotCard set " + field + "=" + value +
                    " where CardID=" + id);
            }
        }


        public IEnumerable<TarotCard> GetListByParameters(TarotCard model,
                            ref Lib.BaseClass.PageParameter pageParm)
        {
            TarotEntities entity = new TarotEntities();

            var lst = entity.TarotCard.Where(x => 1 == 1);

            if (model.CardID >0)
                lst = lst.Where(x => x.CardID == (model.CardID));
            if (!string.IsNullOrEmpty(model.CardName))
                lst = lst.Where(x => x.CardName.Contains(model.CardName));

            //lst = lst.Where(x=>x.IsBigAkana==model.IsBigAkana);
            //lst = lst.Where(x => x.IsGongTingPai == model.IsGongTingPai);

            pageParm.PRecordCount = lst.Count();
            return lst.GetPageList(pageParm);
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