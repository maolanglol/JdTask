using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Jlion.BrushClient.Framework.Helper
{
    public static class Md5Helper
    {
        /// <summary>
        /// 获取md5值
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>md5</returns>
        public static string GetMd5(byte[] bytes)
        {
            string result = null;

            if (bytes != null && bytes.Length > 0)
            {
                var md5 = MD5.Create();
                var bytResult = md5.ComputeHash(bytes);

                //字节类型的数组转换为字符串 
                for (int i = 0; i < bytResult.Length; i++)
                {
                    //16进制转换 
                    result += string.Format("{0:x}", bytResult[i]).PadLeft(2, '0');
                }
            }
            return result;
        }

        /// <summary>
        /// 获取md5
        /// </summary>
        /// <param name="content">编码值</param>
        /// <returns></returns>
        public static string GetMd5(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }
            return GetMd5(Encoding.UTF8.GetBytes(content));
        }
    }
}
