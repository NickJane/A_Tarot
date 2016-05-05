using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lib.Utils;
using Lib.ClassExt;
using Data.Framework.Model;
namespace Data.Framework.DataProvider
{
    public class CardFormationVisitor
    {
        private static CardFormationVisitor instance = new CardFormationVisitor();
        private CardFormationVisitor() { }
        public static CardFormationVisitor GetInstance()
        {
            return instance;
        }

        public static TarotEntities GetBaseDbContext()
        {
            return new TarotEntities();
        }

        public CardFormation GetModelByFunc(Func<CardFormation, bool> func, out TarotEntities enti)
        {
            TarotEntities entity = new TarotEntities();
            enti = entity;
            return entity.CardFormation.Where(func).FirstOrDefault();
        }
        public CardFormation GetModelByFunc(Func<CardFormation, bool> func)
        {
            TarotEntities entity = new TarotEntities();
            return entity.CardFormation.Where(func).FirstOrDefault();
        }

        public int Insert(CardFormation model, IList<CardFormationCore> lst)
        {
            TarotEntities entity = new TarotEntities();
            entity.CardFormation.AddObject(model);
            foreach (var item in lst)
            {
                item.FormationID = model.CardFormationID;
                entity.CardFormationCore.AddObject(item);
            }
            return entity.SaveChanges();
        }

        public int Update(string id, string field, string value, byte[] timeMark)
        {
            using (Model.TarotEntities entity = new TarotEntities())
            {
                if (timeMark != null)
                    return entity.ExecuteStoreCommand("update CardFormation set " + field + "=" + value +
                    " where CardFormationID=" + id + " and timemark=@p0", (timeMark));
                else
                    return entity.ExecuteStoreCommand("update CardFormation set " + field + "=" + value +
                    " where CardFormationID=" + id);
            }
        }


        public IEnumerable<ModelMate.CardFormationMate> GetListByParameters(CardFormation model,
                            ref Lib.BaseClass.PageParameter pageParm)
        {
            TarotEntities entity = new TarotEntities();

            var othercondition = pageParm.OtherCondition.Split(':');

            var lst = (from cf in entity.CardFormation
                      join ct in entity.Ral_CardF_Tag
                      on cf.CardFormationID equals ct.CardFormationID
                      into modellist
                      from prom in modellist.DefaultIfEmpty()
                      select new ModelMate.CardFormationMate { 
                        CardFormationID=cf.CardFormationID,
                        Describe=cf.Describe,
                        FormationName=cf.FormationName,
                        NeedAllCards=cf.NeedAllCards,
                        Popularity=cf.Popularity,
                        State=cf.State,
                        BindTag=prom==null?false:true
                      }).Distinct().AsEnumerable();

            if (model.CardFormationID > 0)
                lst = lst.Where(x => x.CardFormationID == (model.CardFormationID));
            if (!string.IsNullOrEmpty(model.FormationName))
                lst = lst.Where(x => x.FormationName.Contains(model.FormationName));
            if (!string.IsNullOrEmpty(model.Describe))
                lst = lst.Where(x => x.Describe.Contains(model.Describe));
            //if (model.Popularity != -1)
            //    lst = lst.Where(x => x.Popularity == (model.Popularity));
            if (model.State != -1)
                lst = lst.Where(x => x.State == (model.State));

            Func<ModelMate.CardFormationMate, bool> func;
            switch (othercondition[1])
            {
                case "-1": func = x => 1 == 1; break;
                case "0": func = x => x.BindTag == false; break;
                default: func = x => x.BindTag==true; break;
            }
            lst = lst.Where(func);
            pageParm.PRecordCount = lst.Where(func).Count();
            return lst.GetPageList(pageParm);
        }

        /*
        using (TransactionScope tc = new TransactionScope())
        {
            try
            {

                tc.Complete();
            }
            catch
            { }
        }
        */
    }
}