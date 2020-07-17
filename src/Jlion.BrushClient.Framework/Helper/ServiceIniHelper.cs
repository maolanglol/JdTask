using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Jlion.BrushClient.Framework.Enums;

namespace Jlion.BrushClient.Framework.Helper
{
    /// <summary>
    /// .ini 文件处理帮助类
    /// </summary>
    public class ServiceIniHelper
    {

        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern uint GetPrivateProfileStringList(string section, string key, string def, Byte[] retVal, int size, string filePath);

        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="sectionName">配置节</param>
        /// <param name="key">键名</param>
        /// <param name="value">键值</param>
        public static void Write(string sectionName, string key, string value, string root = "", EnumIniFileType fileType = EnumIniFileType.Upgrade)
        {
            if (string.IsNullOrEmpty(root))
                root = AppDomain.CurrentDomain.BaseDirectory;

            var sPath = Path.Combine(root, $"{fileType.ToString().ToLower()}.ini");
            if (!File.Exists(sPath))
            {
                using (FileStream fs = File.Create(sPath)) { }
            }
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(sectionName, key, value, sPath);
        }

        public static void Clear(string root = "", EnumIniFileType fileType = EnumIniFileType.Upgrade)
        {
            if (string.IsNullOrEmpty(root))
                root = AppDomain.CurrentDomain.BaseDirectory;

            var sPath = Path.Combine(root, $"{fileType.ToString().ToLower()}.ini");
            if (File.Exists(sPath))
            {
                File.Delete(sPath);
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Read(string sectionName, string key, string root = "", EnumIniFileType fileType = EnumIniFileType.Upgrade, int length = 1000)
        {
            if (string.IsNullOrEmpty(root))
                root = AppDomain.CurrentDomain.BaseDirectory;

            var sPath = Path.Combine(root, $"{fileType.ToString().ToLower()}.ini");
            if (!File.Exists(sPath)) { return null; }
            // 每次从ini中读取多少字节
            var temp = new StringBuilder(length);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(sectionName, key, "", temp, length, sPath);
            return temp.ToString();
        }

        /// <summary>
        /// 读取所有项
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="root"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public static List<string> ReadKeys(string sectionName, string root = "", EnumIniFileType fileType = EnumIniFileType.Upgrade)
        {
            if (string.IsNullOrEmpty(root))
                root = AppDomain.CurrentDomain.BaseDirectory;

            var sPath = Path.Combine(root, $"{fileType.ToString().ToLower()}.ini");

            List<string> result = new List<string>();
            Byte[] buf = new Byte[65536];
            uint len = GetPrivateProfileStringList(sectionName, null, null, buf, buf.Length, sPath);
            int j = 0;
            for (int i = 0; i < len; i++)
            {
                if (buf[i] == 0)
                {
                    result.Add(Encoding.Default.GetString(buf, j, i - j));
                    j = i + 1;
                }
            }
            return result;
        }
    }
}
