using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// 该加密算法跟服务端得一致,用于跟服务端得加解密对称使用
    /// </summary>
    public class DESHelper
    {

        static string _secretKey = "wpjix9s6";

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DESEncrypt(string input)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                des.Key = Encoding.ASCII.GetBytes(_secretKey);
                des.IV = Encoding.ASCII.GetBytes(_secretKey);
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string DESDecrypt(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = Encoding.ASCII.GetBytes(_secretKey);
                des.IV = Encoding.ASCII.GetBytes(_secretKey);
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
