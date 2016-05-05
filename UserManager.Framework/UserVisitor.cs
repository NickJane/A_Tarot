using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManager.Framework.Enums;
using UserManager.Framework.Model;

namespace UserManager.Framework
{
    public static class UserVisitor
    {
        static IUser iuser = UserFactory.GetUserManagerInstance();
        public static Model.Auth_UserAccount ValidUserLogin(string username, string password) {
            return iuser.ValidUserLogin(username, password);
        }
        public static IEnumerable<Model.Auth_UserAccount> GetListByParameters(Model.Auth_UserAccount model, ref Lib.BaseClass.PageParameter pageparm)
        {
            var lst = iuser.GetListByParameters(model, ref pageparm);
            return lst;
        }
        public static int UpdateState(string id, string field, string value, byte[] timemark=null) {
            return iuser.UpdateState(id, field, value, timemark);
        }

        public static void DeleteCacheOfUserInfoCore(int userid) {
            Lib.Utils.CacheHelper.Remove(string.Format(SystemConfig.Cache_UserInfoCore, userid));
            Lib.Utils.CacheHelper.Remove(string.Format(SystemConfig.Cache_UserAccount, userid));
        }

        /// <summary>
        /// 带缓存;
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Model.Auth_UserAccount GetModelByUserID(int id) {
            return Lib.Utils.CacheHelper.Get<Auth_UserAccount>(
                string.Format(SystemConfig.Cache_UserAccount, id), 
                () => iuser.GetModelByUserID(id),
                20);
        }
        public static Model.Auth_UserAccount GetModelByFunc(Func<Auth_UserAccount, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti)
        {
            return iuser.GetModelByFunc(func,out enti);
        }
        public static Model.Auth_UserAccount GetModelByFunc(Func<Auth_UserAccount, bool> func)
        {
            UserManager.Framework.Model.UserManagerExamEntities enti;
            return iuser.GetModelByFunc(func, out enti);
        }
        
        public static Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func)
        {
            UserManager.Framework.Model.UserManagerExamEntities enti;
            return iuser.GetUserInfoCoreByFunc(func,out enti);
        }
        public static Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti)
        {
            return iuser.GetUserInfoCoreByFunc(func,out enti);
        }
        /// <summary>
        /// 带缓存
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Model.Auth_UserInfoCore GetUserInfoCoreByUserID(int userid)
        {
            return Lib.Utils.CacheHelper.Get<Auth_UserInfoCore>(
                string.Format(SystemConfig.Cache_UserInfoCore, userid),
                () => iuser.GetUserInfoCoreByFunc(x => x.UserID == userid),
                20);
        }
        /// <summary>
        /// 返回userid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model) {
            return iuser.InsertUserAccount(model);
        }
        public static int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model, Auth_UserInfoCore userinfocore)
        {
            return iuser.InsertUserAccount(model, userinfocore);
        }
        /// <summary>
        /// 是否存在field=value的某行记录
        /// </summary>
        /// <param name="field">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool Exists(string field, string value) {
            return iuser.Exists(field, value);
        }
        public static void SaveChanges()
        {
            UserManager.Framework.Model.UserManagerExamEntities enti = new Model.UserManagerExamEntities();
            enti.SaveChanges();
        }  
    }
}
