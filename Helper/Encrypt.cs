using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace ReaderV2.Helper
{
    public abstract class Encrypt
    {


        /// <summary>
        /// 对字节流进行DES加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] EncryptByte(byte[] sourceByte, string key = "Za@$100%", string iv = "395^abC~")
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(key);

                byte[] btIV = Encoding.UTF8.GetBytes(iv);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    //byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            //cs.Write(inData, 0, inData.Length);
                            cs.Write(sourceByte, 0, sourceByte.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }

                        //return Convert.ToBase64String(ms.ToArray());
                        return ms.ToArray();
                        ms.Close();
                    }
                    catch
                    {
                        //return Convert.ToBase64String(sourceByte);
                        return sourceByte;
                    }
                }
            }
            catch { return sourceByte; }
        }

        /// <summary>
        /// 对DES加密的字节流进行解密
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] DecryptByte(byte[] encryptedByte, string key = "Za@$100%",  string iv = "395^abC~")
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(iv);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                //byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedByte, 0, encryptedByte.Length);

                        cs.FlushFinalBlock();
                    }

                    //return Encoding.UTF8.GetString(ms.ToArray());
                    return ms.ToArray();
                }
                catch
                {
                    return encryptedByte;
                }
            }
        }  


        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="sInputString"></param>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <returns></returns>
        public static string EncryptString(string sInputString, string sKey, string sIV)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(sInputString);

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

                DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

                ICryptoTransform desencrypt = DES.CreateEncryptor();

                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);

                return BitConverter.ToString(result);
            }
            catch { }

            return "转换出错！";
        }  

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="sInputString"></param>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        /// <returns></returns>
        public static string DecryptString(string sInputString, string sKey, string sIV)
        {
            try
            {
                string[] sInput = sInputString.Split("-".ToCharArray());

                byte[] data = new byte[sInput.Length];

                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
                }

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();

                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);

                DES.IV = ASCIIEncoding.ASCII.GetBytes(sIV);

                ICryptoTransform desencrypt = DES.CreateDecryptor();

                byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);

                return Encoding.UTF8.GetString(result);
            }
            catch { }

            return "解密出错！";
        }  
    }
}
