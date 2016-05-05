using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authorizations.Framework.Model;
using Authorizations.Framework.Enums;
using Lib.ClassExt;
using Lib.Utils;
namespace Authorizations.Framework
{
    /// <summary>
    /// 权限模块访问入口
    /// </summary>
    public class AuthVisitor
    {
        static string GetCurrentUserResourceCodeKey = "useridResourceCode";
        static IRole irole = AuthFactory.GetInstance<IRole>("DalRole");
        static IResource ires = AuthFactory.GetInstance<IResource>("DalResource");
        #region 资源
        /// <summary>
        /// 获得applicationid系统的父资源id为fatherResourceCode的资源集合.
        /// 如果fatherResourceCode为null或者空. 那么则获得所有资源集合
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="fatherResourceCode"></param>
        /// <returns></returns>
        public static IEnumerable<Auth_Resource> GetAllResource(int? applicationID, string fatherResourceCode) {
            return ires.GetAllResource(applicationID, fatherResourceCode);
        }
        #region 貌似没用了
        //public static IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int userid)
        //{
        //    return GetCurrentUserResource(userid, null,false);
        //}
        //public static IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int userid, int? applicationid)
        //{
        //    return GetCurrentUserResource(userid, applicationid,false);
        //}
        
        //public static IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int userid, int? applicationid,bool isSuper)
        //{
        //    return GetCurrentUserResource(userid, applicationid, isSuper, null, "", false);
        //}
        //public static IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int userid, int? applicationid, bool isSuper,
        //    Enums.ResourceEnum? rType, string fatherResourceCode, bool isIncludeChildren)
        //{
        //    IResource ires = AuthFactory.GetInstance<IResource>("DalResource");
        //    if (isSuper) return ires.GetAllResource(applicationid,fatherResourceCode);
        //    if (Lib.Utils.CacheHelper.Exists(CurrentUserResource) && SystemConfig.UseCache)
        //        return Lib.Utils.CacheHelper.Get<IEnumerable<Authorization.Framework.DBModel.Auth_Resource>>(CurrentUserResource);
        //    else
        //    {
        //        IEnumerable<Authorization.Framework.DBModel.Auth_Resource> lst;
        //        lst = ires.GetCurrentUserResource(userid, applicationid, rType, fatherResourceCode, isIncludeChildren);
        //        Lib.Utils.CacheHelper.Insert<IEnumerable<Authorization.Framework.DBModel.Auth_Resource>>(CurrentUserResource, lst);
        //        return lst;
        //    }
        //}
        #endregion
        public static string[] GetCurrentUserResourceCode(int userid, int? applicationid) {
            string[] lst = CacheHelper.Get<string[]>(GetCurrentUserResourceCodeKey+userid, () =>
            {
                return ires.GetCurrentUserResourceCode(userid, applicationid);
            });
            return lst;
        }
        public static bool HasPermission(int Userid,int? applicationid, string[] resourceCode) {
            if(isSuper(Userid))
                return true;
            var flag = false;
            string[] lst = GetCurrentUserResourceCode(Userid, applicationid);
            foreach (var item in resourceCode)
            {
                if (lst.Contains(item) || lst.Where(x=>x.StartsWith(item)).Count()>0)
                {
                    flag = true; break;
                }
            }
            return flag;
        }
        public static bool HasPermission(int Userid, int? applicationid, string resourceCode)
        {
            string[] str = new string[1];
            str[0] = resourceCode;
            return HasPermission(Userid, applicationid, str);
        }
        public static int[] GetResourceByRoleID(int roleid)
        {
            return ires.GetResourceByRoleID(roleid);
        }
        #endregion
        #region 角色
        public static IEnumerable<Auth_Role> GetRoles(int applicationID) {
            return irole.GetRoles(applicationID);
        }

        public static IEnumerable<Auth_Role> GetRoles(int userID, int applicationID)
        {
            return irole.GetRoles(userID, applicationID);
        }
        public static Auth_Role GetAuth_RoleByFunc(Func<Auth_Role, bool> func, out Model.AuthEntities enti)
        {
            return irole.GetModelByFunc(func,out enti);
        }
        public static Auth_Role GetAuth_RoleByFunc(Func<Auth_Role, bool> func)
        {
            AuthEntities enti;
            return irole.GetModelByFunc(func, out enti);
        }
        public static bool isSuper(int userid, int? applicationid){
            return irole.GetRoles(userid, applicationid ?? SystemConfig.CurrentSystemAuthApplication).Where(x => x.IsSuperRole).Count() > 0;
        }
        public static bool isSuper(int userid)
        {
            return isSuper(userid, null);
        }
        public static  IEnumerable<Auth_Role> GetListByParameters(Auth_Role model, ref Lib.BaseClass.PageParameter pageParm) {
            return irole.GetListByParameters(model, ref pageParm);
        }
        public static int UpdateState(string id, string field, string value, byte[] timeMark) {
            return irole.UpdateState(id, field, value, timeMark);
        }
        public static int InsertAuth_Role(Auth_Role model, IEnumerable<int> resCodes)
        {
            return irole.InsertAuth_Role(model,resCodes);
        }
        public static IEnumerable<Auth_UserRole> GetUserRolesByUserID(int userid) {
            return irole.GetUserRolesByUserID(userid);
        }
        public static int InsertUserRole(int userid, int roleid) {
            return irole.InsertUserRole(userid, roleid);
        }
        #endregion

    }
}
