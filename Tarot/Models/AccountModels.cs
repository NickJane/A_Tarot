using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
namespace Tarot.Models
{

    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        [MaxLength(16,ErrorMessage="不能超过16个字符")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        [MaxLength(16, ErrorMessage = "不能超过16个字符")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新密码")]
        [MaxLength(16, ErrorMessage = "不能超过16个字符")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        [MinLength(3,ErrorMessage="用户名最小长度为3")]
        [MaxLength(20,ErrorMessage="用户名最大长度为20")]
        [RegularExpression("^\\S+$", ErrorMessage = "用户名有非法字符")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "电子邮件地址")]
        [MaxLength(50, ErrorMessage = "电子邮件最大长度为50")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [MaxLength(20, ErrorMessage = "密码最大长度为20")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "昵称")]
        [StringLength(20,ErrorMessage="昵称长度不能超过20")]
        public string NicName { get; set; }
    }

    public class QQWeiboRegisterModel
    {
        [Required]
        [Display(Name = "用户名")]
        [MinLength(3, ErrorMessage = "用户名最小长度为3")]
        [MaxLength(20, ErrorMessage = "用户名最大长度为20")]
        [RegularExpression("^\\S+$", ErrorMessage = "用户名有非法字符")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "电子邮件地址")]
        [MaxLength(50, ErrorMessage = "电子邮件最大长度为50")]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        [MaxLength(20, ErrorMessage = "密码最大长度为20")]
        public string Password { get; set; }
    }
}
