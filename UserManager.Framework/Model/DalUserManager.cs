using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManager.Framework.Enums;
using Lib.ClassExt;
using System.Data.Objects;
using System.Transactions;
namespace UserManager.Framework.Model
{
    public class DalUserManager:IUser
    {
        public Auth_UserAccount ValidUserLogin(string username, string password) {
            password = password.ToMd5();
            Model.UserManagerExamEntities entity = new UserManagerExamEntities();
            return entity.Auth_UserAccount.Where(x => x.UserName == username && x.Password == password && x.ApplicationID == SystemConfig.CurrentSystemUserManagerApplication).FirstOrDefault();
        }
        public IEnumerable<Model.Auth_UserAccount> GetListByParameters(Model.Auth_UserAccount model, ref Lib.BaseClass.PageParameter pageParm)
        {
            Model.UserManagerExamEntities entity = new UserManagerExamEntities();

            var lst = entity.Auth_UserAccount.Where(x=>1==1);

            if (model.ApplicationID != -1)
                lst = lst.Where(x => x.ApplicationID == model.ApplicationID);
            if (!string.IsNullOrEmpty(model.UserName))
                lst =lst.Where(x => x.UserName.Contains(model.UserName));
            if (model.State != -1)
                lst = lst.Where(x => x.State == model.State);
            pageParm.PRecordCount = lst.Count();
            return lst.GetPageList(pageParm);
        }

        public int UpdateState(string id, string field, string value, byte[] timeMark) {
            using (Model.UserManagerExamEntities entity = new UserManagerExamEntities())
            {
                if (timeMark != null)
                    return entity.ExecuteStoreCommand("update auth_useraccount set " + field + "=@p1 where userid=" + id + " and timemark=@p0", (timeMark), value);
                else
                    return entity.ExecuteStoreCommand("update auth_useraccount set " + field + "=@p0 where userid=" + id, value);
            }
        }
        public Model.Auth_UserAccount GetModelByUserID(int id) {
            Model.UserManagerExamEntities entity = new UserManagerExamEntities();
            return entity.Auth_UserAccount.FirstOrDefault(x => x.UserID == id);
        }
        public Model.Auth_UserAccount GetModelByFunc(Func<Auth_UserAccount, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti)
        {
            Model.UserManagerExamEntities entity = new UserManagerExamEntities();
            enti = entity;
            return entity.Auth_UserAccount.FirstOrDefault(func);
        }
        public int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model) {
            return InsertUserAccount(model, new Auth_UserInfoCore { UserID=model.UserID, LastModifyTime=DateTime.Now });
        }
        public int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model, Auth_UserInfoCore UserAccount)
        {
            using (TransactionScope tc = new TransactionScope())
            {
                using (Model.UserManagerExamEntities entity = new UserManagerExamEntities())
                {
                    model.Password = model.Password.ToMd5();
                    entity.Auth_UserAccount.AddObject(model);
                    entity.SaveChanges();
                    UserAccount.UserID = model.UserID;
                    entity.Auth_UserInfoCore.AddObject(UserAccount);
                    entity.SaveChanges();
                    tc.Complete();
                }
            }
            return model.UserID;
        }
        /// <summary>
        /// 是否存在field=value的某行记录
        /// </summary>
        /// <param name="field">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Exists(string field, string value) {
            using (Model.UserManagerExamEntities entity = new UserManagerExamEntities())
            {
                ObjectResult<string> res = entity.ExecuteStoreQuery<string>("select " + field + " from Auth_UserAccount where " + field + "=@p0", value);
                return res.Count() > 0;
            }
        }

        public Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti) {
            UserManager.Framework.Model.UserManagerExamEntities entity = new UserManager.Framework.Model.UserManagerExamEntities();
            
                enti = entity;
                return entity.Auth_UserInfoCore.Where(func).FirstOrDefault();
            
        }
        public Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func) {
            UserManager.Framework.Model.UserManagerExamEntities entity;
            return this.GetUserInfoCoreByFunc(func, out entity);
        }
    }
}
