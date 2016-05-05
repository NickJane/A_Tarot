using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Lib.Utils
{
    public static class Security
    {
        #region DES缺省值

        private const string DefaultDesKey = "auto@#$&";

        private static readonly byte[] DefaultIV = { 0x12, 0x34, 0x56, 0x78, 
									       0x90, 0xAB, 0xCD, 0xEF,
			                               0xA9, 0x3A, 0xCF, 0xAE,
			                               0xB1, 0xF0, 0xF3, 0x9B
		                                 };
        #endregion

        #region 根据密码创建DESCryptoServiceProvider

        /// <summary>
        /// 获取DES加密算法的提供程序
        /// </summary>
        /// <param name="encryptKey">密钥，长度为8位。如果字符超长则截断，不足8位自动用字符@补充。</param>
        /// <returns>DES加密算法的提供程序</returns>
        private static DESCryptoServiceProvider CreateDESProvider(string encryptKey)
        {
            //如果密钥长度小于8位，自动补足到8位。
            int keyLength = encryptKey.Length;
            if (keyLength < 8)
            {
                for (int i = 0; i < (8 - keyLength); i++)
                {
                    encryptKey += "@";
                }
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, des.KeySize / 8));
            byte[] byIv = new byte[des.BlockSize / 8];
            Array.Copy(DefaultIV, 0, byIv, 0, byIv.Length);
            des.IV = byIv;
            return des;
        }

        #endregion

        #region DES加密字符串

        /// <summary>
        /// 使用默认密码加密字符串
        /// </summary>
        /// <param name="strText">字符串数据</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string strText)
        {
            return Encrypt(strText, DefaultDesKey);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strText">字符串数据</param>
        /// <param name="encryptKey">密钥，长度为8位。如果字符超长则截断，不足8位自动用字符@补充。</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string strText, string encryptKey)
        {
            if (string.IsNullOrEmpty(strText))
            {
                return strText;
            }

            DESCryptoServiceProvider des = CreateDESProvider(encryptKey);
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region DES解密字符串

        /// <summary>
        /// 使用默认密码解密字符串
        /// </summary>
        /// <param name="encryptedText">加了密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string encryptedText)
        {
            return Decrypt(encryptedText, DefaultDesKey);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="encryptedText">加了密的字符串</param>
        /// <param name="decryptKey">密钥，长度为8位。如果字符超长则截断，不足8位自动用字符@补充。</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string encryptedText, string decryptKey)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return encryptedText;
            }

            DESCryptoServiceProvider des = CreateDESProvider(decryptKey);
            byte[] inputByteArray = Convert.FromBase64String(encryptedText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return new UTF8Encoding().GetString(ms.ToArray());
        }
        #endregion
        public static string ToMd5(string str) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
        }
    }
}
