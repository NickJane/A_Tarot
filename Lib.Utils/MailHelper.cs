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
	/// �����ʼ�����
	/// </summary>
	public class MailHelper
	{
        #region �����ʼ��ķ���
        /// <summary>
        /// �����ʼ��ķ���
        /// </summary>
        /// <param name="strSmtpServer">�ʼ���������ַ</param>
        /// <param name="strFrom">���͵�ַ</param>
        /// <param name="strFromPass">��������</param>
        /// <param name="strto">���յ�ַ</param>
        /// <param name="strSubject">�ʼ�����</param>
        /// <param name="strBody">�ʼ�����</param>
        /// <param name="isHtmlFormat">�ʼ������Ƿ���html��ʽ����</param>
        public static void SendEmail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, bool isHtmlFormat)
        {
            SendEmail(strSmtpServer, strFrom, strFromPass, strto, strSubject, strBody, isHtmlFormat, null);
        }

        /// <summary>
        /// �����ʼ��ķ���
        /// </summary>
        /// <param name="strSmtpServer">�ʼ���������ַ</param>
        /// <param name="strFrom">���͵�ַ</param>
        /// <param name="strFromPass">��������</param>
        /// <param name="strto">���յ�ַ</param>
        /// <param name="strSubject">�ʼ�����</param>
        /// <param name="strBody">�ʼ�����</param>
        /// <param name="isHtmlFormat">�ʼ������Ƿ���html��ʽ����</param>
        /// <param name="files">�����ļ��ļ���</param>
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
                throw new Exception("�����ʼ�ʧ�ܡ�������Ϣ��" + ex.Message);
            }
        }
        #endregion

        #region �첽�����ʼ��ķ���
        /// <summary>
        /// �첽�����ʼ��ķ���
        /// </summary>
        /// <param name="strSmtpServer">�ʼ���������ַ</param>
        /// <param name="strFrom">���͵�ַ</param>
        /// <param name="strFromPass">��������</param>
        /// <param name="strto">���յ�ַ</param>
        /// <param name="strSubject">�ʼ�����</param>
        /// <param name="strBody">�ʼ�����</param>
        /// <param name="isHtmlFormat">�ʼ������Ƿ���html��ʽ����</param>
        /// <param name="files">�����ļ��ļ���</param>
        /// <param name="userToken">һ���û�������󣬴˶��󽫱����ݸ�����첽����ʱ�����õķ�����</param>
        /// <param name="onComplete">���ͽ�����Ļص�����</param>
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
                
                //���ʼ���������¼�
                client.SendCompleted += new SendCompletedEventHandler(onComplete);
                
                //�첽����
                client.SendAsync(message,userToken );
            }
            catch (Exception ex)
            {
                throw new Exception("�����ʼ�ʧ�ܡ�������Ϣ��" + ex.Message);
            }
        }
        #endregion
	}
}