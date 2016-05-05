using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Lib.Utils
{
    /// <summary>
    /// XML序列化帮助器
    /// </summary>
    public static class XmlSerializeHelper
    {
        #region 泛型

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI">Type of Instance</typeparam>
        /// <param name="srcName">Source PropertyName</param>
        /// <param name="targetName">Target PropertyName</param>
        /// <param name="overrides">Overrides</param>
        /// <returns>Attached-Overrides</returns>
        public static void ChangePropertyName<TOI>(this XmlAttributeOverrides overrides, string srcName, string targetName)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(targetName));
            overrides.Add(typeof(TOI), srcName, attrs);
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOP">Type of Property</typeparam>
        /// <typeparam name="TOI">Type of Instance</typeparam>
        /// <param name="srcName">Source PropertyName</param>
        /// <param name="targetName">Target PropertyName</param>
        /// <param name="overrides">Overrides</param>
        public static XmlAttributeOverrides ChangePropertyName<TOP, TOI>(this XmlAttributeOverrides overrides, string srcName, string targetName)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(targetName, typeof(TOP)));
            overrides.Add(typeof(TOI), srcName, attrs);
            return overrides;
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangePropertiesNames<TOI>(this XmlAttributeOverrides overrides, IDictionary<string, string> transform)
        {
            foreach (var srcName in transform.Keys)
            {
                ChangePropertyName<TOI>(overrides, srcName, transform[srcName]);
            }
            return overrides;
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangePropertiesNames<TOI>(this XmlAttributeOverrides overrides, string[][] transform)
        {
            foreach (var p in transform)
            {
                ChangePropertyName<TOI>(overrides, p[0], p[1]);
            }
            return overrides;
        }

        /// <summary>
        /// 隐藏属性
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides HideProperty<TOI>(this XmlAttributeOverrides overrides, string name)
        {
            XmlAttributes attrs = new XmlAttributes();
            //attrs.XmlElements.Add(new XmlElementAttribute());
            attrs.XmlIgnore = true;
            overrides.Add(typeof(TOI), name, attrs);
            return overrides;
        }

        /// <summary>
        /// 隐藏属性
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides HideProperties<TOI>(this XmlAttributeOverrides overrides, params string[] names)
        {
            foreach (var name in names)
            {
                HideProperty<TOI>(overrides, name);
            }
            return overrides;
        }

        /// <summary>
        /// 设置数组和数组项名称
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="srcArrayName"></param>
        /// <param name="arrayName"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangeArrayName<TOI, TOAI>(this XmlAttributeOverrides overrides, string srcArrayName, string arrayName, string itemName)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlArray = new XmlArrayAttribute(arrayName);
            if (!string.IsNullOrEmpty(itemName)) attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(itemName, typeof(TOAI)));
            overrides.Add(typeof(TOI), srcArrayName, attrs);
            return overrides;
        }

        #endregion

        public static XmlAttributeOverrides ChangeRootName(this XmlAttributeOverrides overrides, string srcName, string targetName, Type instanceType)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlRoot = new XmlRootAttribute(targetName);
            overrides.Add(instanceType, srcName, attrs);
            return overrides;
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI">Type of Instance</typeparam>
        /// <param name="srcName">Source PropertyName</param>
        /// <param name="targetName">Target PropertyName</param>
        /// <param name="overrides">Overrides</param>
        /// <returns>Attached-Overrides</returns>
        public static XmlAttributeOverrides ChangePropertyName(this XmlAttributeOverrides overrides, string srcName, string targetName, Type instanceType)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(targetName));
            overrides.Add(instanceType, srcName, attrs);
            return overrides;
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI">Type of Instance</typeparam>
        /// <param name="srcName">Source PropertyName</param>
        /// <param name="targetName">Target PropertyName</param>
        /// <param name="overrides">Overrides</param>
        /// <returns>Attached-Overrides</returns>
        public static void ChangePropertyName(this XmlAttributeOverrides overrides, string srcName, string targetName, Type instanceType, Type propertyType)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlElements.Add(new XmlElementAttribute(targetName, propertyType));
            overrides.Add(instanceType, srcName, attrs);
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangePropertiesNames(this XmlAttributeOverrides overrides, IDictionary<string, string> transform, Type instanceType)
        {
            foreach (var srcName in transform.Keys)
            {
                ChangePropertyName(overrides, srcName, transform[srcName], instanceType);
            }
            return overrides;
        }

        /// <summary>
        /// 设置属性名
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangePropertiesNames(this XmlAttributeOverrides overrides, string[][] transform, Type instanceType)
        {
            foreach (var p in transform)
            {
                ChangePropertyName(overrides, p[0], p[1], instanceType);
            }
            return overrides;
        }

        /// <summary>
        /// 隐藏属性
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides HideProperty(this XmlAttributeOverrides overrides, string name, Type instanceType)
        {
            XmlAttributes attrs = new XmlAttributes();
            //attrs.XmlElements.Add(new XmlElementAttribute());
            attrs.XmlIgnore = true;
            overrides.Add(instanceType, name, attrs);
            return overrides;
        }

        /// <summary>
        /// 隐藏属性
        /// </summary>
        /// <typeparam name="TOI"></typeparam>
        /// <param name="overrides"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides HideProperties<TOI>(this XmlAttributeOverrides overrides, Type instanceType, params string[] names)
        {
            foreach (var name in names)
            {
                HideProperty(overrides, name, instanceType);
            }
            return overrides;
        }

        /// <summary>
        /// /// 设置数组和数组项名称
        /// </summary>
        /// <param name="overrides"></param>
        /// <param name="srcArrayName"></param>
        /// <param name="arrayName"></param>
        /// <param name="itemName"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public static XmlAttributeOverrides ChangeArrayPropertyName(this XmlAttributeOverrides overrides, string srcArrayName, string arrayName, string itemName, Type instanceType, Type itemType)
        {
            XmlAttributes attrs = new XmlAttributes();
            attrs.XmlArray = new XmlArrayAttribute(arrayName);
            if (!string.IsNullOrEmpty(itemName)) attrs.XmlArrayItems.Add(new XmlArrayItemAttribute(itemName, itemType));
            overrides.Add(instanceType, srcArrayName, attrs);
            return overrides;
        }
    }
}
