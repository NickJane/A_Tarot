﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Lib.Utils;

namespace Admin3.Models
{
    
    public class LoginModel
    {
        [Required(AllowEmptyStrings=false,ErrorMessage="用户名不能为空")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string OldPassword { get; set; }
    }
}