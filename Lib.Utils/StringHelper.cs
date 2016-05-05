using System;
using System.Collections.Generic;
using System.Text;

using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;

namespace Lib.Utils
{
    /// <summary>
    /// 用于进行字符串处理的类
    /// </summary>
    public class StringHelper
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public StringHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion

        #region 获取字符串的实际字节长度的方法
        /// <summary>
        /// 获取字符串的实际字节长度的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>实际长度</returns>
        public static int GetRealLength(string source)
        {
            return Encoding.Default.GetByteCount(source);
        }
        #endregion

        #region 按字节数截取字符串的方法
        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="source">要截取的字符串</param>
        /// <param name="n">要截取的字节数</param>
        /// <param name="needEndDot">是否需要结尾的省略号</param>
        /// <returns>截取后的字符串</returns>
        public static string SubString(string source, int n, bool needEndDot)
        {
            string temp = string.Empty;
            if (GetRealLength(source) <= n)//如果长度比需要的长度n小,返回原字符串
            {
                return source;
            }
            else
            {
                int t = 0;
                char[] q = source.ToCharArray();
                for (int i = 0; i < q.Length && t < n; i++)
                {
                    if ((int)q[i] > 127)//是否汉字
                    {
                        temp += q[i];
                        t += 2;
                    }
                    else
                    {
                        temp += q[i];
                        t++;
                    }
                }
                if (needEndDot)
                    temp += "...";
                return temp;
            }
        }
        #endregion

        #region 去除价格小数点后末尾0的方法
        /// <summary>
        /// 去除价格小数点后末尾0的方法
        /// </summary>
        /// <param name="price">去0之前的价格字符串</param>
        /// <returns>去0之后的价格字符串</returns>
        public static string TrimEndZeroForPrice(string price)
        {
            while (price.EndsWith("0") && price.IndexOf(".") > 0)
            {
                price = price.TrimEnd('0');
            }
            if (price.EndsWith("."))
                price = price.Substring(0, price.Length - 1);
            return price;
        }
        #endregion

        #region 截取规定小数点后位数的方法
        /// <summary>
        /// 截取规定小数点后位数的方法
        /// </summary>
        /// <param name="objDecimal">截取前的小数对象</param>
        /// <param name="length">要截取的小数位长度</param>
        /// <returns>截取后的小数字符串</returns>
        public static string SubSpecialLengthDecimal(object objDecimal, int length)
        {
            decimal strDecimal = ConvertHelper.GetDecimal(objDecimal);
            return strDecimal.ToString("f" + length);
        }
        #endregion

        #region 获取8位当前日期字符串的方法
        /// <summary>
        /// 获取8位当前日期字符串的方法
        /// </summary>
        /// <returns>8位当前日期字符串</returns>
        public static string GetCurrentDateString()
        {
            DateTime now = DateTime.Now;
            return string.Concat(now.Year, GetSpecialNumericString(now.Month, 2), GetSpecialNumericString(now.Day, 2));
        }
        #endregion

        #region 获取6位当前日期字符串的方法
        /// <summary>
        /// 获取6位当前日期字符串的方法
        /// </summary>
        /// <returns>6位当前日期字符串</returns>
        public static string GetSmallCurrentDateString()
        {
            string currentDateString = GetCurrentDateString();
            return currentDateString.Substring(2);
        }
        #endregion

        #region 获取指定长度数字字符串的方法，不足位数用0填充
        /// <summary>
        /// 获取指定长度数字字符串的方法，不足位数用0填充
        /// </summary>
        /// <param name="number">数字</param>
        /// <param name="length">指定的长度</param>
        /// <returns>指定长度数字字符串</returns>
        public static string GetSpecialNumericString(int number, int length)
        {
            return number.ToString("d" + length);
        }
        #endregion

        #region 移除数字字符串开始0的方法
        /// <summary>
        /// 移除数字字符串开始0的方法
        /// </summary>
        /// <param name="source">移除前的字符串</param>
        /// <returns>移除后的字符串</returns>
        public static string TrimStartZero(string source)
        {
            while (source.StartsWith("0"))
            {
                source = source.Substring(1);
            }
            return source;
        }
        #endregion

        #region 转换字符串编码的方法
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding dstEncoding, string s)
        {
            return ConvertEncoding(Encoding.Default, dstEncoding, s);
        }
        /// <summary>
        /// 转换字符串编码的方法
        /// </summary>
        /// <param name="srcEncoding">转换前的编码格式</param>
        /// <param name="dstEncoding">转换后的编码格式</param>
        /// <param name="s">要进行转换的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertEncoding(Encoding srcEncoding, Encoding dstEncoding, string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            bytes = Encoding.Convert(srcEncoding, dstEncoding, bytes);
            return Encoding.Default.GetString(bytes);
        }
        #endregion

        #region 将全角字符串转成半角字符串的方法
        /// <summary>
        /// 将全角字符串转成半角字符串的方法
        /// </summary>
        /// <param name="source">字符串</param>
        /// <returns>半角字符串</returns>
        public static string ConvertDbcToSbcString(string source)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = source.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                sb.Append(ConvertDbcToSbcChar(c[i]));
            }
            return sb.ToString();
        }
        #endregion

        #region 将字符串数组转成逗号分割字符串的方法
        /// <summary>
        /// 将字符串数组转成逗号分割字符串的方法
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns>逗号分割字符串</returns>
        public static string ConvertStringArrayToStrings(string[] strArray)
        {
            if (strArray == null)
            {
                return "";
            }

            string result = "";
            for (int i = 0; i < strArray.Length; i++)
            {
                result += strArray[i] + ",";
            }
            return result.TrimEnd(new char[] { ',' });
        }
        #endregion

        #region 对字符串进行Url编码的方法
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s)
        {
            return UrlEncode(s, Encoding.Default);
        }
        /// <summary>
        /// 对字符串进行Url编码的方法
        /// </summary>
        /// <param name="s">要进行Url编码的字符串</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>Url编码后的字符串</returns>
        public static string UrlEncode(string s, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = encoding.GetBytes(s);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
        #endregion

        #region 生成随机的数字
        /// <summary>
        /// 生成随机的数字
        /// </summary>
        /// <param name="intLength">生成字母的个数</param>
        /// <returns>string</returns>
        public static string GenerateRndNum(int intLength)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(',');
            string VNum = ""; //由于字符串很短，就不用StringBuilder了

            int temp = -1; //记录上次随机数值，尽量避免生产几个一样的随机数


            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < intLength + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(VcArray.Length);
                if (temp != -1 && temp == t)
                {
                    return GenerateRndNum(intLength);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }
        #endregion

        #region 验证字符串是否为空
        /// <summary>
        /// 验证字符串是否为空
        /// </summary>
        /// <param name="str">被判断的字符串</param>
        /// <returns>bool值</returns>
        public static bool IsEmptyString(string str)
        {
            if (str == null || str.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 过滤脚本攻击:sql注入,跨站脚本,flash嵌入

        /// <summary>
        /// 过滤字符串中注入SQL脚本的方法
        /// </summary>
        /// <param name="source">传入的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string SqlFilter(string source)
        {
            //半角括号替换为全角括号
            source = source.Replace("'", "''").Replace(";", "；").Replace("(", "（").Replace(")", "）");

            //去除执行SQL语句的命令关键字
            source = Regex.Replace(source, "select", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "insert", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "update", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "delete", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "drop", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "truncate", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "declare", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "xp_cmdshell", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "/add", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "asc(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "mid(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "char(", "", RegexOptions.IgnoreCase);
            //Regex.Replace(source, "count(", "", RegexOptions.IgnoreCase);
            //fetch 
            //IS_SRVROLEMEMBER
            //Cast(

            source = Regex.Replace(source, "net user", "", RegexOptions.IgnoreCase);

            //去除执行存储过程的命令关键字 
            source = Regex.Replace(source, "exec", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "execute", "", RegexOptions.IgnoreCase);

            //去除系统存储过程或扩展存储过程关键字
            source = Regex.Replace(source, "xp_", "x p_", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "sp_", "s p_", RegexOptions.IgnoreCase);

            //防止16进制注入
            source = Regex.Replace(source, "0x", "0 x", RegexOptions.IgnoreCase);

            return source;
        }

        /// <summary>
        /// 过滤字符串中的注入跨站脚本(先进行UrlDecode再过滤脚本关键字)
        /// </summary>
        /// <param name="source">需要过滤的字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string XSSFilter(string source)
        {
            if (source == "") return source;

            string result = HttpUtility.UrlDecode(source);

            string replaceEventStr = " onXXX =";
            string tmpStr = "";

            string patternGeneral = @"<[^<>]*>";                              //例如 <abcd>
            string patternEvent = @"([\s]|[:])+[o]{1}[n]{1}\w*\s*={1}";     // 空白字符或: on事件=
            string patternScriptBegin = @"\s*((javascript)|(vbscript))\s*[:]?";  // javascript或vbscript:
            string patternScriptEnd = @"<([\s/])*script.*>";                       // </script>   
            string patternTag = @"<([\s/])*\S.+>";                       // 例如</textarea>

            Regex regexGeneral = new Regex(patternGeneral, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexEvent = new Regex(patternEvent, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Regex regexScriptEnd = new Regex(patternScriptEnd, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexScriptBegin = new Regex(patternScriptBegin, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex regexTag = new Regex(patternTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);


            Match matchGeneral, matchEvent, matchScriptEnd, matchScriptBegin, matchTag;

            //符合类似 <abcd> 条件的
            #region 符合类似 <abcd> 条件的
            //过滤字符串中的 </script>   
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptEnd = regexScriptEnd.Match(tmpStr);
                if (matchScriptEnd.Success)
                {
                    tmpStr = regexScriptEnd.Replace(tmpStr, " ");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的脚本事件
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchEvent = regexEvent.Match(tmpStr);
                if (matchEvent.Success)
                {
                    tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            //过滤字符串中的 javascript或vbscript:
            for (matchGeneral = regexGeneral.Match(result); matchGeneral.Success; matchGeneral = matchGeneral.NextMatch())
            {
                tmpStr = matchGeneral.Groups[0].Value;
                matchScriptBegin = regexScriptBegin.Match(tmpStr);
                if (matchScriptBegin.Success)
                {
                    tmpStr = regexScriptBegin.Replace(tmpStr, " ");
                    result = result.Replace(matchGeneral.Groups[0].Value, tmpStr);
                }
            }

            #endregion

            //过滤字符串中的事件 例如 onclick --> onXXX
            for (matchEvent = regexEvent.Match(result); matchEvent.Success; matchEvent = matchEvent.NextMatch())
            {
                tmpStr = matchEvent.Groups[0].Value;
                tmpStr = regexEvent.Replace(tmpStr, replaceEventStr);
                result = result.Replace(matchEvent.Groups[0].Value, tmpStr);
            }

            //过滤掉html标签，类似</textarea>
            for (matchTag = regexTag.Match(result); matchTag.Success; matchTag = matchTag.NextMatch())
            {
                tmpStr = matchTag.Groups[0].Value;
                tmpStr = regexTag.Replace(tmpStr, "");
                result = result.Replace(matchTag.Groups[0].Value, tmpStr);
            }


            return result;
        }

        /// <summary>
        /// 过滤字符串中注入Flash代码
        /// </summary>
        /// <param name="htmlCode">输入字符串</param>
        /// <returns>过滤后的字符串</returns>
        public static string FlashFilter(string htmlCode)
        {
            string pattern = @"\w*<OBJECT\s+.*(macromedia)[\s*|\S*]{1,1300}</OBJECT>";

            return Regex.Replace(htmlCode, pattern, "", RegexOptions.Multiline);
        }

        #endregion

        #region 移除Html标签的方法
        /// <summary>
        /// 移除html标记
        /// </summary>
        /// <param name="source">移除Html标签之前的字符串</param>
        /// <returns>移除Html标签之后的字符串</returns>
        public static string RemoveHtmlTag(string source)
        {
            return Regex.Replace(source, "<[^>]*>", "");
        }
        #endregion

        #region 读取指定URL的内容
        /// <summary>
        /// 读取指定URL的内容
        /// </summary>
        /// <param name="URL">指定URL</param>
        /// <param name="Content">该URL包含的内容</param>
        /// <returns>读取URL的状态</returns>
        public static string ReadHttp(string URL, ref string Content)
        {
            string status = "ERROR";
            HttpWebRequest Webreq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse Webresp = null;
            StreamReader strm = null;
            try
            {
                Webresp = (HttpWebResponse)Webreq.GetResponse();
                status = Webresp.StatusCode.ToString();
                strm = new StreamReader(Webresp.GetResponseStream(), Encoding.GetEncoding("GB2312"));
                Content = strm.ReadToEnd();
            }
            catch
            {
            }
            finally
            {
                if (Webresp != null) Webresp.Close();
                if (strm != null) strm.Close();
            }
            return (status);
        }
        #endregion

        #region 把浮点类型格式化为百分比字符串
        /// <summary>
        /// 把浮点类型格式化为百分比字符串
        /// </summary>
        /// <param name="num">浮点类型数据</param>
        /// <returns>百分比字符串</returns>
        public static string FormatDoubleToPercent(double num)
        {
            return (num * 100).ToString("#0.00") + "%";
        }
        #endregion

        /// <summary>
        /// 获取用户友好的日期格式
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>字符串</returns>
        public static string GetHumanFriendDate(DateTime dt, bool isLongTime = true)
        {
            TimeSpan ts = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1) - dt;
            TimeSpan totalSpan = DateTime.Now - dt;
            string dateView = "";
            switch (ts.Days)
            {
                case 0:
                    if (totalSpan.TotalMinutes <= 0)
                    {
                        dateView = "刚刚";
                    }
                    else if (totalSpan.TotalMinutes <= 1)
                    {
                        dateView = totalSpan.Seconds + "秒钟前";
                    }
                    else if (totalSpan.TotalHours <= 1)
                    {
                        dateView = totalSpan.Minutes + "分钟前";
                    }
                    else
                    {
                        dateView = totalSpan.Hours + "小时前";
                    }
                    break;
                case 1:
                    dateView = dt.ToString("昨天 HH:mm");
                    break;
                case 2:
                    dateView = dt.ToString("前天 HH:mm");
                    break;
                default:
                    if (!isLongTime)
                    {
                        dateView = dt.ToString("MM-dd");
                    }
                    else
                    {
                        dateView = dt.ToString("yyyy-MM-dd HH:mm");
                    }
                    break;
            }
            return dateView;
        }
        #region 私有方法
        /// <summary>
        /// 将全角字符转成半角字符的方法
        /// </summary>
        /// <param name="c">转换前的字符</param>
        /// <returns>半角字符</returns>
        private static char ConvertDbcToSbcChar(char c)
        {
            //得到c的编码
            byte[] bytes = Encoding.Unicode.GetBytes(c.ToString());

            int H = Convert.ToInt32(bytes[1]);
            int L = Convert.ToInt32(bytes[0]);

            //得到unicode编码
            int value = H * 256 + L;

            //是全角
            if (value >= 65281 && value <= 65374)
            {
                int halfvalue = value - 65248;//65248是全半角间的差值。
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else if (value == 12288)
            {
                int halfvalue = 32;
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else
            {
                return c;
            }

            //将bytes转换成字符
            string ret = Encoding.Unicode.GetString(bytes);

            return Convert.ToChar(ret);
        }
        #endregion
    }
}