using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Jlion.BrushClient.Framework.Helper
{
    public class AESHelper
    {
        /// <summary>
        /// 获取密钥
        /// </summary>
        private static string AESKey
        {
            get { return @")O[NB]6,YF}+efcaj{+oESb9d8>Z'e9M"; }
        }

        /// <summary>
        /// 获取向量
        /// </summary>
        private static string AESIV
        {
            get { return @"L+\~f4,Ir)b$=pkf"; }
        }


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source, string aesKey)
        {
            var encrypt = string.Empty;
            try
            {
                var aes = Rijndael.Create();
                var bKey = Encoding.UTF8.GetBytes(aesKey);
                var bIV = Encoding.UTF8.GetBytes(AESIV);
                var byteArray = Encoding.UTF8.GetBytes(source);

                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch { }
            if (string.IsNullOrEmpty(encrypt))
                encrypt = source;

            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt(string source, string aesKey)
        {
            var decrypt = string.Empty;
            try
            {
                var aes = Rijndael.Create();
                var bKey = Encoding.UTF8.GetBytes(aesKey);
                var bIV = Encoding.UTF8.GetBytes(AESIV);
                var byteArray = Convert.FromBase64String(source);

                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch { }
            if (string.IsNullOrEmpty(decrypt))
                decrypt = source;

            return decrypt;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Encrypt(string source)
        {
            var encrypt = string.Empty;
            try
            {
                var aes = Rijndael.Create();
                var bKey = Encoding.UTF8.GetBytes(AESKey);
                var bIV = Encoding.UTF8.GetBytes(AESIV);
                var byteArray = Encoding.UTF8.GetBytes(source);

                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch { }
            if (string.IsNullOrEmpty(encrypt))
                encrypt = source;

            return encrypt;
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Decrypt(string source)
        {
            var decrypt = string.Empty;
            try
            {
                var aes = Rijndael.Create();
                var bKey = Encoding.UTF8.GetBytes(AESKey);
                var bIV = Encoding.UTF8.GetBytes(AESIV);
                var byteArray = Convert.FromBase64String(source);

                using (var mStream = new MemoryStream())
                {
                    using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch { }
            if (string.IsNullOrEmpty(decrypt))
                decrypt = source;

            return decrypt;
        }
    }
}
