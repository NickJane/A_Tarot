using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Xml;
using System.IO;

namespace Lib.Utils
{
    /// <summary>
    /// 用于把对象型数据转成特定数据类型的类
    /// </summary>
    public class ConvertHelper
    {
        #region 将对象变量转成字符串变量的方法
        /// <summary>
        /// 将对象变量转成字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>字符串变量</returns>
        public static string GetString(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? "" : obj.ToString();
        }
        #endregion

        #region 将对象变量转成32位整数型变量的方法
        /// <summary>
        /// 将对象变量转成32位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>32位整数型变量</returns>
        public static int GetInteger(object obj)
        {
            return ConvertStringToInteger(GetString(obj));
        }
        #endregion

        #region 将对象变量转成短整型变量的方法

        /// <summary>
        /// 将对象变量转成短整型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>短整型变量</returns>
        public static short GetShortInt(object obj)
        {
            return ConvertStringToShortInt(GetString(obj));
        }
        #endregion

        #region 将对象变量转成64位整数型变量的方法
        /// <summary>
        /// 将对象变量转成64位整数型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>64位整数型变量</returns>
        public static long GetLong(object obj)
        {
            return ConvertStringToLong(GetString(obj));
        }
        #endregion

        #region 将对象变量转成双精度浮点型变量的方法
        /// <summary>
        /// 将对象变量转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>双精度浮点型变量</returns>
        public static double GetDouble(object obj)
        {
            return ConvertStringToDouble(GetString(obj));
        }
        #endregion

        #region 将对象变量转成十进制数字变量的方法
        /// <summary>
        /// 将对象变量转成十进制数字变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>十进制数字变量</returns>
        public static decimal GetDecimal(object obj)
        {
            return ConvertStringToDecimal(GetString(obj));
        }
        #endregion

        #region 将对象变量转成布尔型变量的方法
        /// <summary>
        /// 将对象变量转成布尔型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>布尔型变量</returns>
        public static bool GetBoolean(object obj)
        {
            return (obj == DBNull.Value || obj == null) ? false :
                GetString(obj).ToLower() == "true" ? true : false;
        }
        #endregion

        #region 将对象变量转成日期时间型字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期时间型字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <param name="sFormat">时间字符串格式，例：yyyy-MM-dd</param>
        /// <returns>时间型字符串变量</returns>
        public static string GetDateTimeString(object obj, string sFormat)
        {
            return GetDateTime(obj).ToString(sFormat);
        }
        #endregion

        #region 将对象变量转成日期字符串变量的方法
        /// <summary>
        /// 将对象变量转成日期字符串变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期字符串变量</returns>
        public static string GetShortDateString(object obj)
        {
            return GetDateTimeString(obj, "yyyy-MM-dd");
        }
        #endregion

        #region 将对象变量转成日期型变量的方法
        /// <summary>
        /// 将对象变量转成日期型变量的方法
        /// </summary>
        /// <param name="obj">对象变量</param>
        /// <returns>日期型变量</returns>
        public static DateTime GetDateTime(object obj)
        {
            DateTime result;
            DateTime.TryParse(GetString(obj), out result);
            return result;
        }
        #endregion

        #region 私有方法

        #region 将字符串转成32位整数型变量的方法
        /// <summary>
        /// 将字符串转成32位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>32位整数型变量</returns>
        private static int ConvertStringToInteger(string s)
        {
            int result;
            int.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成短整型变量的方法

        /// <summary>
        /// 将字符串转成短整型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>短整型变量</returns>
        private static short ConvertStringToShortInt(string s)
        {
            short result;
            short.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成64位整数型变量的方法
        /// <summary>
        /// 将字符串转成64位整数型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>64位整数型变量</returns>
        private static long ConvertStringToLong(string s)
        {
            long result;
            long.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成双精度浮点型变量的方法
        /// <summary>
        /// 将字符串转成双精度浮点型变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>双精度浮点型变量</returns>
        private static double ConvertStringToDouble(string s)
        {
            double result;
            double.TryParse(s, out result);
            return result;
        }
        #endregion

        #region 将字符串转成十进制数字变量的方法
        /// <summary>
        /// 将字符串转成十进制数字变量的方法
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>十进制数字变量</returns>
        private static decimal ConvertStringToDecimal(string s)
        {
            decimal result;
            decimal.TryParse(s, out result);
            return result;
        }
        #endregion

        #endregion

        #region 将DataTable转换成xml格式数据
        /// <summary>
        /// 将DataTable转换成xml格式数据
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <returns>xml结果集</returns>
        public static string ConvertDataTableToXml(DataTable dt)
        {
            var returnValue = string.Empty;
            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter xmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    xmlWt = new XmlTextWriter(ms, Encoding.Unicode);
                    //获取ds中的数据
                    dt.WriteXml(xmlWt);
                    var count = (int)ms.Length;
                    var temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    var ucode = new UnicodeEncoding();
                    returnValue = ucode.GetString(temp).Trim();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (xmlWt != null)
                    {
                        xmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            return returnValue;
        }
        #endregion

        #region  将xml字符串转换成DataSet
        /// <summary>
        /// 将xml字符串转换成DataSet
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>转换后的DataSet</returns>
        public static DataSet ConvertXmlToDataSet(string xmlStr)
        {
            if (!string.IsNullOrEmpty(xmlStr))
            {
                StringReader strStream = null;
                XmlTextReader xmlrdr = null;
                try
                {
                    var ds = new DataSet();
                    //读取字符串中的信息
                    strStream = new StringReader(xmlStr);
                    //获取StrStream中的数据
                    xmlrdr = new XmlTextReader(strStream);
                    //ds获取Xmlrdr中的数据                
                    ds.ReadXml(xmlrdr);
                    return ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    //释放资源
                    if (xmlrdr != null)
                    {
                        xmlrdr.Close();
                        strStream.Close();
                        strStream.Dispose();
                    }
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region xml转换datatable
        /// <summary>
        /// xml转换datatable
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <param name="tableIndex">表格索引</param>
        /// <returns>转换后的DataTable</returns>
        public static DataTable ConvertXmlToDataTable(string xmlStr, int tableIndex)
        {
            var ds = ConvertXmlToDataSet(xmlStr);
            if (ds != null && ds.Tables.Count > tableIndex)
            {
                return ds.Tables[tableIndex];
            }
            return null;
        }
        #endregion

        #region xml转换datatable
        /// <summary>
        ///  xml转换datatable
        /// </summary>
        /// <param name="xmlStr">xml字符串</param>
        /// <returns>转换后的DataTable</returns>
        public static DataTable ConvertXmlToDataTable(string xmlStr)
        {
            return ConvertXmlToDataTable(xmlStr, 0);
        }
        #endregion

    }
}