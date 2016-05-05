using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.ComponentModel;
namespace Lib.ClassExt
{
    public class ModelValidationError
    {
        public string FieldName { get; set; }
        public string Message { get; set; }
    }

    public static class DataAnnotationHelper
    {
        /// <summary>
        /// 验证实体属性
        /// </summary>
        /// <typeparam name="T">需要验证的实体</typeparam>
        /// <returns>返回属性验证结果集合. FieldName:验证的属性名, Message,如果错误结果是多少</returns>
        public static IEnumerable<ModelValidationError> IsValid<T>(this T o)
        {
            var descriptor = GetTypeDescriptor(typeof(T));
            foreach (PropertyDescriptor propertyDescriptor in descriptor.GetProperties())
            {
                foreach (var validationAttribute in propertyDescriptor.Attributes.OfType<ValidationAttribute>())
                {
                    if (!validationAttribute.IsValid(propertyDescriptor.GetValue(o)))
                    {
                        yield return new ModelValidationError() { FieldName = propertyDescriptor.Name, Message = validationAttribute.FormatErrorMessage(propertyDescriptor.Name) };
                    }
                }
            }
        }

        private static ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }

    } 


    #region Validation
    /// <summary>
    /// 类的特性描述.用于比较两个属性的值
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();
        /// <summary>
        /// 比较两个属性的值
        /// </summary>
        /// <param name="originalProperty">第一个属性</param>
        /// <param name="confirmProperty">第二个属性</param>
        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' 至少{1} 个字符 .";
        private readonly int _minCharacters = 6;//Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion
}
