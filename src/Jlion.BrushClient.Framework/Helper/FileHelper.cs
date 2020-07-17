using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Jlion.BrushClient.Framework.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void Rename(string oldPath, string newPath)
        {
            try
            {
                var file = new FileInfo(oldPath);
                file.MoveTo(newPath);
            }
            catch (Exception ex)
            {
                TextHelper.Error($"Rename 异常 oldPath:{oldPath},newPath:{newPath}", ex);
            }
        }

        public static void Copy(string oldPath,string newPath)
        {
            try
            {
                if (File.Exists(oldPath))//必须判断要复制的文件是否存在
                {
                    File.Copy(oldPath, newPath, true);//三个参数分别是源文件路径，存储路径，若存储路径有相同文件是否替换
                }
            }
            catch (Exception ex)
            {
                TextHelper.Error($"Copy 异常 oldPath:{oldPath},newPath:{newPath}", ex);
            }
        }

        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Read(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return "";
                return File.ReadAllText(path, Encoding.Default);
            }
            catch { }
            return "";
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsExists(string path)
        {
            try
            {
                return File.Exists(path);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool CreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static List<string> GetExeFiles(string path, string extension = ".exe")
        {
            var resp = new List<string>();
            try
            {
                var root = new DirectoryInfo(path);
                var files = root.GetFiles();
                foreach (var item in files)
                {
                    if (item.Extension.Equals(extension))
                    {
                        resp.Add(item.Name);
                    }
                }
            }
            catch
            {

            }
            return resp;
        }

        public static void DeleteFile(List<string> exclude, string path, string extension = ".exe")
        {
            try
            {
                var exefileList = GetExeFiles(path, extension);
                exefileList?.ForEach(item =>
                {
                    if (!exclude.Contains(item))
                    {
                        File.Delete($"{path}{item}");
                    }
                });
            }
            catch (Exception ex)
            {
                TextHelper.Error($"DeleteFile 异常 path:{path}", ex);
            }
        }
    }
}
