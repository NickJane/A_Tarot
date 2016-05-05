using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace Authorizations.Framework.ModelMate
{
	public partial class Auth_ResourceMate{
	
[Required(ErrorMessage="ResourceID不能为空")]
		public int ResourceID{ get; set;}
	
[Required(ErrorMessage="ResourceCode不能为空")]
[StringLength(50)]
		public string ResourceCode{ get; set;}
	
[Required(ErrorMessage="State不能为空")]
		public int State{ get; set;}
	
[StringLength(50)]
		public string ResourceUrl{ get; set;}
	
[Required(ErrorMessage="ResourceType不能为空")]
		public int ResourceType{ get; set;}
	
[Required(ErrorMessage="LanguageCode不能为空")]
[StringLength(20)]
		public string LanguageCode{ get; set;}
	
		public int OrderIndex{ get; set;}
		}
}