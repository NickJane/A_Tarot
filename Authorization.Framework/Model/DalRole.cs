using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Authorizations.Framework.Model;
using Lib.ClassExt;
namespace Authorizations.Framework.Model
{
    public class DalRole:IRole
    {
        public IEnumerable<Auth_Role> GetRoles(int applicationID) {
            return (new AuthEntities()).Auth_Role.Where(x => x.ApplicationID == applicationID && x.State==1);
        }
        public Auth_Role GetModelByFunc(Func<Auth_Role,bool> func,out Model.AuthEntities enti) {
            Model.AuthEntities entity = new AuthEntities();
            enti = entity;
            return enti.Auth_Role.Where(func).FirstOrDefault();
        }
        public IEnumerable<Auth_Role> GetRoles(int userID, int applicationID) {
            Model.AuthEntities entity = new AuthEntities();
                return from role in entity.Auth_Role
                       join ur in entity.Auth_UserRole on role.RoleID equals ur.RoleID
                       where role.State == 1 && ur.UserID == userID && role.ApplicationID == applicationID
                       select role;
        }

        public bool IsSuper(int Userid) {
            return IsSuper(Userid, SystemConfig.CurrentSystemAuthApplication);
        }
        public bool IsSuper(int Userid, int applicationID) {
            Model.AuthEntities entity = new AuthEntities();
                return entity.Auth_UserRole.Join(entity.Auth_Role, x => x.RoleID, y => y.RoleID, (x, y) => new { IsSuperRole = y.IsSuperRole }).Where(x => x.IsSuperRole).Count() > 0;
            
        }

        public int InsertAuth_Role(Auth_Role model, IEnumerable<int> resCodes) {
            Model.AuthEntities entity = new AuthEntities();
            entity.Auth_Role.AddObject(model);
            foreach (var item in resCodes)
	        {
                entity.Auth_RoleResource.AddObject(new Auth_RoleResource { RoleID = model.RoleID, ResourceID = item });
	        }
            return entity.SaveChanges();
        }

        public IEnumerable<Auth_Role> GetListByParameters(Auth_Role model, ref Lib.BaseClass.PageParameter pageParm)
        {
            Model.AuthEntities entity = new AuthEntities();

            var lst = entity.Auth_Role.Where(x => 1 == 1);

            if (model.ApplicationID != -1)
                lst = lst.Where(x => x.ApplicationID == model.ApplicationID);
            if (!string.IsNullOrEmpty(model.LanguagueCode))
                lst = lst.Where(x => x.LanguagueCode.Contains(model.LanguagueCode));
            if (model.State != -1)
                lst = lst.Where(x => x.State == model.State);
            pageParm.PRecordCount = lst.Count();
            return lst.GetPageList(pageParm);
        }

        public int UpdateState(string id, string field, string value, byte[] timeMark)
        {
            using (Model.AuthEntities entity = new AuthEntities())
            {
                if (timeMark != null)
                    return entity.ExecuteStoreCommand("update auth_role set " + field + "=" + value + " where roleid=" + id + " and timemark=@p0", (timeMark));
                else
                    return entity.ExecuteStoreCommand("update auth_role set " + field + "=" + value + " where roleid=" + id);
            }
        }
        public IEnumerable<Auth_UserRole> GetUserRolesByUserID(int userid) {
            Model.AuthEntities entity = new AuthEntities();
            return entity.Auth_UserRole.Where(x => x.UserID == userid);
        }
        public int InsertUserRole(int userid, int roleid) {
            Model.AuthEntities entity = new AuthEntities();
            entity.Auth_UserRole.AddObject(new Auth_UserRole { RoleID=roleid, UserID=userid });
            return entity.SaveChanges();
        }
    }
}
