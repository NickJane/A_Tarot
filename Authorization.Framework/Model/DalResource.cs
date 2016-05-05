using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authorizations.Framework.Enums;
using Authorizations.Framework.ModelMate;
namespace Authorizations.Framework.Model
{
    public class DalResource:IResource
    {
        private IList<Authorizations.Framework.Model.Auth_Resource> AllResources
        {
            get
            {
                var entity = new Model.AuthEntities();
                var query = entity.Auth_Resource.Where(x => x.State == 1);
                return query.ToList();
            }
        }
        public IEnumerable<Authorizations.Framework.Model.Auth_Resource> GetAllResource(int? applicationID)
        {
            var lst = AllResources;
            if (applicationID != null)
            {
                var str = "00" + applicationID.Value;
                lst = lst.Where(x => x.ResourceCode.StartsWith(str)).ToList();
            }
            return lst;     
        }
        public IEnumerable<Authorizations.Framework.Model.Auth_Resource> GetAllResource(int? applicationID, string fatherResourceCode) {
            var str = "00" + (applicationID != null ? applicationID.Value.ToString() : SystemConfig.CurrentSystemAuthApplication.ToString());
            var entity = new Model.AuthEntities();
            var query = AllResources.Where(x => x.State == 1 &&
                x.ResourceCode.StartsWith(str));
            if (!string.IsNullOrEmpty(fatherResourceCode))
                query = query.Where(x => x.ResourceCode.StartsWith(fatherResourceCode));
            var lst= (from res in query select res).ToList();
            return lst;
        }
        #region 貌似没用了
        //public IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID) {
        //    return GetCurrentUserResource(UserID, null);
        //}
        //public IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID, int? applicationid, 
        //    ResourceEnum? rType, string fatherResourceCode, bool isIncludeChildren)
        //{ 
        //    var entity = new Model.AuthExamEntities();
        //    var resources = (from ur in entity.Auth_UserRole
        //                     join roleRes in entity.Auth_RoleResource on ur.RoleID equals roleRes.RoleID
        //                     join res in entity.Auth_Resource on roleRes.ResourceID equals res.ResourceID
        //                     where ur.UserID == UserID 
        //                     select new Authorization.Framework.DBModel.Auth_Resource
        //                     {
        //                         ResourceCode = res.ResourceCode,
        //                         LanguageCode = res.LanguageCode,
        //                         OrderIndex = res.OrderIndex == null ? 0 : res.OrderIndex.Value,
        //                         ResourceID = res.ResourceID,
        //                         ResourceType = res.ResourceType,
        //                         ResourceUrl = res.ResourceUrl,
        //                         State = res.State,
                                 
        //                     }).Concat(
        //                    from userRes in entity.Auth_UserResource
        //                    join res in entity.Auth_Resource on userRes.ReourceID equals res.ResourceID
        //                    where userRes.UserID == UserID
        //                    select new Authorization.Framework.DBModel.Auth_Resource
        //                    {
        //                        ResourceCode = res.ResourceCode,
        //                        LanguageCode = res.LanguageCode,
        //                        OrderIndex = res.OrderIndex == null ? 0 : res.OrderIndex.Value,
        //                        ResourceID = res.ResourceID,
        //                        ResourceType = res.ResourceType,
        //                        ResourceUrl = res.ResourceUrl,
        //                        State = res.State,
        //                    }
        //                  );
        //    string tempCode = "00" +
        //        (applicationid == null ? SystemConfig.CurrentSystemAuthApplication : applicationid.Value);
        //    resources = resources.Where(x => x.ResourceCode.StartsWith(tempCode));
        //    var test = resources.ToList();
        //    if (!string.IsNullOrEmpty(fatherResourceCode)) { 
        //        resources = resources.Where(x =>x.ResourceCode.StartsWith(fatherResourceCode));
        //        if (!isIncludeChildren)
        //            resources = resources.Where(x => x.ResourceType == (int)rType);
        //    }

        //    return resources;
        //}
        //public IEnumerable<Authorization.Framework.DBModel.Auth_Resource> GetCurrentUserResource(int UserID,int? applicationid)
        //{
        //    return GetCurrentUserResource(UserID, applicationid,null,"",false);
        //}
        #endregion

        /// <summary>
        /// 已数组形势获得当前系统的用户所有权限资源的ResourceCode
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string[] GetCurrentUserResourceCode(int UserID) { return GetCurrentUserResourceCode(UserID, null); }

        /// <summary>
        /// 已数组形势获得applicationid系统的用户所有权限资源的ResourceCode
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public string[] GetCurrentUserResourceCode(int UserID, int? applicationid)
        {
            var entity = new Model.AuthEntities();
            var resources = (from ur in entity.Auth_UserRole
                             join roleRes in entity.Auth_RoleResource on ur.RoleID equals roleRes.RoleID
                             join res in entity.Auth_Resource on roleRes.ResourceID equals res.ResourceID
                             join role in entity.Auth_Role on ur.RoleID equals role.RoleID
                             where ur.UserID == UserID && role.State==1
                             select new 
                                {
                                    ResourceCode = res.ResourceCode
                                }
                             )
                             .Concat(
                            from userRes in entity.Auth_UserResource
                            join res in entity.Auth_Resource on userRes.ResourceID equals res.ResourceID
                            where userRes.UserID == UserID
                            select new 
                            {
                                ResourceCode = res.ResourceCode
                            }
                          );
            var rcode = "00" +(applicationid==null?1: applicationid.Value);
            if (applicationid != null)
                resources = resources.Where(x => x.ResourceCode.StartsWith(rcode));
            string[] temp = new string[resources.Count()];
            var i = 0;
            foreach (var item in resources)
            {
                temp[i] = item.ResourceCode;
                i++;
            }
            return temp;
        }

        public int[] GetResourceByRoleID(int roleid)
        {
            var entity = new Model.AuthEntities();
            var resources = (from rr in entity.Auth_RoleResource
                             where rr.RoleID == roleid
                             select new
                             {
                                 resourceID = rr.ResourceID
                             }
                               );
            int[] temp = new int[resources.Count()];
            var i = 0;
            foreach (var item in resources)
            {
                temp[i] = item.resourceID;
                i++;
            }
            return temp;
        }
        
    }
}
