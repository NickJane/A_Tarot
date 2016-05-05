using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserManager.Framework.Enums;
using UserManager.Framework.Model;
namespace UserManager.Framework
{
    public interface IUser
    {
        Model.Auth_UserAccount ValidUserLogin(string username, string password);

        IEnumerable<Model.Auth_UserAccount> GetListByParameters(Model.Auth_UserAccount model, ref Lib.BaseClass.PageParameter pageParm);

        int UpdateState(string id, string field, string value, byte[] timemark);

        Model.Auth_UserAccount GetModelByUserID(int id);
        Model.Auth_UserAccount GetModelByFunc(Func<Auth_UserAccount, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti);

        int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model, Auth_UserInfoCore UserInfoCore);
        int InsertUserAccount(UserManager.Framework.Model.Auth_UserAccount model);

        bool Exists(string field, string value);

        Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func, out UserManager.Framework.Model.UserManagerExamEntities enti);
        Model.Auth_UserInfoCore GetUserInfoCoreByFunc(Func<Auth_UserInfoCore, bool> func);
        
    }
}
