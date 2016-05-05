using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Lib.Utils
{
	/// <summary>
	/// 发送邮件的类
	/// </summary>
	public class MailHelper
	{
        #region 发送邮件的方法
        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strSmtpServer">邮件服务器地址</param>
        /// <param name="strFrom">发送地址</param>
        /// <param name="strFromPass">发送密码</param>
        /// <param name="strto">接收地址</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat)
        {
            SendEmail(strSmtpServer, strFrom, strFromPass, strto, strSubject, strBody, isHtmlFormat, null);
        }

        /// <summary>
        /// 发送邮件的方法
        /// </summary>
        /// <param name="strSmtpServer">邮件服务器地址</param>
        /// <param name="strFrom">发送地址</param>
        /// <param name="strFromPass">发送密码</param>
        /// <param name="strto">接收地址</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        /// <param name="files">附件文件的集合</param>
        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat, string[] files)
        {
            try
            {
                SmtpClient client = new SmtpClient(strSmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(strFrom, strFromPass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
                message.BodyEncoding = Encoding.Default;
                message.IsBodyHtml = isHtmlFormat;

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (File.Exists(files[i]))
                        {
                            message.Attachments.Add(new Attachment(files[i]));
                        }
                    }
                }

                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception("发送邮件失败。错误信息：" + ex.Message);
            }
        }
        #endregion

        #region 异步发送邮件的方法
        /// <summary>
        /// 异步发送邮件的方法
        /// </summary>
        /// <param name="strSmtpServer">邮件服务器地址</param>
        /// <param name="strFrom">发送地址</param>
        /// <param name="strFromPass">发送密码</param>
        /// <param name="strto">接收地址</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="isHtmlFormat">邮件内容是否以html格式发送</param>
        /// <param name="files">附件文件的集合</param>
        /// <param name="userToken">一个用户定义对象，此对象将被传递给完成异步操作时所调用的方法。</param>
        /// <param name="onComplete">发送结束后的回调函数</param>
        public static void SendAsyncEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat, string[] files, object  userToken, SendCompletedEventHandler onComplete)
        {
            try
            {
                SmtpClient client = new SmtpClient(strSmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(strFrom, strFromPass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
                message.BodyEncoding = Encoding.Default;
                message.IsBodyHtml = isHtmlFormat;

                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        if (File.Exists(files[i]))
                        {
                            message.Attachments.Add(new Attachment(files[i]));
                        }
                    }
                } 
                
                //绑定邮件发送完成事件
                client.SendCompleted += new SendCompletedEventHandler(onComplete);
                
                //异步发送
                client.SendAsync(message,userToken );
            }
            catch (Exception ex)
            {
                throw new Exception("发送邮件失败。错误信息：" + ex.Message);
            }
        }
        #endregion
	}
}