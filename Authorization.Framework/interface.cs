using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authorizations.Framework.Model;
using Authorizations.Framework.Enums;

namespace Authorizations.Framework
{
    public interface IRole {
        
        IEnumerable<Auth_Role> GetRoles(int applicationID);

        IEnumerable<Auth_Role> GetRoles(int userID, int applicationID);
        Auth_Role GetModelByFunc(Func<Auth_Role, bool> func, out Model.AuthEntities enti);
        bool IsSuper(int Userid);
        bool IsSuper(int Userid, int applicationID);
         IEnumerable<Auth_Role> GetListByParameters(Auth_Role model, ref Lib.BaseClass.PageParameter pageParm);
         int UpdateState(string id, string field, string value, byte[] timeMark);
         int InsertAuth_Role(Auth_Role model, IEnumerable<int> resCodes);
         IEnumerable<Auth_UserRole> GetUserRolesByUserID(int userid);
         int InsertUserRole(int userid, int roleid);
    }

    /// <summary>
    /// 资源
    /// </summary>
    public interface IResource {

        /// <summary>
        /// 获得所有的资源
        /// </summary>
        /// <returns></returns>
        IEnumerable<Auth_Resource> GetAllResource(int? applicationID);
        IEnumerable<Auth_Resource> GetAllResource(int? applicationID, string fatherResourceCode);
        #region 貌似没用了
        ///// <summary>
        ///// 用户所有权限资源
        ///// 包括用户角色权限+用户独立权限
        ///// </summary>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID);
        ///// <summary>
        ///// 用户某个Applicationid所有权限资源
        ///// 包括用户角色权限+用户独立权限
        ///// </summary>
        ///// <param name="UserID"></param>
        ///// <returns></returns>
        //IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID, int? applicationID);

        //IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID, int? applicationID,
        //    ResourceEnum? rType, string fatherResourceCode, bool isIncludeChildren);

        #endregion
        /// <summary>
        /// 已数组形势获得用户所有权限资源的ResourceCode
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        string[] GetCurrentUserResourceCode(int UserID);
        /// <summary>
        /// 已数组形势获得applicationid系统的用户所有权限资源的ResourceCode
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        string[] GetCurrentUserResourceCode(int UserID, int? applicationID);

        int[] GetResourceByRoleID(int roleid);
    }

    public interface IAuthorization { }

    public interface IAuthentication { }
}
