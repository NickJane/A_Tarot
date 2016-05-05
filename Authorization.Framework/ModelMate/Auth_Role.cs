using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Authorizations.Framework.ModelMate
{
	public partial class Auth_RoleMate{
	
[Required(ErrorMessage="RoleID不能为空")]
		public int RoleID{ get; set;}
	
[Required(ErrorMessage="LanguagueCode不能为空")]
[StringLength(20)]
		public string LanguagueCode{ get; set;}
	
[Required(ErrorMessage="State不能为空")]
		public int State{ get; set;}
	
[Required(ErrorMessage="IsSuperRole不能为空")]
		public bool IsSuperRole{ get; set;}
	
[Required(ErrorMessage="ApplicationID不能为空")]
		public int ApplicationID{ get; set;}
		}
}