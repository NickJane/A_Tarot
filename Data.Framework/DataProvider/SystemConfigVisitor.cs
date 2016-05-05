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
    public class SystemConfigVisitor
    {
        private static SystemConfigVisitor instance = new SystemConfigVisitor();
        private SystemConfigVisitor() { }
        public static SystemConfigVisitor GetInstance()
        {
            return instance;
        }

        public static TarotEntities GetBaseDbContext()
        {
            return new TarotEntities();
        }

        public SystemConfig GetModelByFunc(Func<SystemConfig, bool> func, out TarotEntities enti)
        {
            TarotEntities entity = new TarotEntities();
            enti = entity;
            return entity.SystemConfig.Where(func).FirstOrDefault();
        }
        public SystemConfig GetModelByFunc(Func<SystemConfig, bool> func)
        {
            TarotEntities entity = new TarotEntities();
            return entity.SystemConfig.Where(func).FirstOrDefault();
        }

        public int Insert(SystemConfig model)
        {
            TarotEntities entity = new TarotEntities();
            entity.SystemConfig.AddObject(model);
            return entity.SaveChanges();
        }

        public int Update(string field, string value)
        {
            using (Model.TarotEntities entity = new TarotEntities())
            {
                return entity.ExecuteStoreCommand("update SystemConfig set _value=@p0 where _key=@p1",value,field);
            }
        }


        public IEnumerable<SystemConfig> GetListByParameters(SystemConfig model,
                            ref Lib.BaseClass.PageParameter pageParm)
        {
            TarotEntities entity = new TarotEntities();

            var lst = entity.SystemConfig.Where(x => 1 == 1);
            /*
                        if(model._ID > 0)
                lst = lst.Where(x=>x._ID == (model._ID));
            if(!string.IsNullOrEmpty(model._Key))
                lst = lst.Where(x=>x._Key.Contains(model._Key));
            if(!string.IsNullOrEmpty(model._Value))
                lst = lst.Where(x=>x._Value.Contains(model._Value));
            */
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